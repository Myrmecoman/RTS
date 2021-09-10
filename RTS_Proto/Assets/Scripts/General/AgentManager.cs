using Unity.Mathematics;
using UnityEngine;


public class AgentManager : MonoBehaviour
{
    public float force = 1.0f;
    public float AttackRange = 5f;
    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public Transform leftMostPart;
    public Transform rightMostPart;

    [HideInInspector] public int playerPossessorId = 0;
    [HideInInspector] public WorldGrid worldGrid;
    [HideInInspector] public int gridIndexe;
    [HideInInspector] public bool hasDestination = false;

    private bool attackCommand = false;
    private Rigidbody rb;
    private DijkstraTile lastValidTile;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // NEED TO FIND OUT WHEN DESTINATION IS REACHED

        AgentManager foundTarget = CheckEnnemy();
        if (!hasDestination || attackCommand)
        {
            // attack target
        }

        if (!hasDestination)
        {
            return;
        }

        AdjustHeight();

        DijkstraTile currentTile = worldGrid.NodeFromWorldPoint(transform.position);

        // Detecting if we reached our target position
        float horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(worldGrid.StartPosition.x, 0, worldGrid.StartPosition.z));
        if (horizontalDist <= 0.05f)
        {
            GameManager.instance.inUse[gridIndexe]--;
            hasDestination = false;
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
                Vector3 Ynull = new Vector3(worldGrid.StartPosition.x, transform.position.y, worldGrid.StartPosition.z);
                Vector3 moveDir = (Ynull - transform.position).normalized;

                rb.MovePosition(transform.position + new Vector3(moveDir.x, 0, moveDir.z) * Time.fixedDeltaTime * force);
                if (moveDir != Vector3.zero)
                    transform.LookAt(new Vector3(worldGrid.StartPosition.x, transform.position.y, worldGrid.StartPosition.z), Vector3.up);
            }
            else
            {
                // Obstructed line of sight to target position
                if (currentTile.FlowFieldVector.Equals(int2.zero))
                {
                    int2 flowVector = lastValidTile.gridPos - currentTile.gridPos;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.fixedDeltaTime * force);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
                else
                {
                    lastValidTile = currentTile;
                    int2 flowVector = currentTile.FlowFieldVector;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.fixedDeltaTime * force);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
            }
        }
    }


    // very intensive function, to be optimized later (layer exclusion, running every x seconds etc...)
    private AgentManager CheckEnnemy()
    {
        Collider[] casts = Physics.OverlapCapsule(transform.position - Vector3.up * 100, transform.position + Vector3.up * 100, AttackRange, LayerMask.GetMask("agent"));
        if (casts.Length != 0)
        {
            int bestTarget = 0;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 pos = transform.position;
            for(int i = 0; i < casts.Length; i++)
            {
                if (playerPossessorId == casts[i].GetComponent<AgentManager>().playerPossessorId)
                    continue;

                float dSqrToTarget = (casts[i].transform.position - pos).sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = i;
                }
            }

            return casts[bestTarget].GetComponent<AgentManager>();
        }

        return null;
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


    public void AddDestination(WorldGrid grid, int index)
    {
        worldGrid = grid;
        gridIndexe = index;
        hasDestination = true;
    }


    public void UnsetDestination()
    {
        GameManager.instance.inUse[gridIndexe]--;
        hasDestination = false;
    }
}
