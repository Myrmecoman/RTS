using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


public class PathCalculator : MonoBehaviour
{
    [HideInInspector] public bool computingJobs;                   // Tells if we have access to precise pathfinding
    [HideInInspector] private Transform TrStartPosition;           // Keep a transform in case of changes (follow)
    [HideInInspector] public Vector3 targetPosition;               // This is where the program will start the pathfinding from

    // Needed to manage jobs without blocking
    private double delay;
    private double clearDelay;
    private double pathingDelay;
    private double flowFieldDelay;
    private int gridId;
    private NativeArray<DijkstraTile> tempDijkstra;
    private NativeQueue<DijkstraTile> toVisit;
    private JobHandle handle;

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
        if (TrStartPosition != null && TrStartPosition.position != targetPosition && Time.realtimeSinceStartup - delay > 0.5f)
            ChangeTarget(TrStartPosition, gridId, true);

        if (computingJobs && handle.IsCompleted)
        {
            // complete dijkstra
            handle.Complete();

            DebugFeeder.instance.lastPathingTime = Time.realtimeSinceStartup - pathingDelay;
            flowFieldDelay = Time.realtimeSinceStartup;

            // flowfield
            var jobData = new FlowFieldGrid();
            jobData.gridSize = new int2(pathRegister.iGridSizeX, pathRegister.iGridSizeY);
            tempDijkstra = new NativeArray<DijkstraTile>(PathRegister.instance.grids[gridId], Allocator.TempJob);
            jobData.RdGrid = tempDijkstra;
            jobData.grid = PathRegister.instance.grids[gridId];
            handle = jobData.Schedule(PathRegister.instance.grids[gridId].Length, 32 /* batches */);
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
        if (newStartPosition.position == targetPosition)
            return;

        computingJobs = true;
        this.gridId = gridId;

        if (follow)
            TrStartPosition = newStartPosition;
        else
            TrStartPosition = null;

        targetPosition = newStartPosition.position;

        // imprecise grid clear
        var jobUpdateGridImprecise = new UpdateGrid();
        jobUpdateGridImprecise.grid = PathRegister.instance.impreciseGrids[gridId];
        handle = jobUpdateGridImprecise.Schedule(PathRegister.instance.impreciseGrids[gridId].Length, 32 /* batches */);
        handle.Complete();

        // imprecise dijkstra
        var jobDataDijImprecise = new DijkstraGrid();
        jobDataDijImprecise.target = NodeFromWorldPoint(targetPosition);
        jobDataDijImprecise.gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY);
        jobDataDijImprecise.toVisit = toVisit;
        jobDataDijImprecise.grid = PathRegister.instance.impreciseGrids[gridId];
        jobDataDijImprecise.Run();

        // imprecise flowfield
        var jobData = new FlowFieldGrid();
        jobData.gridSize = new int2(pathRegister.impreciseiGridSizeX, pathRegister.impreciseiGridSizeY);
        NativeArray<DijkstraTile> imprecisetempDijkstra = new NativeArray<DijkstraTile>(PathRegister.instance.impreciseGrids[gridId], Allocator.TempJob);
        jobData.RdGrid = imprecisetempDijkstra;
        jobData.grid = PathRegister.instance.impreciseGrids[gridId];
        handle = jobData.Schedule(PathRegister.instance.impreciseGrids[gridId].Length, 32 /* batches */);
        handle.Complete();
        imprecisetempDijkstra.Dispose();

        delay = Time.realtimeSinceStartup;
        clearDelay = Time.realtimeSinceStartup;

        // grid clear
        var jobUpdateGrid = new UpdateGrid();
        jobUpdateGrid.grid = PathRegister.instance.grids[gridId];
        handle = jobUpdateGrid.Schedule(PathRegister.instance.grids[gridId].Length, 32 /* batches */);
        handle.Complete();

        DebugFeeder.instance.lastClearTime = Time.realtimeSinceStartup - clearDelay;
        pathingDelay = Time.realtimeSinceStartup;

        // need to deactivate temporarily for NodeFromWorldPoint
        computingJobs = false;

        // dijkstra start
        var jobDataDij = new DijkstraGrid();
        jobDataDij.target = NodeFromWorldPoint(targetPosition);
        computingJobs = true;
        jobDataDij.gridSize = new int2(pathRegister.iGridSizeX, pathRegister.iGridSizeY);
        jobDataDij.toVisit = toVisit;
        jobDataDij.grid = PathRegister.instance.grids[gridId];
        handle = jobDataDij.Schedule();

        computingJobs = true;
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


    private void OnDestroy()
    {
        toVisit.Dispose();
    }
}
