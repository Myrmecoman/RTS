using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class AgentManager : Selectable
{
    public int supply = 1;
    public int attackDamage = 5;
    public int attackRange = 5;
    public float attackSpeed = 1;
    public float speed = 1.0f;
    public bool canAttackGround = true;
    public bool canAttackAir = true;

    public Transform leftPart;
    public Transform rightPart;

    // variables for personnal pathfinding
    private JobHandle handle;
    private NativeQueue<DijkstraTile> toVisit;
    private int agroRange;
    private int quitAgroRange;
    private double delay = 0;
    private bool useOwnGrid = false;

    // info variables
    private bool hasDestination = false;
    private bool holdPosition = false;
    private bool attackCommand = false;
    private double attackCooldown = 0;
    private Rigidbody rb;
    private Transform follow;
    private ResourceManager ressource = null;

    // variables to know when destination reached
    private bool directView = false;
    private Vector3 targetDirectDirection;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        ownGrid = new NativeArray<DijkstraTile>(PathRegister.instance.iGridSizeX * PathRegister.instance.iGridSizeY, Allocator.Persistent);
        toVisit = new NativeQueue<DijkstraTile>(Allocator.Persistent);

        if (attackRange > 8)
            agroRange = attackRange;
        else
            agroRange = 8;

        quitAgroRange = agroRange + 2;
    }


    private void FixedUpdate()
    {
        // attack cooldown
        if (attackCooldown > 0)
            attackCooldown -= Time.fixedDeltaTime;

        // attack reachable targets
        Selectable foundTarget = CheckReachable(); // VERY INTENSIVE : about 0.1ms, so 100ms for 1000 units
        float closestEnemyDist = 1000f;
        if (foundTarget != null)
            closestEnemyDist = Vector3.Distance(transform.position, foundTarget.GetComponent<Collider>().ClosestPoint(transform.position));

        if (attackCooldown <= 0 && (!hasDestination || attackCommand || holdPosition) && ressource == null && closestEnemyDist <= attackRange)
        {
            Attack(foundTarget);
            rb.isKinematic = true;
            return;
        }

        // attacking, check if needing to hold position
        if ((attackCommand || holdPosition) && closestEnemyDist <= attackRange && ressource == null)
        {
            rb.isKinematic = true;
            return;
        }
        else if (ressource == null)
            rb.isKinematic = false;

        // if can't move, cannot take agro or move so stop now
        if (holdPosition)
            return;

        // check if nearby enemy to take focus on if we do not focus fire already
        if ((attackCommand || !hasDestination) && closestEnemyDist > attackRange)
            useOwnGrid = ShouldAgro(foundTarget, closestEnemyDist);

        // exit if no destination before move function
        if (!hasDestination)
            return;

        // getting distance to destination
        float horizontalDist;
        if (follow == null)
            horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(destination.x, 0, destination.z));
        else
            horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(follow.position.x, 0, follow.position.z));

        // if we already reached our target, stop now
        if (horizontalDist <= 0.05f && follow == null)
        {
            UnsetDestination(true);
            if (ressource != null)
                HoldPosition();
            return;
        }

        if (destination != null)
            MoveAndRotate(horizontalDist);
    }


    private bool ShouldAgro(Selectable foundPotentialTarget, float closestEnemyDist)
    {
        // waiting but nothing in zone yet
        if (!useOwnGrid && closestEnemyDist >= agroRange)
            return false;

        // got too far, leave agro
        if (useOwnGrid && closestEnemyDist >= quitAgroRange)
            return false;

        // recalculate every second
        if (Time.realtimeSinceStartupAsDouble - delay < 1)
            return true;

        hasDestination = true;
        attackCommand = true;
        useOwnGrid = true;
        follow = foundPotentialTarget.transform;
        delay = Time.realtimeSinceStartupAsDouble;

        // clear grid
        PathRegister.instance.cleanGrid.CopyTo(ownGrid);

        // dijkstra
        var jobDataDij = new DijkstraRestrainedJob
        {
            target = NodeFromWorldPoint(foundPotentialTarget.transform.position, true),
            gridSize = new int2(PathRegister.instance.iGridSizeX, PathRegister.instance.iGridSizeY),
            toVisit = toVisit,
            grid = ownGrid,
            maxDistance = quitAgroRange + 5
        };
        jobDataDij.Run();

        // flowfield
        NativeArray<DijkstraTile> tempDijkstra = new NativeArray<DijkstraTile>(ownGrid, Allocator.TempJob);
        var jobData = new FlowFieldRestrainedJob
        {
            gridSize = new int2(PathRegister.instance.iGridSizeX, PathRegister.instance.iGridSizeY),
            RdGrid = tempDijkstra,
            grid = ownGrid
        };
        handle = jobData.Schedule(ownGrid.Length, 8 /* batches */);
        handle.Complete();
        tempDijkstra.Dispose();

        //Debug.Log("total : " + (Time.realtimeSinceStartupAsDouble - delay) * 1000 + "ms\n");

        return true;
    }


    private void MoveAndRotate(float horizontalDist)
    {
        float3 leftMostPart = new float3(leftPart.position.x, -50, leftPart.position.z);
        float3 rightMostPart = new float3(rightPart.position.x, -50, rightPart.position.z);
        float3 targetPosition;
        if (follow == null)
            targetPosition = new float3(destination.x, -50, destination.z);
        else
            targetPosition = new float3(follow.position.x, -50, follow.position.z);

        // control height
        RaycastHit verifyHeight;
        Physics.Raycast(transform.position, Vector3.down, out verifyHeight, 1000f, LayerMask.GetMask("ground"));
        float heightDist = verifyHeight.distance * 10;

        int layerMask = LayerMask.GetMask("wall");

        if (!Physics.Raycast(leftMostPart, targetPosition - leftMostPart, out _, horizontalDist, layerMask) &&
            !Physics.Raycast(rightMostPart, targetPosition - rightMostPart, out _, horizontalDist, layerMask))
        {
            // get direct view direction, if we got beyond the point, we reached the destination
            if (!directView)
            {
                directView = true;
                targetDirectDirection = new Vector3(targetPosition.x, 0, targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z);
            }
            Vector3 currentDirectDirection = new Vector3(targetPosition.x, 0, targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z);
            float angle = Vector3.SignedAngle(currentDirectDirection, targetDirectDirection, Vector3.up);
            if (angle < -90 || angle > 90)
            {
                UnsetDestination(false);
                return;
            }

            // Clear line of sight to target position
            if (follow == null)
            {
                Vector3 Ynull = new Vector3(destination.x, transform.position.y, destination.z);
                Vector3 moveDir = (Ynull - transform.position).normalized;

                rb.MovePosition(transform.position + new Vector3(moveDir.x, -heightDist, moveDir.z) * Time.fixedDeltaTime * speed);
                if (moveDir != Vector3.zero)
                    transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z), Vector3.up);
            }
            else
            {
                Vector3 Ynull = new Vector3(follow.position.x, transform.position.y, follow.position.z);
                Vector3 moveDir = (Ynull - transform.position).normalized;

                rb.MovePosition(transform.position + new Vector3(moveDir.x, -heightDist, moveDir.z) * Time.fixedDeltaTime * speed);
                if (moveDir != Vector3.zero)
                    transform.LookAt(new Vector3(follow.position.x, transform.position.y, follow.position.z), Vector3.up);
            }
        }
        else
        {
            int2 flowVector = NodeFromWorldPoint(transform.position, useOwnGrid).FlowFieldVector;
            Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

            rb.MovePosition(transform.position + (moveDir + Vector3.up * -heightDist) * Time.fixedDeltaTime * speed);
            if (moveDir != Vector3.zero)
                transform.forward = moveDir;
        }
    }


    // function cost to be checked
    private Selectable CheckReachable()
    {
        Transform nearestEnemy = null;

        if (isAlly)
            nearestEnemy = GameManager.instance.enemyUnits.FindClosest(transform.position);
        else
            nearestEnemy = GameManager.instance.allyUnits.FindClosest(transform.position);

        if (nearestEnemy != null)
            return nearestEnemy.GetComponent<Selectable>();
        else
        {
            if (isAlly)
                nearestEnemy = GameManager.instance.enemyBuildings.FindClosest(transform.position);
            else
                nearestEnemy = GameManager.instance.allyBuildings.FindClosest(transform.position);
        }

        return nearestEnemy.GetComponent<Selectable>();
    }


    public override void HoldPosition()
    {
        UnsetDestination(true);
        holdPosition = true;
        rb.isKinematic = true;
    }


    private void Attack(Selectable ag)
    {
        transform.LookAt(new Vector3(ag.transform.position.x, transform.position.y, ag.transform.position.z));
        ag.GetAttacked(attackDamage);
        attackCooldown = attackSpeed;
    }


    public override void GetAttacked(int dmg)
    {
        int diff = dmg - armor;

        if (diff <= 0)
            return;

        health -= diff;

        if (health <= 0)
            Destroy(gameObject);
    }


    private void OnDestroy()
    {
        if (isAlly)
            GameManager.instance.allyUnits.RemoveAll(new System.Predicate<int>(IsSameObj));
        else
            GameManager.instance.enemyUnits.RemoveAll(new System.Predicate<int>(IsSameObj));

        SelectedDico.instance.DeslectDueToDestruction(GetInstanceID());
        ownGrid.Dispose();
        toVisit.Dispose();
        UnsetDestination();
    }


    public override void AddDestination(int gridId, int calculatorId, Vector3 dest, Transform follow = null, Actions action = Actions.MOVE, ResourceManager res = null)
    {
        if (action == Actions.HARVEST && isWorker)
        {
            int result = res.IsFreeSlot(gameObject.GetInstanceID());
            if (result == 0)
                UnsetDestination(true);
            else if (result == 1)
            {
                UnsetDestination();
                res.GetFreeSlot(gameObject.GetInstanceID(), transform);
                ressource = res;
            }
            else
                UnsetDestination();
        }
        else
            UnsetDestination();

        destination = dest;
        hasDestination = true;
        this.gridId = gridId;
        this.calculatorId = calculatorId;

        if (action == Actions.FOLLOW || follow != null)
            this.follow = follow;

        if (action == Actions.ATTACK)
            attackCommand = true;
    }


    private void UnsetDestination(bool unsetResource = false)
    {
        if (hasDestination && !useOwnGrid)
            PathRegister.instance.AgentRetire(gridId);

        directView = false;
        follow = null;
        useOwnGrid = false;
        hasDestination = false;
        attackCommand = false;
        holdPosition = false;
        rb.isKinematic = false;

        if (unsetResource && ressource != null)
        {
            ressource.FreeSlot();
            ressource = null;
        }
    }


    // Gets the closest node to the given world position
    private DijkstraTile NodeFromWorldPoint(Vector3 a_vWorldPos, bool useOurOwnGrid = false)
    {
        float ixPos = (a_vWorldPos.x + PathRegister.instance.vGridWorldSize.x / 2) / PathRegister.instance.vGridWorldSize.x;
        float iyPos = (a_vWorldPos.z + PathRegister.instance.vGridWorldSize.y / 2) / PathRegister.instance.vGridWorldSize.y;

        if (useOurOwnGrid)
        {
            int ix = (int)(ixPos * PathRegister.instance.iGridSizeX);
            int iy = (int)(iyPos * PathRegister.instance.iGridSizeY);
            return ownGrid[PathRegister.instance.iGridSizeY * ix + iy];
        }

        if (!PathRegister.instance.calculators[calculatorId].computingJobs)
        {
            int ix = (int)(ixPos * PathRegister.instance.iGridSizeX);
            int iy = (int)(iyPos * PathRegister.instance.iGridSizeY);
            return PathRegister.instance.grids[gridId][PathRegister.instance.iGridSizeY * ix + iy];
        }
        else
        {
            int ix = (int)(ixPos * PathRegister.instance.impreciseiGridSizeX);
            int iy = (int)(iyPos * PathRegister.instance.impreciseiGridSizeY);
            return PathRegister.instance.impreciseGrids[gridId][PathRegister.instance.impreciseiGridSizeY * ix + iy];
        }
    }
}
