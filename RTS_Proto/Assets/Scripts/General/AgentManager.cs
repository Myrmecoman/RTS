using Unity.Mathematics;
using UnityEngine;


public class AgentManager : Selectable
{
    public int sightRange = 10;
    public int supply = 1;
    public int attackDamage = 5;
    public int attackRange = 5;
    public float attackSpeed = 1;
    public int health = 50;
    public int armor = 0;
    public float speed = 1.0f;
    public bool canAttackGround = true;
    public bool canAttackAir = true;

    public Transform leftPart;
    public Transform rightPart;

    private bool hasDestination = false;
    private bool holdPosition = false;
    private bool attackCommand = false;
    private double attackCooldown = 0;
    private Rigidbody rb;
    private Transform follow;
    private ResourceManager ressource = null;

    private bool directView = false;
    private Vector3 targetDirectDirection;
     

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {

        // attack cooldown
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        // attack reachable targets
        AgentManager foundTarget = CheckEnnemy();
        if (attackCooldown <= 0 && (!hasDestination || attackCommand || holdPosition) && foundTarget != null && ressource == null)
        {
            // attack target
            Attack(foundTarget);
            return;
        }

        // attacking, check if needing to hold position
        if ((attackCommand || holdPosition) && foundTarget != null && ressource == null)
        {
            rb.isKinematic = true;
            return;
        }
        else if (ressource == null)
            rb.isKinematic = false;

        // exit if no destination
        if (!hasDestination)
            return;

        // getting distance to destination
        float horizontalDist;
        if (follow == null)
            horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(destination.x, 0, destination.z));
        else
            horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(follow.position.x, 0, follow.position.z));

        if (horizontalDist <= 0.05f && follow == null)
        {
            UnsetDestination(true);
            if (ressource != null)
                HoldPosition();
            return;
        }

        if (horizontalDist > 0.05f && destination != null)
            MoveAndRotate(horizontalDist);
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

        RaycastHit hitLeft;
        RaycastHit hitRight;
        int layerMask = LayerMask.GetMask("wall");

        if (!Physics.Raycast(leftMostPart, targetPosition - leftMostPart, out hitLeft, horizontalDist, layerMask) &&
            !Physics.Raycast(rightMostPart, targetPosition - rightMostPart, out hitRight, horizontalDist, layerMask))
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
            int2 flowVector = NodeFromWorldPoint(transform.position).FlowFieldVector;
            Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

            rb.MovePosition(transform.position + (moveDir + Vector3.up * -heightDist) * Time.fixedDeltaTime * speed);
            if (moveDir != Vector3.zero)
                transform.forward = moveDir;
        }
    }


    // very intensive function (could be run every x seconds)
    private AgentManager CheckEnnemy()
    {
        if (GameManager.instance.enemyUnits.Count == 0 || GameManager.instance.allyUnits.Count == 0)
            return null;

        Transform nearestEnemy;

        if (isAlly)
            nearestEnemy = GameManager.instance.enemyUnits.FindClosest(transform.position);
        else
            nearestEnemy = GameManager.instance.allyUnits.FindClosest(transform.position);

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(nearestEnemy.position.x, 0, nearestEnemy.position.z)) <= attackRange)
            return nearestEnemy.GetComponent<AgentManager>();

        return null;
    }


    private void Attack(AgentManager ag)
    {
        transform.LookAt(new Vector3(ag.transform.position.x, transform.position.y, ag.transform.position.z));
        ag.GetAttacked(attackDamage);
        attackCooldown = attackSpeed;
    }


    public override void HoldPosition()
    {
        UnsetDestination(true);
        holdPosition = true;
        rb.isKinematic = true;
    }


    private void GetAttacked(int dmg)
    {
        int diff = dmg - armor;

        if (diff <= 0)
            return;

        health -= diff;

        if (health <= 0)
        {
            if (isAlly)
                GameManager.instance.allyUnits.RemoveAll(new System.Predicate<int>(IsSameObj));
            else
                GameManager.instance.enemyUnits.RemoveAll(new System.Predicate<int>(IsSameObj));

            SelectedDico.instance.DeslectDueToDestruction(GetInstanceID());
            UnsetDestination();
            Destroy(gameObject);
        }
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


    public void UnsetDestination(bool unsetResource = false)
    {
        if (hasDestination)
            PathRegister.instance.AgentRetire(gridId);

        directView = false;
        follow = null;
        customUsed = false;
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
    public DijkstraTile NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = (a_vWorldPos.x + PathRegister.instance.vGridWorldSize.x / 2) / PathRegister.instance.vGridWorldSize.x;
        float iyPos = (a_vWorldPos.z + PathRegister.instance.vGridWorldSize.y / 2) / PathRegister.instance.vGridWorldSize.y;

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
