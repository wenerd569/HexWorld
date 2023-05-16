using UnityEngine;
using UnityEngine.Events;

public class RailGun : MonoBehaviour, IWeapon
{
    public float AbsoluteForce { get; protected set; }
    public IAttackType AttackType { get; protected set; }
    public virtual LayerMask LayerMask { get; protected set; }

    public UnityEvent<AttackData> Attaced { get; protected set; }

    public virtual void AdditionalAction()
    {

    }

    public virtual void Attack(MonoBehaviour atacker, Vector3 attackDirection)
    {
        var ray = new Ray(atacker.transform.position, attackDirection);

        if (Physics.Raycast(ray, out var hitInfo, LayerMask))
        {

            var hitObject = hitInfo.transform.gameObject;
            var hitPoint = hitInfo.transform.position;

            AttackType.Target(hitObject, hitPoint, attackDirection, AbsoluteForce);

            var attackData = new AttackData(
                atacker,
                hitObject,
                this,
                this.AttackType,
                attackDirection,
                hitPoint,
                this.AbsoluteForce);

            Attaced.Invoke(attackData);
        }
        else
        {

        }
    }
}
