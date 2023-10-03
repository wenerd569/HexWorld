using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CellConfig", menuName = "Hex/Cell/Types")]
public class CellTypes : SerializedScriptableObject
{
    [OdinSerialize] private Dictionary<CellType, Cell> cellTypes = new Dictionary<CellType, Cell>();
    private NativeArray<Cell> cellTypesNative;


    public NativeArray<Cell> NativeArray
    {
        get { return cellTypesNative; }
    }


    public Cell this[CellType type]
    {
        get
        {
            return cellTypes[type];
        }
    }

    public void Init()
    {
        cellTypesNative = new NativeArray<Cell>((int)CellType.Last, Allocator.Persistent);

        for (CellType i = CellType.Last; i >= 0; i--)
        {
            if (cellTypes.TryGetValue(i, out Cell cell))
            {
                cellTypesNative[(int)i] = cell;
            }
        }
    }



    public void ChangeCellSettings(CellType type, Cell newCellCettings)
    {
        cellTypes[type] = newCellCettings;
    }
}