using Unity.Mathematics;


public struct DijkstraTile
{
    public int weight;
    public int2 gridPos;
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
