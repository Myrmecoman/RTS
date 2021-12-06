using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


[BurstCompile(FloatPrecision = FloatPrecision.Low, FloatMode = FloatMode.Fast), NoAlias]
public struct AstarJob : IJob
{
    public void Execute()
    {
        throw new System.NotImplementedException();
    }
}
