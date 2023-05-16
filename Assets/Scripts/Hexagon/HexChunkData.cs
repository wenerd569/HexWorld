using System;
using UnityEngine;

public class HexChunkData
{
    private HexCell[][] _hexCells;
    public readonly int ChunkRadius;
    public readonly int ChunkDiametr;


    public HexChunkData(int radius)
    {
        ChunkRadius = radius;
        ChunkDiametr = radius * 2 + 1;

        _hexCells = new HexCell[ChunkDiametr][];
        for (int x = 0; x < ChunkDiametr; x++)
        {
            _hexCells[x] = new HexCell[ChunkDiametr - Math.Abs(ChunkRadius - x)];
        }
    }

    public HexCell this[int x, int y]
    {
        get
        {
            return _hexCells[x + ChunkRadius][y + ChunkRadius - Math.Max(0, -x)];
        }
        set
        {
            _hexCells[x + ChunkRadius][y + ChunkRadius - Math.Max(0, -x)] = value;
        }
    }
}
