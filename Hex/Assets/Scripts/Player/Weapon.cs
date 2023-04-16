using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeapon
{ 
    public float Couldown { get; }
    public IAttack _attack { get; }

    public UnityEvent<> Attaced { get; }

    public void Attack(MonoBehaviour atacker, Vector3 attackDirection);
    public void AdditionalAction();

}

public abstract class Gun : IWeapon
{
    public float Couldown { get; protected set; }
    public IAttack _attack { get; protected set; }

    public UnityEvent< > Attaced { get; protected set; }

    public virtual void Attack(MonoBehaviour atacker, Vector3 attackDirection)
    {
        

    }
    public void AdditionalAction()
    {

    }
}

public abstract class MeleeWeapon: IWeapon
{
    public float Couldown { get; protected set; }
    public IAttack _attack { get; protected set; }

    public Event Attaced { get; protected set; }

    public virtual void Attack(MonoBehaviour atacker, Vector3 attackDirection)
    {
        var ray = new

    }
    public void AdditionalAction()
    {

    }
}

public abstract class MagicWeapon: IWeapon
{
    public float Couldown { get; protected set; }
    public IAttack _attack { get; protected set; }

    public Event Attaced { get; protected set; }

    public virtual void Attack(MonoBehaviour atacker, Vector3 attackDirection)
    {
        var ray = new

    }
    public void AdditionalAction()
    {

    }
}

