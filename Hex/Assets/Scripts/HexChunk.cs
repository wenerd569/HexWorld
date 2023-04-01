using System;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
    private int _chunkRadius;
    public HexChunkData ChunkData;
    private Transform _chunkTransform;

    [SerializeField] private HexCell _cell;

    public void Start()
    {
        _chunkTransform = transform;
        _chunkRadius = HexMain.singleton.ChunkRadius;
        ChunkData = new HexChunkData(_chunkRadius);
        GenerateStandartPlane();
    }

    private void GenerateStandartPlane()
    {
        for (int x = -_chunkRadius; x <= _chunkRadius; x++)
        {
            for (int y = -_chunkRadius; y <= _chunkRadius; y++)
           {
                if (Math.Abs(x + y) <= _chunkRadius)
                {
                    var clellOfset = HexMain.singleton.HexBasis.x * x + HexMain.singleton.HexBasis.y * y;
                    ChunkData[x, y] = Instantiate(_cell, _chunkTransform.position + new Vector3(clellOfset.x, 0, clellOfset.y), HexRotation.Right);
                    ChunkData[x, y].ChunkPosition = new Vector2Int(x, y);
                }
            }
        }
    }
}
