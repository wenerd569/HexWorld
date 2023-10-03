using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public struct PositionComponent : IComponentData, IEquatable<PositionComponent>
{
    public Vector2Int Position;

    public PositionComponent(Vector2Int pos)
    {
        Position = pos;
    }

    public bool Equals(PositionComponent other)
    {
        if (Position == other.Position) return true;
        return false;
    }
}

public struct HeightComponent : IComponentData
{
    public float Height;
    public float StartHeight;
}

public struct ForcesComponent : IComponentData
{
    public float Force;
    public float Speed;
    public float Mass;
}

public struct SpringComponent : IComponentData
{
    public float SpringCoefficient;
    public float FrictionCoefficient;
}
