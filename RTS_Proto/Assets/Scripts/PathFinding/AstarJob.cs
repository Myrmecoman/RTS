using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


[BurstCompile(FloatPrecision = FloatPrecision.Low, FloatMode = FloatMode.Fast), NoAlias]
public struct AstarJob : IJob
{
    [NoAlias, ReadOnly]
    public NativeArray<DijkstraTile> RdGrid;
    [NoAlias, ReadOnly]
    public int2 gridSize;
    [NoAlias, ReadOnly]
    public int2 start;
    [NoAlias, ReadOnly]
    public int2 end;
    [NoAlias]
    public NativePriorityQueue<int2> _frontier;
    [NoAlias]
    public NativeHashMap<int2, int2> _parents;
    [NoAlias]
    public NativeHashMap<int2, int> _costs;
    [NoAlias]
    public NativeList<int2> _neighbours;
    [NoAlias]
    public NativeList<int2> _output;


    public void Execute()
    {
        _frontier.Enqueue(start, 0);
        _costs[start] = 0;

        while (_frontier.Length > 0)
        {
            var currNode = _frontier.Dequeue();

            var curr = currNode;
            if (curr.Equals(end))
                break;

            _neighbours.Clear();
            bool bleft = isValid(curr.x - 1, curr.y);
            if (bleft)
                _neighbours.Add(new int2(curr.x - 1, curr.y));

            bool bup = isValid(curr.x, curr.y - 1);
            if (bup)
                _neighbours.Add(new int2(curr.x, curr.y - 1));

            bool bright = isValid(curr.x + 1, curr.y);
            if (bright)
                _neighbours.Add(new int2(curr.x + 1, curr.y));

            bool bdown = isValid(curr.x, curr.y + 1);
            if (bdown)
                _neighbours.Add(new int2(curr.x, curr.y + 1));

            bool bleftup = isValid(curr.x - 1, curr.y - 1);
            if (bleftup)
                _neighbours.Add(new int2(curr.x - 1, curr.y - 1));

            bool bupright = isValid(curr.x + 1, curr.y - 1);
            if (bupright)
                _neighbours.Add(new int2(curr.x + 1, curr.y - 1));

            bool bdownright = isValid(curr.x + 1, curr.y + 1);
            if (bdownright)
                _neighbours.Add(new int2(curr.x + 1, curr.y + 1));

            bool bdownleft = isValid(curr.x - 1, curr.y + 1);
            if (bdownleft)
                _neighbours.Add(new int2(curr.x - 1, curr.y + 1));

            for (int i = 0; i < _neighbours.Length; ++i)
            {
                var next = _neighbours[i];

                int newCost = _costs[curr] + 1;

                if (!_costs.TryGetValue(next, out int nextCost) || newCost < nextCost)
                {
                    _costs[next] = newCost;
                    int priority = newCost + (int) math.sqrt((next.x - end.x) * (next.x - end.x) + (next.y - end.y) * (next.y - end.y));
                    _frontier.Enqueue(next, priority);
                    _parents[next] = curr;
                }
            }
        }

        GetPath();

        _parents.Clear();
        _costs.Clear();
        _frontier.Clear();

        // neighbours is cleared in loop
        // output is used outside
    }


    private void GetPath()
    {
        if (!_parents.ContainsKey(end))
            // Pathfinding failed
            return;

        _output.Clear();

        int2 curr = end;

        while (!curr.Equals(start))
        {
            _output.Add(curr);
            curr = _parents[curr];
        }

        _output.Add(start);
    }


    private bool isValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < gridSize.x && y < gridSize.y && RdGrid[gridSize.y * x + y].weight != int.MaxValue;
    }
}
