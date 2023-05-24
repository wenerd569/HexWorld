using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HexCell : MonoBehaviour
{
    public struct Hex
    {
        public float StartHeight;
        public float SpringRate;
        public float Mass;
        public float Friction—oefficient;
    }


    public HexPhisicMaterial PhisicMaterial;
    public float StartHeight;

    [HideInInspector] public float Speed = 0;
	[HideInInspector] public float Force = 0;

    public HexCell(float height, HexPhisicMaterial phisicMaterial)
    {
        StartHeight = height;
        PhisicMaterial = phisicMaterial;
    }
}
