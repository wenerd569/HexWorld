using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellSettings", menuName = "Hex/Cell/CellSettings")]
public class CellSettings : ScriptableObject
{
    private static CellSettings instance;
    [SerializeField] public CellTypes cellTypes;
    [SerializeField] public CellPrefabs cellPrefabs;
    [SerializeField] private int hexSize;


    public static CellTypes CellTypes
    {
        get { return instance.cellTypes; }
    }
    public static CellPrefabs CellPrefabs
    {
        get { return instance.cellPrefabs; }
    }
    public static int HexSize
    {
        get { return instance.hexSize; }
    }

    public static Transform2D HexBasis { get; private set; }
    public static Transform2D InvertHexBasis { get; private set; }


    public void Init()
    {
        instance = this;
        HexBasis = Transform2D.CalculateHexBasis(HexSize);
        InvertHexBasis = Transform2D.InvertBasis(HexBasis);
    }
}
