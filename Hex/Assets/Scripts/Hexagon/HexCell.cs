using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Material))]
public class HexCell : MonoBehaviour
{
    private float _height = 0;
    public Vector2 Position;

    public float Height
    {
        get { return _height; }
        set { _height = value; }
    }
}
