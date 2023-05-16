using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    public AttackData(MonoBehaviour attacer, GameObject AtacedObject, IWeapon weapon, IAttackType attackType, Vector3 atackDirection, Vector3 attackPoint, float atackForce)
    {
        Attacker = attacer;
        Weapon = weapon;
        AttackType = attackType;
        AtackDirection = atackDirection;
        AttackForce = atackForce;
    }
    public readonly MonoBehaviour Attacker;
    public readonly GameObject AtacedObject;
    public readonly IWeapon Weapon;
    public readonly IAttackType AttackType;
    public readonly Vector3 AtackDirection;
    public readonly Vector3 AttackPoint;
    public readonly float AttackForce;
}
