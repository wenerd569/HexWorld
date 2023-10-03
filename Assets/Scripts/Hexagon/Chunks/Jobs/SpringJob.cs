using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct SpringJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Cell> Cell—haracteristic;
    [ReadOnly] public NativeArray<CellType> Cells;
    [ReadOnly] public NativeArray<NativeArray<float>> NeighbourChunkHeightDifference;
    public NativeArray<float> Forces;
    public int ChunkSize;

    public void Execute(int index)
    {
        var cellType = Cells[index];
        var cell = Cell—haracteristic[(int)cellType];
        if (cell.IgnorePhisic)
        {
            return;
        }
        var cellHeight = NeighbourChunkHeightDifference[4][index];

        var x = index / ChunkSize;
        var y = index % ChunkSize;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (Mathf.Abs(dx + dy) < 2)
                {
                    var curentX = x + dx + ChunkSize;
                    var curentY = y + dy + ChunkSize;
                    if (TryGetHeightInPosition(curentX, curentY, out float neighburCellHeight))
                    {
                        Forces[index] += (cellHeight - neighburCellHeight) * cell.SpringRate;
                    }
                }
            }
        }
    }
    private bool TryGetHeightInPosition(int x, int y, out float result)
    {
        var chunk = NeighbourChunkHeightDifference[x / ChunkSize * 3 + y / ChunkSize];
        if (chunk.Length == 0)
        {
            result = 0;
            return false;
        }
        result = chunk[x % ChunkSize * ChunkSize + y % ChunkSize];
        return true;
    }
}
