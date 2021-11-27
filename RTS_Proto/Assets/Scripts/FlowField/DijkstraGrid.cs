using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


[BurstCompile(FloatPrecision = FloatPrecision.Low, FloatMode = FloatMode.Fast), NoAlias]
public struct DijkstraGrid : IJob
{
    [ReadOnly, NoAlias]
    public DijkstraTile target;
    [ReadOnly, NoAlias]
    public int2 gridSize;
    [NoAlias]
    public NativeArray<DijkstraTile> grid;


    public void Execute()
    {
        // flood fill out from the end point
        DijkstraTile destination = target;
        destination.weight = 0;
        grid[gridSize.y * destination.gridPos.x + destination.gridPos.y] = new DijkstraTile(grid[gridSize.y * destination.gridPos.x + destination.gridPos.y].gridPos, 0, grid[gridSize.y * destination.gridPos.x + destination.gridPos.y].FlowFieldVector);

        NativeList<DijkstraTile> toVisit = new NativeList<DijkstraTile>(Allocator.Temp);
        toVisit.Add(destination);

        // for each node we need to visit, starting with the pathEnd
        for (int i = 0; i < toVisit.Length; i++)
        {
            if (toVisit[i].gridPos.x > 0) // left
            {
                DijkstraTile neighbour = new DijkstraTile(new int2(toVisit[i].gridPos.x - 1, toVisit[i].gridPos.y));
                // if tile has not been visited
                if (grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].weight == -1)
                {
                    neighbour.weight = toVisit[i].weight + 1;
                    grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y] = new DijkstraTile(grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].gridPos, neighbour.weight, grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].FlowFieldVector);
                    toVisit.Add(neighbour);
                }
            }

            if (toVisit[i].gridPos.y > 0) // up
            {
                DijkstraTile neighbour = new DijkstraTile(new int2(toVisit[i].gridPos.x, toVisit[i].gridPos.y - 1));
                // if tile has not been visited
                if (grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].weight == -1)
                {
                    neighbour.weight = toVisit[i].weight + 1;
                    grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y] = new DijkstraTile(grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].gridPos, neighbour.weight, grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].FlowFieldVector);
                    toVisit.Add(neighbour);
                }
            }

            if (toVisit[i].gridPos.x < gridSize.x - 1) // right
            {
                DijkstraTile neighbour = new DijkstraTile(new int2(toVisit[i].gridPos.x + 1, toVisit[i].gridPos.y));
                // if tile has not been visited
                if (grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].weight == -1)
                {
                    neighbour.weight = toVisit[i].weight + 1;
                    grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y] = new DijkstraTile(grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].gridPos, neighbour.weight, grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].FlowFieldVector);
                    toVisit.Add(neighbour);
                }
            }

            if (toVisit[i].gridPos.y < gridSize.y - 1) // down
            {
                DijkstraTile neighbour = new DijkstraTile(new int2(toVisit[i].gridPos.x, toVisit[i].gridPos.y + 1));
                // if tile has not been visited
                if (grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].weight == -1)
                {
                    neighbour.weight = toVisit[i].weight + 1;
                    grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y] = new DijkstraTile(grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].gridPos, neighbour.weight, grid[gridSize.y * neighbour.gridPos.x + neighbour.gridPos.y].FlowFieldVector);
                    toVisit.Add(neighbour);
                }
            }
        }
    }
}
