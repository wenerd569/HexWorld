using UnityEngine;
using System.Collections.Generic;
using System;

public class HexMain : MonoBehaviour
{
    public static HexMain Instance;
    public Dictionary<Vector2Int, HexChunk> Chunks = new Dictionary<Vector2Int, HexChunk>();

    private Camera _mainCamera;
    public HexChunk StandartChunk;

    [SerializeField] private LayerMask _hexLayer;
    private HexCell _selelectionCell;

    public void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
    }
    public void Start()
    {
        GenerateStandartMain(HexMainSettings.Instance.WorldRadius);
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
                    var chunk = Instantiate(StandartChunk, new Vector3(chunkOfset.x, 0, chunkOfset.y), Quaternion.identity);
                    chunk.transform.parent = this.transform;
                    chunk.name = locate.ToString();
                    Chunks.Add(locate, chunk);
                }
            }
        }
    }
}