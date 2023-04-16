using UnityEngine;
using System.Collections.Generic;
using System;

public class HexMain : MonoBehaviour
{

    public Dictionary<Vector2Int, HexChunk> _chunks = new Dictionary<Vector2Int, HexChunk>();

    private Camera MainCamera;
    [SerializeField] public HexChunk _standartChunk;

    [SerializeField] private LayerMask _hexLayer;
    private HexCell _seleleciktCell;

    public void Awake()
    {
        MainCamera = Camera.main;
    }
    public void Start()
    {
        GenerateStandartMain(HexMainSettings.Instance.WorldRadius);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _hexLayer))
            {
                var hitCoordinate = hit.point;
                var calculateCellPosition = new HexPosition();
                calculateCellPosition.WorldCoordinate = hitCoordinate;
                var _selectCell = hit.collider.gameObject.GetComponent<HexCell>();
                print(hitCoordinate);
                print(calculateCellPosition.Chunk.ToString() + " : " + calculateCellPosition.CellInChunk.ToString());
                print(_selectCell.PositionInChunk);
            }
        }
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
                    var chunkOfset = x * HexMainSettings.Instance.ChunkBasis.x + y * HexMainSettings.Instance.ChunkBasis.y;
                    var chunk = Instantiate(_standartChunk, new Vector3(chunkOfset.x, 0, chunkOfset.y), Quaternion.identity);
                    chunk.transform.parent = this.transform;
                    chunk.name = locate.ToString();
                    _chunks.Add(locate, chunk);
                }
            }
        }
    }
}