using UnityEngine;

public interface IAttackType
{
//    public float AttackRadius { get; set; }
//    public float AttackDuration { get; set; }

    void Target(GameObject targetObject, Vector3 targetPoint, Vector3 targetDirection, float forces);
}