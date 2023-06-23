using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HexAutoring : MonoBehaviour
{
    public float Mass;
    public float StartHeight;
}

public class HexBaker : Baker<HexAutoring>
{
    public override void Bake(HexAutoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new Hex { Mass = authoring.Mass, StartHeight = authoring.StartHeight });
    }
}
