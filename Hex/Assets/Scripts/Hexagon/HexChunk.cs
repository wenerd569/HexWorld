using System;
using Unity.VisualScripting;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
    public HexChunkData HexChunkData;
    private Transform _chunkTransform;
    private HexMainSettings _settings;
    [SerializeField] private HexCell _cell;

    public void Start()
    {
        _settings = HexMainSettings.Instance;
        _chunkTransform = transform;
        HexChunkData = new HexChunkData(_settings);
        GenerateStandartPlane();
    }

    private void GenerateStandartPlane()
    {
        for (int x = -_settings.ChunkRadius; x <= _settings.ChunkRadius; x++)
        {
            for (int y = -_settings.ChunkRadius; y <= _settings.ChunkRadius; y++)
           {
                if (Math.Abs(x + y) <= _settings.ChunkRadius)
                {
                    AddHexCellToRender(x, y);
                }
            }
        }
    }

    public void AddHexCellToRender(int x, int y)
    {
        var clellOfset = _settings.HexBasis.x * x + _settings.HexBasis.y * y;
        var currentHexCell = Instantiate(_cell, new Vector3(clellOfset.x, 0, clellOfset.y) + _chunkTransform.position, _settings.UnityStupidRotationFix);
        currentHexCell.transform.parent = this.transform;
        currentHexCell.name = x.ToString() + y.ToString();
        currentHexCell.Height = _chunkTransform.position.y;
        currentHexCell.Position = clellOfset;
        HexChunkData[x, y] = currentHexCell;
    }
}
