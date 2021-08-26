using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius; // This stores how big each square on the graph will be

    [HideInInspector] public Vector3 StartPosition; // This is where the program will start the pathfinding from

    private NativeArray<DijkstraTile> NodeArray; // The array of nodes that the A Star algorithm uses
    private float fNodeDiameter; // Twice the amount of the radius (Set in the start function)
    private int iGridSizeX, iGridSizeY; // Size of the Grid in Array units
    private Vector3 bottomLeft; // Get the real world position of the bottom left of the grid


    private void Awake()
    {
        fNodeDiameter = fNodeRadius * 2;
        bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;

        // Divide the grids world co-ordinates by the diameter to get the size of the graph in array units
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);
        CreateGrid();
    }


    // Change target possition
    public void ChangeTarget(Vector3 newStartPosition)
    {
        StartPosition = newStartPosition;

        double t = Time.realtimeSinceStartupAsDouble;
        var jobUpdateGrid = new UpdateGrid();
        jobUpdateGrid.grid = NodeArray;
        var handleUpdate = jobUpdateGrid.Schedule(NodeArray.Length, 32 /* batches */);
        handleUpdate.Complete();
        double updateGrid = Time.realtimeSinceStartupAsDouble - t;

        t = Time.realtimeSinceStartupAsDouble;
        var jobDataDij = new DijkstraGrid();
        jobDataDij.target = NodeFromWorldPoint(StartPosition);
        jobDataDij.gridSize = new int2(iGridSizeX, iGridSizeY);
        jobDataDij.grid = NodeArray;
        jobDataDij.Run();
        double DijkstraTime = Time.realtimeSinceStartupAsDouble - t;

        t = Time.realtimeSinceStartupAsDouble;
        var jobData = new FlowFieldGrid();
        jobData.gridSize = new int2(iGridSizeX, iGridSizeY);
        NativeArray<DijkstraTile> tempDijkstra = new NativeArray<DijkstraTile>(NodeArray, Allocator.TempJob);
        jobData.RdGrid = tempDijkstra;
        jobData.grid = NodeArray;
        var handle = jobData.Schedule(NodeArray.Length, 32 /* batches */);
        handle.Complete();
        tempDijkstra.Dispose();
        double FlowFieldTime = Time.realtimeSinceStartupAsDouble - t;

        //Debug.Log("Times : \n" + "dijkstra : " + DijkstraTime + " | flowfield : " + FlowFieldTime + " | update time : " + updateGrid);
    }


    // Draw the grid
    void CreateGrid()
    {
        NodeArray = new NativeArray<DijkstraTile>(iGridSizeX * iGridSizeY, Allocator.Persistent); // Declare the array of nodes

        //Loop through the array of nodes
        for (int x = 0; x < iGridSizeX; x++)
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);
                DijkstraTile tile = new DijkstraTile(new int2(x, y));

                if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask))
                    tile.weight = int.MaxValue;

                NodeArray[iGridSizeY * x + y] = tile; // Create a new node in the array
            }
        }
    }


    // Gets the closest node to the given world position
    public DijkstraTile NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        return NodeArray[iGridSizeY * ix + iy];
    }


    #if (UNITY_EDITOR)
    // Function that draws the wireframe
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y)); // Draw a wire cube with the given dimensions from the Unity inspector

        if (NodeArray != null) // If the grid is not empty
        {
            foreach (DijkstraTile n in NodeArray) // Loop through every node in the grid
            {
                if (n.weight == int.MaxValue) // If the current node is a wall node
                {
                    Gizmos.color = Color.magenta;
                    Vector3 worldPoint = bottomLeft + Vector3.right * (n.gridPos.x * fNodeDiameter + fNodeRadius) + Vector3.forward * (n.gridPos.y * fNodeDiameter + fNodeRadius);
                    Gizmos.DrawCube(worldPoint, new Vector3(1, 0, 1) * fNodeDiameter);
                }
            }
        }
    }
    #endif


    private void OnApplicationQuit()
    {
        NodeArray.Dispose();
    }
}