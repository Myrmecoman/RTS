using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


public class PathCalculator : MonoBehaviour
{
    [HideInInspector] public bool computingJobs = false;                   // Tells if we have access to precise pathfinding
    [HideInInspector] private Transform TrStartPosition;                   // Keep a transform in case of changes (follow)
    [HideInInspector] public Vector3 targetPosition;                       // This is where the program will start the pathfinding from

    // Needed to manage jobs without blocking
    private double delay;
    private int gridId;
    private NativeArray<DijkstraTile> tempDijkstra;
    private JobHandle handleUpdate;
    private bool updateScheduled = false;
    private JobHandle handleDijkstra;
    private bool dijkstraScheduled = false;
    private JobHandle handle;
    private bool flowScheduled = false;

    private PathRegister pathRegister;


    private void Start()
    {
        pathRegister = PathRegister.instance;
    }


    private void Update()
    {
        // if we follow a target && we do not compute again the same path (every 0.5 sec)
        if (TrStartPosition != null && TrStartPosition.position != targetPosition && Time.realtimeSinceStartup - delay > 0.5f)
            ChangeTarget(TrStartPosition, gridId);

        if (computingJobs)
        {
            if (!updateScheduled)
            {
                updateScheduled = true;
                var jobUpdateGrid = new UpdateGrid();
                jobUpdateGrid.grid = PathRegister.instance.grids[gridId];
                handleUpdate = jobUpdateGrid.Schedule(PathRegister.instance.grids[gridId].Length, 32 /* batches */);
            }

            if (handleUpdate.IsCompleted)
            {
                if (!dijkstraScheduled)
                {
                    dijkstraScheduled = true;
                    handleUpdate.Complete();
                    var jobDataDij = new DijkstraGrid();
                    computingJobs = false;
                    jobDataDij.target = NodeFromWorldPoint(targetPosition);
                    computingJobs = true;
                    jobDataDij.gridSize = new int2(pathRegister.iGridSizeX, pathRegister.iGridSizeY);
                    jobDataDij.grid = PathRegister.instance.grids[gridId];
                    handleDijkstra = jobDataDij.Schedule(handleUpdate);
                }

                if (handleDijkstra.IsCompleted)
                {
                    if (!flowScheduled)
                    {
                        flowScheduled = true;
                        handleDijkstra.Complete();
                        var jobData = new FlowFieldGrid();
                        jobData.gridSize = new int2(pathRegister.iGridSizeX, pathRegister.iGridSizeY);
                        tempDijkstra = new NativeArray<DijkstraTile>(PathRegister.instance.grids[gridId], Allocator.TempJob);
                        jobData.RdGrid = tempDijkstra;
                        jobData.grid = PathRegister.instance.grids[gridId];
                        handle = jobData.Schedule(PathRegister.instance.grids[gridId].Length, 32 /* batches */, handleDijkstra);
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
    public void ChangeTarget(Transform newStartPosition, int gridId)
    {
        if (newStartPosition.position == targetPosition)
            return;

        this.gridId = gridId;

        TrStartPosition = newStartPosition;
        targetPosition = newStartPosition.position;
        computingJobs = true;
        double imprecisedelay = Time.realtimeSinceStartupAsDouble;

        // imprecise claculation
        var jobUpdateGrid = new UpdateGrid();
        jobUpdateGrid.grid = PathRegister.instance.impreciseGrids[gridId];
        var imprecisehandleUpdate = jobUpdateGrid.Schedule(PathRegister.instance.impreciseGrids[gridId].Length, 32 /* batches */);
        imprecisehandleUpdate.Complete();

        var jobDataDij = new DijkstraGrid();
        jobDataDij.target = NodeFromWorldPoint(targetPosition);
        jobDataDij.gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY);
        jobDataDij.grid = PathRegister.instance.impreciseGrids[gridId];
        jobDataDij.Run();

        var jobData = new FlowFieldGrid();
        jobData.gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY);
        NativeArray<DijkstraTile> imprecisetempDijkstra = new NativeArray<DijkstraTile>(PathRegister.instance.impreciseGrids[gridId], Allocator.TempJob);
        jobData.RdGrid = imprecisetempDijkstra;
        jobData.grid = PathRegister.instance.impreciseGrids[gridId];
        var impreciseFlowHandle = jobData.Schedule(PathRegister.instance.impreciseGrids[gridId].Length, 32 /* batches */);
        impreciseFlowHandle.Complete();
        imprecisetempDijkstra.Dispose();

        // Debug.Log("imprecise total : " + (Time.realtimeSinceStartup - imprecisedelay));

        delay = Time.realtimeSinceStartupAsDouble;
    }


    // Gets the closest node to the given world position
    public DijkstraTile NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = (a_vWorldPos.x + pathRegister.vGridWorldSize.x / 2) / pathRegister.vGridWorldSize.x;
        float iyPos = (a_vWorldPos.z + pathRegister.vGridWorldSize.y / 2) / pathRegister.vGridWorldSize.y;

        if (!computingJobs)
        {
            int ix = (int)(ixPos * pathRegister.iGridSizeX);
            int iy = (int)(iyPos * pathRegister.iGridSizeY);
            return PathRegister.instance.grids[gridId][pathRegister.iGridSizeY * ix + iy];
        }
        else
        {
            int ix = (int)(ixPos * pathRegister.impreciseiGridSizeX);
            int iy = (int)(iyPos * pathRegister.impreciseiGridSizeY);
            return PathRegister.instance.impreciseGrids[gridId][pathRegister.impreciseiGridSizeY * ix + iy];
        }
    }
}
