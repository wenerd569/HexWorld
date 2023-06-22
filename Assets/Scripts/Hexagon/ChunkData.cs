using System;
using System.Drawing;
using UnityEngine;

public class ChunkData
{
    private HexCell[,] _hexCells;

    public ChunkData(int size, float height, HexPhisicMaterial phisicMaterial)
    {
        _hexCells = GenerateStandartPlane(size, height, phisicMaterial);
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

    private HexCell[,] GenerateStandartPlane(int size, float height, HexPhisicMaterial phisicMaterial)
    {
        var result = new HexCell[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y <= size; y++)
            {
                result[x, y] = new HexCell(height, phisicMaterial);
            }
        }
        return result;
    }
}
