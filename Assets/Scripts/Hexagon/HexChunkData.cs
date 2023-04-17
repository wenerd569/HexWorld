using System;
using UnityEngine;

public class HexChunkData
{
    private HexMainSettings _settings;

    private HexCell[][] _hexCells;

    public HexCell this[int x, int y]
    {
        get
        {
            return _hexCells[x + _settings.ChunkRadius][y + _settings.ChunkRadius - Math.Max(0, -x)];
        }
        set
        {
            _hexCells[x + _settings.ChunkRadius][y + _settings.ChunkRadius - Math.Max(0, -x)] = value;
        }
    }
    public HexChunkData(HexMainSettings settings)
    {
        _settings = settings;
        _hexCells = new HexCell[_settings.ChunkDiametr][];
        for (int x = 0; x < _settings.ChunkDiametr; x++)
        {
            _hexCells[x] = new HexCell[_settings.ChunkDiametr - Math.Abs(_settings.ChunkRadius - x)];
        }
    }
}
