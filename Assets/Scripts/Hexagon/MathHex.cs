using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Transform2D
{
    public Vector2 x;
    public Vector2 y;


    public Transform2D()
    {
        x = Vector2.zero;
        y = Vector2.zero;
    }

    public Transform2D(Vector2 x, Vector2 y)
    {
        this.x = x;
        this.y = y;
    }

    public static Transform2D CalculateHexBasis(float hexSize)
    {
        var gridBasis = CalculateGridBasis(hexSize);
        var x = gridBasis.x * 2;
        var y = gridBasis.x + gridBasis.y * 3;
        var hexBasis = new Transform2D(x , y);
        return hexBasis;
    }

    public static Transform2D CalculateGridBasis(float hexSize)
    {
        var x = new Vector2(CalculateShortSide(hexSize), 0);
        var y = new Vector2(0, CalculateLongSide(hexSize));
        var gridBasis = new Transform2D(x, y);
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
        result.x.x = (float)(basis.y.y * idet);
        result.x.y = (float)(-basis.x.y * idet);
        result.y.y = (float)(basis.x.x * idet);
        result.y.x = (float)(-basis.y.x * idet);
        return result;
    }
}

public class HexPosition // скорее всего слишком медленный код, переделай (потом)
{
    private Vector2Int cellInWorld;

    #region Initialize Coordinate
    public HexPosition(Vector3 point)
    {
        WorldCoordinate = point;
    }

    public HexPosition(Vector2Int position)
    {
        CellInWorld = position;
    }


    public Vector3 WorldCoordinate
    {
        get 
        {
            return ConvertToWorldCoordinate(cellInWorld);
        }
        set
        {
            cellInWorld = ConvetrToHexPosition(value);
        }
    }

    public Vector2Int Chunk
    {
        get 
        {
            return cellInWorld / ChunkSettings.ChunkSize;
        }
        set
        {
            var chunkDelta = value - Chunk;
            cellInWorld += chunkDelta * ChunkSettings.ChunkSize;
        }
    }
    public Vector2Int CellInChunk
    {
        get 
        {
            return new Vector2Int(cellInWorld.x % ChunkSettings.ChunkSize, cellInWorld.y % ChunkSettings.ChunkSize);
        }
        set
        {
            var cellDelta = value - CellInChunk;
            cellInWorld += cellDelta;
        }
    }
    public Vector2Int CellInWorld
    {
        get 
        {
            return cellInWorld;
        }
        set
        {
            cellInWorld = value;
        }
    }

    public int ChunkIndex
    {
        get
        {
            var chunkPosition = CellInChunk;
            return chunkPosition.x * ChunkSettings.ChunkSize + chunkPosition.y;
        }
    }
    #endregion


    #region Calculate Coordinate
    public static Vector2Int ConvetrToHexPosition(Vector3 worldCoordinate)
    {
        return RoundHex(ConvertCoordinate(worldCoordinate, CellSettings.InvertHexBasis));
    }
    
    public static Vector3 ConvertToWorldCoordinate(Vector2Int hexPosition)
    {
        var worldCoordinate = ConvertCoordinate(hexPosition, CellSettings.HexBasis);
        return new Vector3(worldCoordinate.x, 0, worldCoordinate.y);
    }
    
    
    
    public static Vector2 ConvertCoordinate(Vector2 coordinate, Transform2D basis)
    {
        var locateInCellCoordinate = new Vector2();
        locateInCellCoordinate.x = coordinate.x * basis.x.x +
            coordinate.y * basis.y.x;
        locateInCellCoordinate.y = coordinate.x * basis.x.y +
            coordinate.y * basis.y.y;
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
