﻿using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    [HideInInspector] public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    [HideInInspector] public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    [HideInInspector] public float fNodeRadius; // This stores how big each square on the graph will be
    [HideInInspector] public bool computingJobs = false; // Tells if we have access to precise pathfinding
    [HideInInspector] public Vector3 StartPosition; // This is where the program will start the pathfinding from

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
                        Debug.Log("precise total : " + (Time.realtimeSinceStartup - delay));
                    }
                }
            }
        }
    }


    // Change target position
    public void ChangeTarget(Vector3 newStartPosition)
    {
        StartPosition = newStartPosition;
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

        Debug.Log("imprecise total : " + (Time.realtimeSinceStartup - imprecisedelay));

        delay = Time.realtimeSinceStartupAsDouble;
    }


    // Draw the grid
    void CreateGrid()
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

                if (Physics.CheckSphere(worldPoint, fNodeRadius - 0.0001f /* in case of single point collision */, WallMask))
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

                if (Physics.CheckSphere(worldPoint, imprecisefNodeRadius - 0.0001f /* in case of single point collision */, WallMask))
                    tile.weight = int.MaxValue;

                impreciseNodeArray[impreciseiGridSizeY * x + y] = tile; // Create a new node in the array
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
        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y)); // Draw a wire cube with the given dimensions from the Unity inspector

        if (impreciseNodeArray != null) // If the grid is not empty
        {
            foreach (DijkstraTile n in impreciseNodeArray) // Loop through every node in the grid
            {
                if (n.weight == int.MaxValue) // If the current node is a wall node
                {
                    Gizmos.color = Color.blue;
                    Vector3 worldPoint = bottomLeft + Vector3.right * (n.gridPos.x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (n.gridPos.y * imprecisefNodeDiameter + imprecisefNodeRadius);
                    Gizmos.DrawCube(worldPoint, new Vector3(1, 0, 1) * imprecisefNodeDiameter);
                }
            }
        }

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
}