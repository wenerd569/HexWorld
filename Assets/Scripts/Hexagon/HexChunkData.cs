using System;
using UnityEngine;

public class HexChunkData
{
    private HexCell[,] _hexCells;

    public HexChunkData(int size)
    {
        _hexCells = new HexCell[size, size];
    }

    public HexCell this[int x, int y]
    {
        get
        {
            return _hexCells[x, y];
        }
        set
        {
            _hexCells[x, y] = value;
        }
    }
}
