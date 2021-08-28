using Unity.Mathematics;
using UnityEngine;


public class AgentNavigation : MonoBehaviour
{
    public float force = 1.0f;
    public SpriteRenderer sprite;
    public Transform leftMostPart;
    public Transform rightMostPart;

    [HideInInspector] public WorldGrid worldGrid;
    [HideInInspector] public bool hasDestination = false;
    [HideInInspector] public bool destinationReached = true;

    private Rigidbody rb;
    private DijkstraTile lastValidTile;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // NEED TO FIND OUT WHEN DESTINATION IS REACHED

        if (!hasDestination)
            return;

        DijkstraTile currentTile = worldGrid.NodeFromWorldPoint(transform.position);

        RaycastHit hitLeft;
        RaycastHit hitRight;
        // Bit shift the index of the layer to get a bit mask, 7 corresponds to agents
        int layerMask = 1 << 7;
        // This would cast rays only against colliders in layer 7, we want to collide against everything except layer 7
        layerMask = ~layerMask;

        // Detecting if we reached our target position
        float horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(worldGrid.StartPosition.x, 0, worldGrid.StartPosition.z));

        if (horizontalDist > 0.05f &&
            worldGrid.StartPosition != null &&
            Physics.Raycast(leftMostPart.position, worldGrid.StartPosition - leftMostPart.position, out hitLeft, 1000f, layerMask) &&
            Physics.Raycast(rightMostPart.position, worldGrid.StartPosition - rightMostPart.position, out hitRight, 1000f, layerMask))
        {
            // Debug.DrawRay(leftMostPart.position, worldGrid.StartPosition - leftMostPart.position, Color.red, 0.1f);
            // Debug.DrawRay(rightMostPart.position, worldGrid.StartPosition - rightMostPart.position, Color.red, 0.1f);

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
                    int2 flowVector = this.lastValidTile.gridPos - currentTile.gridPos;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.fixedDeltaTime * force);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
                else
                {
                    this.lastValidTile = currentTile;
                    int2 flowVector = currentTile.FlowFieldVector;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.fixedDeltaTime * force);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
            }
        }

        lastValidTile = worldGrid.NodeFromWorldPoint(transform.position);
    }


    public void Select()
    {
        sprite.enabled = true;
    }


    public void SetDestination()
    {
        hasDestination = true;
    }


    public void UnSelect()
    {
        sprite.enabled = false;
    }
}
