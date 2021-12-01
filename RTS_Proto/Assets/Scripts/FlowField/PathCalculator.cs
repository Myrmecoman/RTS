using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


public class PathCalculator : MonoBehaviour
{
    [HideInInspector] public bool computingJobs;     // Tells if we have access to precise pathfinding
    [HideInInspector] public Vector3 targetPosition; // This is where the program will start the pathfinding from
    [HideInInspector] public bool following = false;
    [HideInInspector] public JobHandle handle;

    private Transform TrStartPosition;               // Keep a transform in case of changes (follow)
    private double delay;
    private double clearDelay;
    private double pathingDelay;
    private double flowFieldDelay;
    private int gridId;
    private NativeQueue<DijkstraTile> toVisit;

    private PathRegister pathRegister;


    private void Start()
    {
        computingJobs = false;
        pathRegister = PathRegister.instance;
        toVisit = new NativeQueue<DijkstraTile>(Allocator.Persistent);
    }


    private void Update()
    {
        // if we follow a target && we do not compute again the same path (every 0.5 sec)
        if (!computingJobs && following && Time.realtimeSinceStartup - delay > 1)
            ChangeTarget(TrStartPosition, gridId, true);

        if (computingJobs && handle.IsCompleted)
        {
            // complete dijkstra
            handle.Complete();

            DebugFeeder.instance.lastPathingTime = Time.realtimeSinceStartup - pathingDelay;
            flowFieldDelay = Time.realtimeSinceStartup;

            // flowfield
            NativeArray<DijkstraTile> tempDijkstra = new NativeArray<DijkstraTile>(pathRegister.grids[gridId], Allocator.TempJob);
            var jobData = new FlowFieldGrid
            {
                gridSize = new int2(pathRegister.iGridSizeX, pathRegister.iGridSizeY),
                RdGrid = tempDijkstra,
                grid = pathRegister.grids[gridId]
            };
            handle = jobData.Schedule(pathRegister.grids[gridId].Length, 32 /* batches */);
            handle.Complete();

            tempDijkstra.Dispose();
            computingJobs = false;

            DebugFeeder.instance.lastFlowFIeldTime = Time.realtimeSinceStartup - flowFieldDelay;
            DebugFeeder.instance.lastTotalTime = Time.realtimeSinceStartup - delay;

            // Debug.Log("precise total : " + (Time.realtimeSinceStartup - delay));
        }
    }


    // Change target position
    public void ChangeTarget(Transform newStartPosition, int gridId, bool follow)
    {
        // needed before the check
        following = follow;

        if (newStartPosition.position == targetPosition)
            return;

        this.gridId = gridId;
        computingJobs = true;
        targetPosition = newStartPosition.position;
        if (following)
            TrStartPosition = newStartPosition;
        else
            TrStartPosition = null;

        // imprecise grid clear
        var jobUpdateGridImprecise = new UpdateGrid
        {
            grid = pathRegister.impreciseGrids[gridId]
        };
        handle = jobUpdateGridImprecise.Schedule(pathRegister.impreciseGrids[gridId].Length, 32 /* batches */);
        handle.Complete();

        // imprecise dijkstra
        var jobDataDijImprecise = new DijkstraGrid
        {
            target = NodeFromWorldPoint(targetPosition),
            gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY),
            toVisit = toVisit,
            grid = pathRegister.impreciseGrids[gridId]
        };
        jobDataDijImprecise.Run();

        // imprecise flowfield
        NativeArray<DijkstraTile> imprecisetempDijkstra = new NativeArray<DijkstraTile>(pathRegister.impreciseGrids[gridId], Allocator.TempJob);
        var jobDataImprecise = new FlowFieldGrid
        {
            gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY),
            RdGrid = imprecisetempDijkstra,
            grid = pathRegister.impreciseGrids[gridId]
        };
        handle = jobDataImprecise.Schedule(pathRegister.impreciseGrids[gridId].Length, 32 /* batches */);
        handle.Complete();
        imprecisetempDijkstra.Dispose();

        delay = Time.realtimeSinceStartup;
        clearDelay = Time.realtimeSinceStartup;

        // grid clear
        var jobUpdateGrid = new UpdateGrid
        {
            grid = pathRegister.grids[gridId]
        };
        handle = jobUpdateGrid.Schedule(pathRegister.grids[gridId].Length, 32 /* batches */);
        handle.Complete();

        DebugFeeder.instance.lastClearTime = Time.realtimeSinceStartup - clearDelay;
        pathingDelay = Time.realtimeSinceStartup;

        // dijkstra start
        var jobDataDij = new DijkstraGrid
        {
            target = NodeFromWorldPoint(targetPosition, true),
            gridSize = new int2(pathRegister.iGridSizeX, pathRegister.iGridSizeY),
            toVisit = toVisit,
            grid = pathRegister.grids[gridId]
        };
        handle = jobDataDij.Schedule();
    }


    // Gets the closest node to the given world position
    public DijkstraTile NodeFromWorldPoint(Vector3 a_vWorldPos, bool bypass = false)
    {
        float ixPos = (a_vWorldPos.x + pathRegister.vGridWorldSize.x / 2) / pathRegister.vGridWorldSize.x;
        float iyPos = (a_vWorldPos.z + pathRegister.vGridWorldSize.y / 2) / pathRegister.vGridWorldSize.y;

        if (!computingJobs || bypass)
        {
            int ix = (int)(ixPos * pathRegister.iGridSizeX);
            int iy = (int)(iyPos * pathRegister.iGridSizeY);
            return pathRegister.grids[gridId][pathRegister.iGridSizeY * ix + iy];
        }
        else
        {
            int ix = (int)(ixPos * pathRegister.impreciseiGridSizeX);
            int iy = (int)(iyPos * pathRegister.impreciseiGridSizeY);
            return pathRegister.impreciseGrids[gridId][pathRegister.impreciseiGridSizeY * ix + iy];
        }
    }


    private void OnDestroy()
    {
        toVisit.Dispose();
    }
}
