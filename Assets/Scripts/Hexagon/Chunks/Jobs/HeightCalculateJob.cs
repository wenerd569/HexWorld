using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct HeightCalculateJob : IJobParallelFor
{
    [ReadOnly]public NativeArray<CellType> Cells;
    [ReadOnly]public NativeArray<float> Forces;
    public NativeArray<float> Speed;
    public NativeArray<float> HeightDifference;

    public void Execute(int index)
    {
        var cellType = Cells[index];
        var cell = CellSettings.CellTypes[cellType];

        var acceleration = Forces[index] / cell.Mass;
        Speed[index] += acceleration;
        HeightDifference[index] += Speed[index];
    }
}