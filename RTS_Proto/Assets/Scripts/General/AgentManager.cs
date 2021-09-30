using Unity.Mathematics;
using UnityEngine;


public class AgentManager : MonoBehaviour
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
    public bool isWorker = false;

    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public Transform leftMostPart;
    public Transform rightMostPart;

    [HideInInspector] public bool isAlly = true;
    [HideInInspector] public WorldGrid worldGrid;
    [HideInInspector] public int gridIndexe;
    [HideInInspector] public bool hasDestination = false;

    private bool holdPosition = false;
    private bool attackCommand = false;
    private bool patrolCommand = false;
    private double attackCooldown;
    private Rigidbody rb;
    private DijkstraTile lastValidTile;
    private Transform follow;
    private ResourceManager res = null;
     

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // NEED TO FIND OUT WHEN DESTINATION IS REACHED

        AdjustHeight();

        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        AgentManager foundTarget = CheckEnnemy();

        if (attackCooldown <= 0 && (!hasDestination || attackCommand || holdPosition) && foundTarget != null)
        {
            // attack target
            Attack(foundTarget);
            return;
        }

        if ((attackCommand || holdPosition) && foundTarget != null)
        {
            rb.isKinematic = true;
            return;
        }
        else
            rb.isKinematic = false;

        if (!hasDestination)
            return;

        DijkstraTile currentTile = worldGrid.NodeFromWorldPoint(transform.position);

        // Detecting if we reached our target position
        float horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(worldGrid.StartPosition.x, 0, worldGrid.StartPosition.z));
        if (horizontalDist <= 0.05f && follow == null)
        {
            UnsetDestinationExceptResource();
            return;
        }
        
        MoveAndRotate(ref currentTile, ref horizontalDist);

        lastValidTile = worldGrid.NodeFromWorldPoint(transform.position);
    }


    private void AdjustHeight()
    {
        RaycastHit verifyHeight;
        // collide against ground only
        int layerMaskHeight = LayerMask.GetMask("ground");
        if (Physics.Raycast(transform.position, Vector3.down, out verifyHeight, 1000f, layerMaskHeight))
        {
            if (verifyHeight.distance > 0.01f)
                transform.position = new Vector3(transform.position.x, transform.position.y - (verifyHeight.distance + 0.01f), transform.position.z);
        }
    }


    private void MoveAndRotate(ref DijkstraTile currentTile, ref float horizontalDist)
    {
        RaycastHit hitLeft;
        RaycastHit hitRight;

        int layerMask = LayerMask.GetMask("wall", "ground", "building");

        if (horizontalDist > 0.05f &&
            worldGrid.StartPosition != null &&
            Physics.Raycast(leftMostPart.position, worldGrid.StartPosition - leftMostPart.position, out hitLeft, 1000f, layerMask) &&
            Physics.Raycast(rightMostPart.position, worldGrid.StartPosition - rightMostPart.position, out hitRight, 1000f, layerMask))
        {
            if (hitLeft.distance / Vector3.Distance(leftMostPart.position, worldGrid.StartPosition) >= 0.99f &&
                hitLeft.distance / Vector3.Distance(leftMostPart.position, worldGrid.StartPosition) <= 1.01f &&
                hitRight.distance / Vector3.Distance(rightMostPart.position, worldGrid.StartPosition) >= 0.99f &&
                hitRight.distance / Vector3.Distance(rightMostPart.position, worldGrid.StartPosition) <= 1.01f)
            {
                // Clear line of sight to target position
                if (follow == null)
                {
                    Vector3 Ynull = new Vector3(worldGrid.StartPosition.x, transform.position.y, worldGrid.StartPosition.z);
                    Vector3 moveDir = (Ynull - transform.position).normalized;

                    rb.MovePosition(transform.position + new Vector3(moveDir.x, 0, moveDir.z) * Time.fixedDeltaTime * speed);
                    if (moveDir != Vector3.zero)
                        transform.LookAt(new Vector3(worldGrid.StartPosition.x, transform.position.y, worldGrid.StartPosition.z), Vector3.up);
                }
                else
                {
                    Vector3 Ynull = new Vector3(follow.position.x, transform.position.y, follow.position.z);
                    Vector3 moveDir = (Ynull - transform.position).normalized;

                    rb.MovePosition(transform.position + new Vector3(moveDir.x, 0, moveDir.z) * Time.fixedDeltaTime * speed);
                    if (moveDir != Vector3.zero)
                        transform.LookAt(new Vector3(follow.position.x, transform.position.y, follow.position.z), Vector3.up);
                }
            }
            else
            {
                // Obstructed line of sight to target position
                if (currentTile.FlowFieldVector.Equals(int2.zero))
                {
                    int2 flowVector = lastValidTile.gridPos - currentTile.gridPos;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.fixedDeltaTime * speed);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
                else
                {
                    lastValidTile = currentTile;
                    int2 flowVector = currentTile.FlowFieldVector;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.fixedDeltaTime * speed);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
            }
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


    public void HoldPosition()
    {
        UnsetDestination();
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


    private bool IsSameObj(int objID)
    {
        return gameObject.GetInstanceID() == objID;
    }


    public void Select()
    {
        sprite.enabled = true;
    }


    public void UnSelect()
    {
        sprite.enabled = false;
    }


    public void MoveTowardsSprite()
    {
        moveTowardsSprite.SetActive(true);
    }


    public void AddDestination(WorldGrid grid, int index, Transform follow = null, int action = 0 /* 1 = attack, 2 = patrol, 3 = collect-resource */, ResourceManager res = null)
    {
        if (action == 3 && isWorker)
        {
            int result = res.IsFreeSlot(gameObject.GetInstanceID());
            if (result == 0)
                UnsetDestinationExceptResource();
            else if (result == 1)
            {
                UnsetDestination();
                res.GetFreeSlot(gameObject.GetInstanceID(), transform);
                this.res = res;
            }
            else
                UnsetDestination();
        }
        else
        {
            UnsetDestination();
            Debug.Log("FullUnset");
        }

        worldGrid = grid;
        gridIndexe = index;
        hasDestination = true;
        this.follow = follow;

        if (action == 1 || action == 2)
            attackCommand = true;
        else
            attackCommand = false;

        if (action == 2)
            patrolCommand = true;
        else
            patrolCommand = false;
    }


    public void UnsetDestination()
    {
        if (hasDestination)
        {
            GameManager.instance.inUse[gridIndexe]--;
            follow = null;
            hasDestination = false;
            attackCommand = false;
            patrolCommand = false;
            holdPosition = false;
            rb.isKinematic = false;
            if (res != null)
            {
                res.FreeSlot();
                res = null;
            }
        }
    }

    
    public void UnsetDestinationExceptResource()
    {
        if (hasDestination)
        {
            GameManager.instance.inUse[gridIndexe]--;
            follow = null;
            hasDestination = false;
            attackCommand = false;
            patrolCommand = false;
            holdPosition = false;
            rb.isKinematic = false;
        }
    }
}
