using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Hex/_settings")]
public class HexMainSettings : ScriptableObject
{   
    public static HexMainSettings Instance { get; private set; }

    public void OnValidate()
    {
        Instance = this;
    }

    [SerializeField] private int _hexSize;
    [SerializeField] private int _chunckSize;
    [SerializeField] private int _worldSimulateRadius;
    [SerializeField] private int _worldLoadedRadius;

    public int HexSize
    {
        get { return _chunckSize; }
    }
    public int ChunkSize
    {
        get { return _chunckSize; }
    }
    public int WorldSimulateRadius
    {
        get { return _worldSimulateRadius; }
    }
    public int WorldLoadRadius
    {
        get { return _worldLoadedRadius; }
    }

    public Transform2D HexBasis { get; private set; }
    public Transform2D InvertHexBasis { get; private set; }

    public Quaternion UnityStupidRotationFix { get; private set; }
    public Vector3 UnityStupidScaleFix { get; private set; }


    public void OnEnable()
    {
        UnityStupidRotationFix = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
        UnityStupidScaleFix = new Vector3(100, 100, 100);

        HexBasis = CalculateHexBasis(HexSize);
        InvertHexBasis = InvertBasis(HexBasis);
    }

    private static Transform2D CalculateHexBasis(float hexSize)
    {
        var gridBasis = CalculateGridBasis(hexSize);
        var hexBasis = new Transform2D();
        hexBasis.x = gridBasis.x * 2;
        hexBasis.y = gridBasis.x + gridBasis.y * 3;
        return hexBasis;
    }
    private static Transform2D CalculateGridBasis(float hexSize)
    {
        var gridBasis = new Transform2D();
        gridBasis.x = new Vector2(CalculateShortSide(hexSize), 0);
        gridBasis.y = new Vector2(0, CalculateLongSide(hexSize));
        return gridBasis;
    }

    private static float CalculateShortSide(float hexSize)
    {
        var shortSide = (float)(hexSize * Math.Sqrt(3) / 2);
        return shortSide;
    }
    private static float CalculateLongSide(float hexSize)
    {
        var longSide = hexSize / 2;
        return longSide;
    }

    private static Transform2D InvertBasis(Transform2D basis)
    {
        var det = basis.x.x * basis.y.y - basis.y.x * basis.x.y;
        var idet = 1.0 / det;

        var result = new Transform2D();
        result.y.y = (float)(basis.x.x * idet);
        result.x.x = (float)(basis.y.y * idet);
        result.x.y = (float)(-basis.x.y * idet);
        result.y.x = (float)(-basis.y.x * idet);
        return result;
    }
}
