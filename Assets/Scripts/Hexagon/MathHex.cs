using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Transform2D
{
    public Vector2 x;
    public Vector2 y;
}

public class HexRotation //...хуйня
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


public class HexPosition // скорее всего слишком медленный код, переделай (потом)
{
    private Vector2Int _cellInWorld;
    private HexMainSettings _settings;

    #region Initialize Coordinate
    public HexPosition(Vector3 point)
    {
        _settings = HexMainSettings.Instance;
        WorldCoordinate = point;
    }

    public Vector3 WorldCoordinate
    {
        get 
        {
            var worldCoordinate = ConvertCoordinate(_cellInWorld, _settings.HexBasis);
            return new Vector3(worldCoordinate.x, 0, worldCoordinate.y);
        }
        set
        {
            _cellInWorld = RoundHex(ConvertCoordinate(value, _settings.InvertHexBasis));
        }
    }

    public Vector2Int Chunk
    {
        get 
        {
            return _cellInWorld / _settings.ChunkSize;
        }
        set
        {
            var chunkDelta = value - Chunk;
            _cellInWorld += chunkDelta * _settings.ChunkSize;
        }
    }
    public Vector2Int CellInChunk
    {
        get 
        {
            return new Vector2Int(_cellInWorld.x % _settings.ChunkSize, _cellInWorld.y % _settings.ChunkSize);
        }
        set
        {
            var cellDelta = value - CellInChunk;
            _cellInWorld += cellDelta;
        }
    }
    public Vector2Int CellInWorld
    {
        get 
        {
            return _cellInWorld;
        }
        set
        {
            _cellInWorld = value;
        }
    }
    #endregion


    #region Calculate Coordinate
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
    #endregion


    #region Additional Functions
    /*public static HexChunkData GetAllHexCellInArea(HexPosition hexPosition, int radius)
    {
        var hexArea = new HexChunkData(radius);

        var tempHexPosition = new HexPosition(); 
        for (int x  = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                if (Math.Abs(x + y) <= radius)
                {
                    tempHexPosition.CellInWorld = hexPosition.CellInWorld + new Vector2Int(x, y);
                    hexArea[x, y] = HexMain.Instance.Chunks[tempHexPosition.Chunk].HexChunkData[tempHexPosition.CellInChunk.x, tempHexPosition.CellInChunk.y];
                }
            }
        }
        return hexArea;
    }*/
    #endregion
}
