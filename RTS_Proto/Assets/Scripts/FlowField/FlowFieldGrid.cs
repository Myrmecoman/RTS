using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile(FloatPrecision = FloatPrecision.Low)]
public struct FlowFieldGrid : IJobParallelFor
{
    [ReadOnly]
    public int2 gridSize;
    [ReadOnly]
    public NativeArray<DijkstraTile> RdGrid;
    [WriteOnly]
    public NativeArray<DijkstraTile> grid;


    public void Execute(int i)
    {
        int2 pos;
        int2 min;
        int2 n;
        //skip current iteration if index has obstacle
        if (RdGrid[i].weight != int.MaxValue)
        {
            pos.x = i / gridSize.y;
            pos.y = i % gridSize.y;

            bool minNotNull = false;
            int minDist = 0;
            min = int2.zero; // this may be incorrect

            bool bleft = isValid(pos.x - 1, pos.y);
            int2 left = new int2(pos.x - 1, pos.y);

            bool bleftup = isValid(pos.x, pos.y - 1) /* bup */ && isValid(pos.x - 1, pos.y - 1);
            int2 leftup = new int2(pos.x - 1, pos.y - 1);

            bool bup = isValid(pos.x, pos.y - 1);
            int2 up = new int2(pos.x, pos.y - 1);

            bool bupright = isValid(pos.x + 1, pos.y) /* right */ && isValid(pos.x + 1, pos.y - 1);
            int2 upright = new int2(pos.x + 1, pos.y - 1);

            bool bright = isValid(pos.x + 1, pos.y);
            int2 right = new int2(pos.x + 1, pos.y);

            bool bdownright = isValid(pos.x, pos.y + 1) /* down */ && isValid(pos.x + 1, pos.y + 1);
            int2 downright = new int2(pos.x + 1, pos.y + 1);

            bool bdown = isValid(pos.x, pos.y + 1);
            int2 down = new int2(pos.x, pos.y + 1);

            bool bdownleft = bleft && isValid(pos.x - 1, pos.y + 1);
            int2 downleft = new int2(pos.x - 1, pos.y + 1);

            if (bleft)
            {
                n = left;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bleftup && bleft && bup)
            {
                n = leftup;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bup)
            {
                n = up;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bupright && bup && bright)
            {
                n = upright;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bright)
            {
                n = right;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bdownright && bdown && bright)
            {
                n = downright;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bdown)
            {
                n = down;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            if (bdownleft && bdown && bleft)
            {
                n = downleft;
                int dist = RdGrid[gridSize.y * n.x + n.y].weight - RdGrid[gridSize.y * pos.x + pos.y].weight;
                if (dist < minDist)
                {
                    min = n;
                    minNotNull = true;
                    minDist = dist;
                }
            }

            //If we found a valid neighbour, point in its direction
            if (minNotNull)
                grid[i] = new DijkstraTile(RdGrid[i].gridPos, RdGrid[i].weight, min - pos); //potential problem
        }
    }


    private bool isValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < gridSize.x && y < gridSize.y && RdGrid[gridSize.y * x + y].weight != int.MaxValue;
    }
}
