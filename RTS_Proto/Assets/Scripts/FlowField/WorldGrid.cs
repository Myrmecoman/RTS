using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    [HideInInspector] public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    [HideInInspector] public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    [HideInInspector] public float fNodeRadius; // This stores how big each square on the graph will be
    [HideInInspector] public bool computingJobs = false; // Tells if we have access to precise pathfinding
    [HideInInspector] private Transform TrStartPosition; // Keep a transform in case of changes (follow)
    [HideInInspector] public Vector3 StartPosition; // This is where the program will start the pathfinding from
    #if (UNITY_EDITOR)
    [HideInInspector] public int debugNb;
    #endif

    private NativeArray<DijkstraTile> NodeArray; // The array of nodes that the A Star algorithm uses
    private float fNodeDiameter; // Twice the amount of the radius (Set in the start function)
    private int iGridSizeX, iGridSizeY; // Size of the Grid in Array units
    private Vector3 bottomLeft; // Get the real world position of the bottom left of the grid

    // Variable for fast pathfinding
    private NativeArray<DijkstraTile> impreciseNodeArray;
    private float imprecisefNodeRadius;
    private float imprecisefNodeDiameter;
    private int impreciseiGridSizeX, impreciseiGridSizeY;

    // Needed to manage jobs without blocking
    private double delay;
    private NativeArray<DijkstraTile> tempDijkstra;
    private JobHandle handleUpdate;
    private bool updateScheduled = false;
    private JobHandle handleDijkstra;
    private bool dijkstraScheduled = false;
    private JobHandle handle;
    private bool flowScheduled = false;


    private void Start()
    {
        bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;

        // precise
        fNodeDiameter = fNodeRadius * 2;
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);

        // imprecise
        imprecisefNodeRadius = fNodeDiameter;
        imprecisefNodeDiameter = imprecisefNodeRadius * 2;
        impreciseiGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / imprecisefNodeDiameter);
        impreciseiGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / imprecisefNodeDiameter);

        CreateGrid();
    }


    private void Update()
    {
        if (TrStartPosition != null && TrStartPosition.position != StartPosition && Time.realtimeSinceStartup - delay > 0.5f)
        {
            ChangeTarget(TrStartPosition);
        }

        if (computingJobs)
        {
            if (!updateScheduled)
            {
                updateScheduled = true;
                var jobUpdateGrid = new UpdateGrid();
                jobUpdateGrid.grid = NodeArray;
                handleUpdate = jobUpdateGrid.Schedule(NodeArray.Length, 32 /* batches */);
            }

            if (handleUpdate.IsCompleted)
            {
                if (!dijkstraScheduled)
                {
                    dijkstraScheduled = true;
                    handleUpdate.Complete();
                    var jobDataDij = new DijkstraGrid();
                    computingJobs = false;
                    jobDataDij.target = NodeFromWorldPoint(StartPosition);
                    computingJobs = true;
                    jobDataDij.gridSize = new int2(iGridSizeX, iGridSizeY);
                    jobDataDij.grid = NodeArray;
                    handleDijkstra = jobDataDij.Schedule(handleUpdate);
                }

                if (handleDijkstra.IsCompleted)
                {
                    if (!flowScheduled)
                    {
                        flowScheduled = true;
                        handleDijkstra.Complete();
                        var jobData = new FlowFieldGrid();
                        jobData.gridSize = new int2(iGridSizeX, iGridSizeY);
                        tempDijkstra = new NativeArray<DijkstraTile>(NodeArray, Allocator.TempJob);
                        jobData.RdGrid = tempDijkstra;
                        jobData.grid = NodeArray;
                        handle = jobData.Schedule(NodeArray.Length, 32 /* batches */, handleDijkstra);
                    }

                    if (handle.IsCompleted)
                    {
                        handle.Complete();
                        tempDijkstra.Dispose();
                        computingJobs = false;
                        updateScheduled = false;
                        dijkstraScheduled = false;
                        flowScheduled = false;
                        // Debug.Log("precise total : " + (Time.realtimeSinceStartup - delay));
                    }
                }
            }
        }
    }


    // Change target position
    public void ChangeTarget(Transform newStartPosition)
    {
        if (newStartPosition.position == StartPosition)
            return;

        TrStartPosition = newStartPosition;
        StartPosition = newStartPosition.position;
        computingJobs = true;
        double imprecisedelay = Time.realtimeSinceStartupAsDouble;

        // imprecise claculation
        var jobUpdateGrid = new UpdateGrid();
        jobUpdateGrid.grid = impreciseNodeArray;
        var imprecisehandleUpdate = jobUpdateGrid.Schedule(impreciseNodeArray.Length, 32 /* batches */);
        imprecisehandleUpdate.Complete();

        var jobDataDij = new DijkstraGrid();
        jobDataDij.target = NodeFromWorldPoint(StartPosition);
        jobDataDij.gridSize = new int2(impreciseiGridSizeX, impreciseiGridSizeY);
        jobDataDij.grid = impreciseNodeArray;
        jobDataDij.Run();

        var jobData = new FlowFieldGrid();
        jobData.gridSize = new int2(impreciseiGridSizeX, impreciseiGridSizeY);
        NativeArray<DijkstraTile>  imprecisetempDijkstra = new NativeArray<DijkstraTile>(impreciseNodeArray, Allocator.TempJob);
        jobData.RdGrid = imprecisetempDijkstra;
        jobData.grid = impreciseNodeArray;
        var impreciseFlowHandle = jobData.Schedule(impreciseNodeArray.Length, 32 /* batches */);
        impreciseFlowHandle.Complete();
        imprecisetempDijkstra.Dispose();

        // Debug.Log("imprecise total : " + (Time.realtimeSinceStartup - imprecisedelay));

        delay = Time.realtimeSinceStartupAsDouble;
    }


    private void CreateGrid()
    {
        NodeArray = new NativeArray<DijkstraTile>(iGridSizeX * iGridSizeY, Allocator.Persistent);
        impreciseNodeArray = new NativeArray<DijkstraTile>(impreciseiGridSizeX * impreciseiGridSizeY, Allocator.Persistent);

        //Loop through the arrays
        for (int x = 0; x < iGridSizeX; x++)
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);
                DijkstraTile tile = new DijkstraTile(new int2(x, y));

                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, fNodeRadius - 0.001f /* in case of single point collision */, WallMask))
                    tile.weight = int.MaxValue;

                NodeArray[iGridSizeY * x + y] = tile; // Create a new node in the array
            }
        }

        for (int x = 0; x < impreciseiGridSizeX; x++)
        {
            for (int y = 0; y < impreciseiGridSizeY; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (y * imprecisefNodeDiameter + imprecisefNodeRadius);
                DijkstraTile tile = new DijkstraTile(new int2(x, y));

                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, imprecisefNodeRadius - 0.001f /* in case of single point collision */, WallMask))
                    tile.weight = int.MaxValue;

                impreciseNodeArray[impreciseiGridSizeY * x + y] = tile; // Create a new node in the array
            }
        }
    }


    public void AddGridColliders(Vector3 centerPos)
    {
        int largestBuildingRadius = (int) (2 / fNodeRadius);
        int impreciseLargestBuildingRadius = (int)(2 / imprecisefNodeRadius);

        float ixPos = ((centerPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((centerPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        ix = Mathf.RoundToInt((impreciseiGridSizeX - 1) * ixPos);
        iy = Mathf.RoundToInt((impreciseiGridSizeY - 1) * iyPos);

        for (int x = ix - impreciseLargestBuildingRadius; x < ix + impreciseLargestBuildingRadius; x++)
        {
            for (int y = iy - impreciseLargestBuildingRadius; y < iy + impreciseLargestBuildingRadius; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (y * imprecisefNodeDiameter + imprecisefNodeRadius);

                //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up * 2, Quaternion.identity);
                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, imprecisefNodeRadius - 0.001f /* in case of single point collision */, WallMask))
                {
                    DijkstraTile tile = new DijkstraTile(new int2(x, y));
                    tile.weight = int.MaxValue;
                    impreciseNodeArray[impreciseiGridSizeY * x + y] = tile;
                }
            }
        }

        if (!computingJobs)
        {
            for (int x = ix - largestBuildingRadius; x < ix + largestBuildingRadius; x++)
            {
                for (int y = iy - largestBuildingRadius; y < iy + largestBuildingRadius; y++)
                {
                    //Get the world coordinates from the bottom left of the graph
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);

                    //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up, Quaternion.identity);
                    if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, fNodeRadius - 0.001f /* in case of single point collision */, WallMask))
                    {
                        DijkstraTile tile = new DijkstraTile(new int2(x, y));
                        tile.weight = int.MaxValue;
                        NodeArray[iGridSizeY * x + y] = tile;
                    }
                }
            }
        }
    }


    public void RemoveGridColliders(Vector3 centerPos)
    {
        int largestBuildingRadius = (int)(2 / fNodeRadius);
        int impreciseLargestBuildingRadius = (int)(2 / imprecisefNodeRadius);

        float ixPos = ((centerPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((centerPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        ix = Mathf.RoundToInt((impreciseiGridSizeX - 1) * ixPos);
        iy = Mathf.RoundToInt((impreciseiGridSizeY - 1) * iyPos);

        for (int x = ix - impreciseLargestBuildingRadius; x < ix + impreciseLargestBuildingRadius; x++)
        {
            for (int y = iy - impreciseLargestBuildingRadius; y < iy + impreciseLargestBuildingRadius; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (y * imprecisefNodeDiameter + imprecisefNodeRadius);

                //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up * 2, Quaternion.identity);
                if (!Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, imprecisefNodeRadius - 0.001f /* in case of single point collision */, WallMask))
                {
                    DijkstraTile tile = new DijkstraTile(new int2(x, y));
                    impreciseNodeArray[impreciseiGridSizeY * x + y] = tile;
                }
            }
        }

        if (!computingJobs)
        {
            for (int x = ix - largestBuildingRadius; x < ix + largestBuildingRadius; x++)
            {
                for (int y = iy - largestBuildingRadius; y < iy + largestBuildingRadius; y++)
                {
                    //Get the world coordinates from the bottom left of the graph
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);

                    //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up, Quaternion.identity);
                    if (!Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, fNodeRadius - 0.001f /* in case of single point collision */, WallMask))
                    {
                        DijkstraTile tile = new DijkstraTile(new int2(x, y));
                        NodeArray[iGridSizeY * x + y] = tile;
                    }
                }
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

        if (!computingJobs)
        {
            int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
            int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);
            return NodeArray[iGridSizeY * ix + iy];
        }
        else
        {
            int ix = Mathf.RoundToInt((impreciseiGridSizeX - 1) * ixPos);
            int iy = Mathf.RoundToInt((impreciseiGridSizeY - 1) * iyPos);
            return impreciseNodeArray[impreciseiGridSizeY * ix + iy];
        }
    }


    private void OnApplicationQuit()
    {
        NodeArray.Dispose();
        impreciseNodeArray.Dispose();
    }

    
    #if (UNITY_EDITOR)
    // Function that draws the wireframe
    private void OnDrawGizmos()
    {
        if (debugNb != 1)
            return;

        if (impreciseNodeArray != null) // If the grid is not empty
        {
            foreach (DijkstraTile n in impreciseNodeArray) // Loop through every node in the grid
            {
                if (n.weight == int.MaxValue) // If the current node is a wall node
                {
                    Gizmos.color = Color.blue;
                    Vector3 worldPoint = bottomLeft + Vector3.right * (n.gridPos.x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (n.gridPos.y * imprecisefNodeDiameter + imprecisefNodeRadius);
                    Gizmos.DrawCube(worldPoint, new Vector3(1, 0.001f, 1) * imprecisefNodeDiameter);
                }
            }
        }

        if (NodeArray != null) // If the grid is not empty
        {
            foreach (DijkstraTile n in NodeArray) // Loop through every node in the grid
            {
                if (n.weight == int.MaxValue) // If the current node is a wall node
                {
                    Gizmos.color = Color.red;
                    Vector3 worldPoint = bottomLeft + Vector3.right * (n.gridPos.x * fNodeDiameter + fNodeRadius) + Vector3.forward * (n.gridPos.y * fNodeDiameter + fNodeRadius);
                    Gizmos.DrawCube(worldPoint, new Vector3(1, 0.001f, 1) * fNodeDiameter);
                }
            }
        }
    }
    #endif
}