using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public float Distance { get; set; }
    public float Duration { get; set; }
    public abstract void Attack();
}


public class GeoAttack : ScriptableObject, IAttack
{
    public float Distance { get; set; }
    public float Duration { get; set; }

    public override void Attack()
    {

    }
}

public class MleeAttack : ScriptableObject, IAttack
{
    public float Distance { get; set; }
    public float Duration { get; set; }

    public override void Attack()
    {

    }
}

public class ShootAttack : ScriptableObject, IAttack
{
    public float Distance { get; set; }
    public float Duration { get; set; }

    public override void Attack()
    {

    }
}
