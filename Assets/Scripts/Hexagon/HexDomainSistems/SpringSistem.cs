using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
public partial struct SpringSistem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var hexes = SystemAPI.Query<Hex>();

    }
}


