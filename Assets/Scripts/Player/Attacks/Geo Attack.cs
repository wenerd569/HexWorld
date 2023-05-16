using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoAttack : MonoBehaviour, IAttackType
{
    [SerializeField] protected AnimationCurve _targetShape;
    [SerializeField] protected float AttackRadius;
    [SerializeField] protected float AttackDuration;

    public void Target(GameObject targetObject, Vector3 targetPoint, Vector3 targetDirection, float forces)
    {
        var hexPosition = new HexPosition(targetPoint);
        RegionTarget(hexPosition, forces, (int)AttackRadius, _targetShape);
    }

    public static void RegionTarget(HexPosition hexPosition, float forces, int radius, AnimationCurve curve)
    {
        var hexArea = HexPosition.GetAllHexCellInArea(hexPosition, radius);
        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                if (Math.Abs(x + y) <= radius)
                {
                    var currentRadius = Math.Abs(x + y);
                    hexArea[x, y].Height = forces * curve.Evaluate(currentRadius / radius);
                }
            }
        }
    }
}