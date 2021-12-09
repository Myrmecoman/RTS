using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


[BurstCompile(FloatPrecision = FloatPrecision.Low, FloatMode = FloatMode.Fast), NoAlias]
public struct FlowFieldRestrainedJob : IJobParallelFor
{
    [ReadOnly, NoAlias]
    public int2 gridSize;
    [ReadOnly, NoAlias]
    public NativeArray<DijkstraTile> RdGrid;
    [WriteOnly, NoAlias]
    public NativeArray<DijkstraTile> grid;


    public void Execute(int i)
    {
        //skip current iteration if index has obstacle
        if (RdGrid[i].weight == int.MaxValue || RdGrid[i].weight == -1)
            return;

        int2 pos = new int2(i / gridSize.y, i % gridSize.y);
        int2 n;
        int2 min = int2.zero;
        int minDist = 0;

        bool bleft = isValid(pos.x - 1, pos.y);
        bool bup = isValid(pos.x, pos.y - 1);
        bool bright = isValid(pos.x + 1, pos.y);
        bool bdown = isValid(pos.x, pos.y + 1);
        bool bleftup = bup && isValid(pos.x - 1, pos.y - 1);
        bool bupright = bright && isValid(pos.x + 1, pos.y - 1);
        bool bdownright = bdown && isValid(pos.x + 1, pos.y + 1);
        bool bdownleft = bleft && isValid(pos.x - 1, pos.y + 1);

        if (bleft)
        {
            n = new int2(pos.x - 1, pos.y);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bleftup && bleft)
        {
            n = new int2(pos.x - 1, pos.y - 1);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bup)
        {
            n = new int2(pos.x, pos.y - 1);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bupright && bup)
        {
            n = new int2(pos.x + 1, pos.y - 1);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bright)
        {
            n = new int2(pos.x + 1, pos.y);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bdownright && bright)
        {
            n = new int2(pos.x + 1, pos.y + 1);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bdown)
        {
            n = new int2(pos.x, pos.y + 1);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        if (bdownleft && bdown)
        {
            n = new int2(pos.x - 1, pos.y + 1);
            int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
            if (dist < minDist)
            {
                min = n;
                minDist = dist;
            }
        }

        //If we found a valid neighbour, point in its direction
        if (minDist < 0)
            grid[i] = new DijkstraTile(RdGrid[i].gridPos, RdGrid[i].weight, min - pos);

    }


    private bool isValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < gridSize.x && y < gridSize.y && RdGrid[gridSize.y * x + y].weight != int.MaxValue && RdGrid[gridSize.y * x + y].weight != -1;
    }
}
