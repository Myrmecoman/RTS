using Unity.Mathematics;
using UnityEngine;


public class AgentManager : MonoBehaviour
{
    public int sightRange = 10;
    public int supply = 1;
    public int attackDamage = 5;
    public int attackRange = 5;
    public float attackSpeed = 2;
    public int health = 50;
    public int armor = 0;
    public float speed = 1.0f;
    public bool canAttackGround = true;
    public bool canAttackAir = true;

    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public Transform leftMostPart;
    public Transform rightMostPart;

    [HideInInspector] public WorldGrid worldGrid;
    [HideInInspector] public int gridIndexe;
    [HideInInspector] public bool hasDestination = false;

    private bool attackCommand = false;
    private Rigidbody rb;
    private DijkstraTile lastValidTile;
    private Transform follow;
     

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // NEED TO FIND OUT WHEN DESTINATION IS REACHED

        AdjustHeight();

        AgentManager foundTarget = null;//CheckEnnemy();
        if ((!hasDestination || attackCommand) && foundTarget != null)
        {
            Debug.Log("no");
            // attack target
            rb.isKinematic = true;
            return;
        }

        if (rb.isKinematic)
            rb.isKinematic = false;

        if (!hasDestination)
            return;

        DijkstraTile currentTile = worldGrid.NodeFromWorldPoint(transform.position);

        // Detecting if we reached our target position
        float horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(worldGrid.StartPosition.x, 0, worldGrid.StartPosition.z));
        if (horizontalDist <= 0.05f && follow == null)
        {
            UnsetDestination();
            return;
        }
        
        MoveAndRotate(ref currentTile, ref horizontalDist);

        lastValidTile = worldGrid.NodeFromWorldPoint(transform.position);
    }


    private void AdjustHeight()
    {
        RaycastHit verifyHeight;
        // collide against everything except layer 7
        int layerMaskHeight = 1 << 7;
        layerMaskHeight = ~layerMaskHeight;
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
        // collide against everything except layer 7
        int layerMask = 1 << 7;
        layerMask = ~layerMask;

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


    // very intensive function, to be optimized later (layer exclusion, running every x seconds etc...)
    private AgentManager CheckEnnemy()
    {
        int bestTarget = -1;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 pos = transform.position;

        for (int i = 0; i < GameManager.instance.enemyUnits.Count; i++)
        {
            float dSqrToTarget = (GameManager.instance.enemyUnits[i].transform.position - pos).sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = i;
            }
        }

        if (bestTarget == -1)
            return null;

        return GameManager.instance.enemyUnits[bestTarget].GetComponent<AgentManager>();
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
        //Debug.Log("moving towards");
        moveTowardsSprite.SetActive(true);
    }


    public void AddDestination(WorldGrid grid, int index, Transform follow = null)
    {
        worldGrid = grid;
        gridIndexe = index;
        hasDestination = true;
        this.follow = follow;
    }


    public void UnsetDestination()
    {
        GameManager.instance.inUse[gridIndexe]--;
        hasDestination = false;
        follow = null;
    }
}
