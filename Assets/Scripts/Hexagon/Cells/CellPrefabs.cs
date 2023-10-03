using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellPrefabs", menuName = "Hex/Cell/Prefabs")]
public class CellPrefabs : SerializedScriptableObject
{
    [OdinSerialize] private Dictionary<CellType, GameObject> cellPrefabs;

    public GameObject this[CellType type]
    {
        get
        {
            return cellPrefabs[type];
        }
    }
}
