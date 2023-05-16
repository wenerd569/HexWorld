using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Material))]
public abstract class HexCell : MonoBehaviour
{
	[SerializeField] protected float startHeight = 0;
	[SerializeField] protected float mass = 0;
	[SerializeField] protected float springRate = 0;
	[SerializeField] protected float friction—oefficient = 0;
    protected float speed = 0;
    protected float cerrentHeight = 0;
	protected float force = 0;

}
