using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;


[BurstCompile(FloatPrecision = FloatPrecision.Low)]
public struct UpdateGrid : IJobParallelFor
{
    public NativeArray<DijkstraTile> grid;


    public void Execute(int i)
    {
        if (grid[i].weight != int.MaxValue)
            grid[i] = new DijkstraTile(grid[i].gridPos, -1, grid[i].FlowFieldVector);
    }
}
