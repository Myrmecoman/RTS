using Unity.Burst;
using Unity.Mathematics;


[NoAlias]
public struct DijkstraTile
{
    [NoAlias]
    public int weight;
    [NoAlias]
    public int2 gridPos;
    [NoAlias]
    public int2 FlowFieldVector;

    public DijkstraTile(int2 gridPos)
    {
        this.gridPos = gridPos;
        this.weight = -1;
        this.FlowFieldVector = int2.zero;
    }

    public DijkstraTile(int2 gridPos, int weight, int2 FlowFieldVector)
    {
        this.gridPos = gridPos;
        this.weight = weight;
        this.FlowFieldVector = FlowFieldVector;
    }
}
