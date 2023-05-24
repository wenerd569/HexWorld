using UnityEngine;

[CreateAssetMenu(fileName = "HexPhisicMaterial", menuName = "PhysicMaterial/", order = 1)]
public abstract class HexPhisicMaterial : PhysicMaterial
{
    public float SpringRate = 0;
    public float Mass = 0;
    public float Friction—oefficient = 0;
}
