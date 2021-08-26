using Unity.Mathematics;
using UnityEngine;


public class AgentNavigation : MonoBehaviour
{
    public WorldGrid worldGrid;
    public float force = 1.0f;

    [HideInInspector] public bool destinationReached = true;

    private Rigidbody rb;
    private DijkstraTile lastValidTile;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        this.lastValidTile = worldGrid.NodeFromWorldPoint(transform.position);
    }

    private void Update()
    {
        DijkstraTile currentTile = worldGrid.NodeFromWorldPoint(transform.position);

        RaycastHit hit;
        // Bit shift the index of the layer to get a bit mask, 7 corresponds to agents
        int layerMask = 1 << 7;
        // This would cast rays only against colliders in layer 7, we want to collide against everything except layer 7
        layerMask = ~layerMask;

        if (worldGrid.StartPosition != null && Physics.Raycast(transform.position, worldGrid.StartPosition - transform.position, out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, worldGrid.StartPosition.position - transform.position, Color.red, 1f);

            if (hit.collider.tag == "target")
            {
                // Clear line of sight to target position
                Vector3 moveDir = (hit.transform.position - transform.position).normalized;

                rb.MovePosition(transform.position + moveDir * Time.deltaTime * force);
                if (moveDir != Vector3.zero)
                    transform.forward = new Vector3(moveDir.x, 0, moveDir.z);
            }
            else
            {
                // Obstructed line of sight to target position
                if (currentTile.FlowFieldVector.Equals(int2.zero))
                {
                    int2 flowVector = this.lastValidTile.gridPos - currentTile.gridPos;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.deltaTime * force);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
                else
                {
                    this.lastValidTile = currentTile;
                    int2 flowVector = currentTile.FlowFieldVector;
                    Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

                    rb.MovePosition(transform.position + moveDir * Time.deltaTime * force);
                    if (moveDir != Vector3.zero)
                        transform.forward = moveDir;
                }
            }
        }
        //Debug.Log("Failed cast");
    }
}
