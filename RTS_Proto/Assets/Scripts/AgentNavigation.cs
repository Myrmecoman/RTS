using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class AgentNavigation : MonoBehaviour
{
    public float force = 1.0f;
    public float heightFromCenter = 0f;
    public SpriteRenderer sprite;
    public Transform leftMostPart;
    public Transform rightMostPart;

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public List<WorldGrid> worldGrid;
    [HideInInspector] public List<int> gridIndexes;
    [HideInInspector] public bool hasDestination = false;

    private Rigidbody rb;
    private DijkstraTile lastValidTile;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        worldGrid = new List<WorldGrid>();
        gridIndexes = new List<int>();
    }


    private void FixedUpdate()
    {
        // NEED TO FIND OUT WHEN DESTINATION IS REACHED

        if (!hasDestination)
            return;

        AdjustHeight();

        DijkstraTile currentTile = worldGrid[0].NodeFromWorldPoint(transform.position);

        // Detecting if we reached our target position
        float horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(worldGrid[0].StartPosition.x, 0, worldGrid[0].StartPosition.z));
        if (horizontalDist <= 0.05f)
        {
            gameManager.inUse[gridIndexes[0]]--;
            gridIndexes.RemoveAt(0);
            worldGrid.RemoveAt(0);
            if (worldGrid.Count == 0)
                hasDestination = false;
            return;
        }

        MoveAndRotate(ref currentTile, ref horizontalDist);

        lastValidTile = worldGrid[0].NodeFromWorldPoint(transform.position);
    }


    private void AdjustHeight()
    {
        RaycastHit verifyHeight;
        // collide against everything except layer 7
        int layerMaskHeight = 1 << 7;
        layerMaskHeight = ~layerMaskHeight;
        if (Physics.Raycast(transform.position, Vector3.down, out verifyHeight, 1000f, layerMaskHeight))
        {
            if (verifyHeight.distance > heightFromCenter + 0.01f)
                transform.position = new Vector3(transform.position.x, transform.position.y - (verifyHeight.distance - (heightFromCenter - 0.01f)), transform.position.z);
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
            worldGrid[0].StartPosition != null &&
            Physics.Raycast(leftMostPart.position, worldGrid[0].StartPosition - leftMostPart.position, out hitLeft, 1000f, layerMask) &&
            Physics.Raycast(rightMostPart.position, worldGrid[0].StartPosition - rightMostPart.position, out hitRight, 1000f, layerMask))
        {
            if (hitLeft.distance / Vector3.Distance(leftMostPart.position, worldGrid[0].StartPosition) >= 0.99f &&
                hitLeft.distance / Vector3.Distance(leftMostPart.position, worldGrid[0].StartPosition) <= 1.01f &&
                hitRight.distance / Vector3.Distance(rightMostPart.position, worldGrid[0].StartPosition) >= 0.99f &&
                hitRight.distance / Vector3.Distance(rightMostPart.position, worldGrid[0].StartPosition) <= 1.01f)
            {
                // Clear line of sight to target position
                Vector3 Ynull = new Vector3(worldGrid[0].StartPosition.x, transform.position.y, worldGrid[0].StartPosition.z);
                Vector3 moveDir = (Ynull - transform.position).normalized;

                rb.MovePosition(transform.position + new Vector3(moveDir.x, 0, moveDir.z) * Time.fixedDeltaTime * force);
                if (moveDir != Vector3.zero)
                    transform.LookAt(new Vector3(worldGrid[0].StartPosition.x, transform.position.y, worldGrid[0].StartPosition.z), Vector3.up);
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


    public void Select()
    {
        sprite.enabled = true;
    }


    public void UnSelect()
    {
        sprite.enabled = false;
    }


    public void AddDestination(WorldGrid grid, int index)
    {
        worldGrid.Add(grid);
        gridIndexes.Add(index);
        hasDestination = true;
    }


    public void UnsetCurrentDestination()
    {
        gameManager.inUse[gridIndexes[0]]--;
        gridIndexes.RemoveAt(0);
        worldGrid.RemoveAt(0);
    }


    public void UnsetAllDestinations()
    {
        foreach (int indexes in gridIndexes)
            gameManager.inUse[indexes]--;

        gridIndexes.Clear();
        worldGrid.Clear();
        hasDestination = false;
    }
}
