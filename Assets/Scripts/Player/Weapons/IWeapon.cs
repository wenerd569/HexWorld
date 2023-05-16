using UnityEngine;
using UnityEngine.Events;

public interface IWeapon
{
    float AbsoluteForce { get; }
    IAttackType AttackType { get; }
    LayerMask LayerMask { get; }

    UnityEvent<AttackData> Attaced { get; }

    void Attack(MonoBehaviour atacker, Vector3 attackDirection);
    void AdditionalAction();
}
