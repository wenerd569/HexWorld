using System;

public class HexChunkData
{
    public readonly int _chunkRadius;
    public readonly int _chunkDiameter;

    private HexCell[][] _hexCells;

    public HexCell this[int x, int y]
    {
        get
        {
            return _hexCells[x + _chunkRadius][y + _chunkRadius - Math.Max(0, -x)];
        }
        set
        {
            _hexCells[x + _chunkRadius][y + _chunkRadius - Math.Max(0, -x)] = value;
        }
    }
    public HexChunkData(int chunkRadius)
    {
        _chunkRadius = chunkRadius;
        _chunkDiameter = _chunkRadius * 2 + 1;
        _hexCells = new HexCell[_chunkDiameter][];
        for (int x = 0; x < _chunkDiameter; x++)
        {
            _hexCells[x] = new HexCell[_chunkDiameter - Math.Abs(chunkRadius - x)];
        }
    }
}
