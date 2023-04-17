using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Material))]
public abstract class HexCell : MonoBehaviour
{
    protected float _height = 0;
    protected Vector3 _position;
    public Vector2 PositionInChunk;

    public virtual float Height
    {
        get 
        { 
            return _height;
        }
        set 
        { 
            _height = value;
            _position = transform.position;
            _position.z = _height;
            transform.Translate(_position);
        }
    }
}
