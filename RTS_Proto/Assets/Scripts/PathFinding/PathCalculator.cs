using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


public class PathCalculator : MonoBehaviour
{
    [HideInInspector] public bool computingJobs = false;
    [HideInInspector] public bool following = false;
    [HideInInspector] public JobHandle handle;

    private Transform target;
    private double delay;
    private double clearDelay;
    private double pathingDelay;
    private double flowFieldDelay;
    private int gridId;
    private NativeQueue<DijkstraTile> toVisit;

    private PathRegister pathRegister;


    private void Start()
    {
        pathRegister = PathRegister.instance;
        toVisit = new NativeQueue<DijkstraTile>(Allocator.Persistent);
    }


    private void Update()
    {
        if (computingJobs && handle.IsCompleted)
        {
            // complete dijkstra
            handle.Complete();

            DebugFeeder.instance.lastPathingTime = Time.realtimeSinceStartup - pathingDelay;
            flowFieldDelay = Time.realtimeSinceStartup;

            // flowfield
            NativeArray<DijkstraTile> tempDijkstra = new NativeArray<DijkstraTile>(pathRegister.grids[gridId], Allocator.TempJob);
            var jobData = new FlowFieldJob
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
        }

        // if we follow a target && we do not compute again the same path (every sec)
        if (!computingJobs && target && following && Time.realtimeSinceStartup - delay > 1)
            ChangeTarget(target, gridId, true);
    }


    // Change target position
    public void ChangeTarget(Transform newStartPosition, int gridId, bool follow)
    {
        // needed before the check
        computingJobs = true;
        following = follow;
        this.gridId = gridId;
        target = newStartPosition;

        // imprecise grid clear
        var jobUpdateGridImprecise = new UpdateGridJob
        {
            grid = pathRegister.impreciseGrids[gridId]
        };
        handle = jobUpdateGridImprecise.Schedule(pathRegister.impreciseGrids[gridId].Length, 32 /* batches */);
        handle.Complete();

        // imprecise dijkstra
        var jobDataDijImprecise = new DijkstraJob
        {
            target = NodeFromWorldPoint(target.position),
            gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY),
            toVisit = toVisit,
            grid = pathRegister.impreciseGrids[gridId]
        };
        jobDataDijImprecise.Run();

        // imprecise flowfield
        NativeArray<DijkstraTile> imprecisetempDijkstra = new NativeArray<DijkstraTile>(pathRegister.impreciseGrids[gridId], Allocator.TempJob);
        var jobDataImprecise = new FlowFieldJob
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
        var jobUpdateGrid = new UpdateGridJob
        {
            grid = pathRegister.grids[gridId]
        };
        handle = jobUpdateGrid.Schedule(pathRegister.grids[gridId].Length, 32 /* batches */);
        handle.Complete();

        DebugFeeder.instance.lastClearTime = Time.realtimeSinceStartup - clearDelay;
        pathingDelay = Time.realtimeSinceStartup;

        // dijkstra start
        var jobDataDij = new DijkstraJob
        {
            target = NodeFromWorldPoint(target.position, true),
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
