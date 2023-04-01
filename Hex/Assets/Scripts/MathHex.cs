using System;
using UnityEngine;
using UnityEngine.UIElements;

public static class MathHex
{
    public static Transform2D CalculateHexBasis(float hexSize)
    {
        var gridBasis = CalculateGridBasis(hexSize);
        var hexBasis = new Transform2D();
        hexBasis.x = gridBasis.x * 2;
        hexBasis.y = gridBasis.x + gridBasis.y * 3;
        return hexBasis;
    }
    public static Transform2D CalculateGridBasis(float hexSize)
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

    public static Transform2D InvertBasis(Transform2D basis)
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
    public static Transform2D CalculateChunkBasis(Transform2D hexBasis, int chunkRadius)
    {
        var chunkBasis = new Transform2D();
        chunkBasis.x = hexBasis.x * (chunkRadius * 2 + 1) - hexBasis.y * chunkRadius;
        chunkBasis.y = hexBasis.x * chunkRadius + hexBasis.y * (chunkRadius + 1);
        return chunkBasis;
    }



    public static Vector2Int CoordinateToHexCell(Vector3 locateInWorldCoordinate)
    {
        var invertGridBasis = HexMain.singleton.InvertGridBasis;
        var locateInCellCoordinate = new Vector2();
        locateInCellCoordinate.x = locateInWorldCoordinate.x * invertGridBasis.x.x +
            locateInWorldCoordinate.z * invertGridBasis.x.y;
        locateInCellCoordinate.y = locateInWorldCoordinate.x * invertGridBasis.x.y +
            locateInWorldCoordinate.z * invertGridBasis.y.y;
        return MathHex.RoundHex(locateInCellCoordinate);
    }

    public static Vector2Int RoundHex(Vector2 hex)
    {
        var rX = (int)hex.x;
        var rY = (int)hex.y;
        var rZ = (int)(-hex.x - hex.y);

        var diffX = Math.Abs(hex.x - rX);
        var diffY = Math.Abs(hex.y - rY);
        var diffZ = Math.Abs((-hex.x - hex.y) - rZ);

        if (diffX > diffY && diffX > diffZ)
        {
            rX = -rY - rZ;
        }
        else if (diffY > diffZ)
        {
            rY = -rX - rZ;
        }
        return new Vector2Int(rX, rY);
    }
}
public class Transform2D
{
    public Vector2 x;
    public Vector2 y;
}
public class HexRotation
{
    public static readonly Quaternion Right = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
    public static readonly Quaternion downRight = new Quaternion(0.612372398f, -0.353553504f, -0.353553504f, -0.612372398f);
    public static readonly Quaternion downLeft = new Quaternion(0.353553414f, -0.612372458f, -0.612372458f, -0.353553414f);
    public static readonly Quaternion Left = new Quaternion(0, -0.707106829f, -0.707106829f, 0);
    public static readonly Quaternion upLeft = new Quaternion(-0.353553355f, -0.612372458f, -0.612372458f, 0.353553355f);
    public static readonly Quaternion upRight = new Quaternion(-0.612372458f, -0.353553414f, -0.353553414f, 0.612372458f);
    private Rotate _rotate;

    public static Rotate TurnCounterClockWise(Rotate startAngle, int step)
    {
        return new Rotate(startAngle.angle.value + (60 * step));
    }
}
