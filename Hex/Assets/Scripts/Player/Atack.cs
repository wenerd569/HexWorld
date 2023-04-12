using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    float Couldown { get; }
    void Atack(Vector3 point);
}

public abstract class GeoWeapon : IWeapon
{
    public float Couldown { get; }

    public  void Atack(Vector3 point)
    {
        var atackDot = new HexPosition(point);

    }
}
