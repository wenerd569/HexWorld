using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HexMain : MonoBehaviour
{
    public static HexMain singleton { get; private set; }
    public Transform2D HexBasis { get; private set; }
    public Transform2D InvertGridBasis { get; private set; }
    public Transform2D ChunkBasis;
    public int HexSize = 1;
    [Range(0, 8)] public int ChunkRadius;
    [Range(0, 8)] public int WorldRadius;
    private Dictionary<Vector2Int, HexChunk> _chunks = new Dictionary<Vector2Int, HexChunk>();
    [SerializeField] private HexChunk _standartChunk;

    private Camera _mainCamera;

    private LayerMask _hexLayer;
    private HexCell _selelectCell;

    public void Awake()
    {
        singleton = this;
        HexBasis = MathHex.CalculateHexBasis(HexSize);
        InvertGridBasis = MathHex.InvertBasis(HexBasis);
        ChunkBasis = MathHex.CalculateChunkBasis(HexBasis, ChunkRadius);
        _mainCamera = Camera.main;
    }
    public void Start()
    {
        GenerateStandartMain(WorldRadius);
    }

    private void GenerateStandartMain(int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (Math.Abs(x + y) <= radius)
                {
                    var locate = new Vector2Int(x, y);
                    var chunkOfset = x * ChunkBasis.x + y * ChunkBasis.y;
                    var chunk = Instantiate<HexChunk>(_standartChunk, new Vector3(chunkOfset.x, 0, chunkOfset.y), Quaternion.identity);
                    _chunks.Add(locate, chunk);
                }
            }
        }
    }


    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _hexLayer))
            {
                var hitCoordinate = hit.point;
                var calculateCellPosition = MathHex.CoordinateToHexCell(hitCoordinate);
                var _selectCell = hit.collider.gameObject.GetComponent<HexCell>();
                var realCoordinate = _selectCell.ChunkPosition;
                print(hitCoordinate);
                print(realCoordinate);
                print(calculateCellPosition);
            }
        }
    }
}
