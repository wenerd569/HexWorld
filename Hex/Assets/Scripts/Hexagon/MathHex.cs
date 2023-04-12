using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

public class HexPosition
{
    private Vector2 _worldCoordinate;
    private Vector2Int _chunk;
    private Vector2Int _cellInChunk;
    private Vector2Int _cellInWorld;
    private HexMainSettings _settings;

    public HexPosition()
    {
        _settings = HexMainSettings.Instance;
    }

    public HexPosition(Vector3 point)
    {
        _settings = HexMainSettings.Instance;
        RecalculateAllCoordinate(point);
    }

    public Vector3 WorldCoordinate // Vector3 тут только для удобства передачи
    {
        get { return new Vector3(_worldCoordinate.x, 0, _worldCoordinate.y); }
        set
        {
            RecalculateAllCoordinate(new Vector2(value.x, value.z));
        }
    }

    public Vector2Int Chunk
    {
        get { return _chunk; }
        set
        {
            var chunckDelta = _chunk - value;
            RecalculateAllCoordinate(_worldCoordinate + ConvertCoordinate(chunckDelta, _settings.ChunkBasis));
        }
    }
    public Vector2Int CellInChunk
    {
        get { return _cellInChunk; }
        set
        {
            var celldelta = _cellInChunk - value;
            RecalculateAllCoordinate(_worldCoordinate + ConvertCoordinate(celldelta, _settings.HexBasis));
        }
    }
    public Vector2Int CellInWorld
    {
        get { return _cellInWorld; }
        set
        {
            var celldelta = _cellInWorld - value;
            RecalculateAllCoordinate(_worldCoordinate + ConvertCoordinate(celldelta, _settings.HexBasis));
        }
    }

    public void RecalculateAllCoordinate(Vector2 worldCoordinate)
    {
        _worldCoordinate = worldCoordinate;
        _cellInWorld = RoundHex(ConvertCoordinate(worldCoordinate, _settings.InvertHexBasis));
        _chunk = RoundHex(ConvertCoordinate(worldCoordinate, _settings.InvertChunkBasis));
        _cellInChunk = CalculateCoordinateInChunck(_chunk, _cellInWorld);
    }

    private Vector2Int CalculateCoordinateInChunck(Vector2Int chunk, Vector2Int cellInWorld)
    {
        var result = new Vector2Int();
        result = -RoundHex(ConvertCoordinate(chunk, _settings.ChunkInHexBasis));
        result.x += cellInWorld.x;
        result.y += cellInWorld.y;
        return result;
    }

    private Vector2 ConvertCoordinate(Vector2 locateInWorldCoordinate, Transform2D basis)
    {
        var locateInCellCoordinate = new Vector2();
        locateInCellCoordinate.x = locateInWorldCoordinate.x * basis.x.x +
            locateInWorldCoordinate.y * basis.y.x;
        locateInCellCoordinate.y = locateInWorldCoordinate.x * basis.x.y +
            locateInWorldCoordinate.y * basis.y.y;
        return locateInCellCoordinate;
    }

    private static Vector2Int RoundHex(Vector2 hex)
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
