using UnityEngine;
using System.Collections.Generic;
using System;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public Dictionary<Vector2Int, MoveableChunk> Chunks = new Dictionary<Vector2Int, MoveableChunk>();
    public MonoBehaviour Player;

    private MainSettings _settings;


    public ChunkMoveSistem StandartChunk;

    public void Awake()
    {
        Instance = this;
        _settings = MainSettings.Instance;
    }
    public void Start()
    {
        GenerateStandartMain(_settings.WorldLoadRadius);
    }

    public void Update()
    {
        UpdateChunksLoadetArea();

        ChunkPhisicIteration();
    }

    private void UpdateChunksLoadetArea()
    {
        var simulateDistantion = _settings.ChunkSize * _settings.WorldSimulateRadius;
        var loadDistantion = _settings.ChunkSize * _settings.WorldLoadRadius;

        var chunkPosition = new HexPosition(transform.position);
        var playerPosition = new HexPosition(Player.transform.position);

        var deletedList = new List<Vector2Int>();
        foreach (var chunk in Chunks)
        {
            chunkPosition.Chunk = chunk.Key;
            var distantion = (playerPosition.CellInWorld - chunkPosition.CellInWorld).magnitude;
            if (distantion > loadDistantion)
            {
                deletedList.Add(chunk.Key);
            }
            else if (distantion > simulateDistantion)
            {
                chunk.Value.IsSimulate = false;
            }
            else
            {
                chunk.Value.IsSimulate = true;
            }
        }
    }

    private void ChunkPhisicIteration()
    {
        foreach (var chunk in Chunks)
        {
            if (chunk.Value.IsSimulate)
            {
                chunk.Value.PrepairCalcForces();
                chunk.Value.CalcForces();
            }
        }
        foreach (var chunk in Chunks)
        {
            if (chunk.Value.IsSimulate)
            {
                chunk.Value.CalcHeights();
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
                    var chunkOfset = (x * _settings.HexBasis.x * _settings.ChunkSize) + (y * _settings.HexBasis.y * _settings.ChunkSize);
                    var chunk = Instantiate(StandartChunk, new Vector3(chunkOfset.x, 0, chunkOfset.y), Quaternion.identity);
                    chunk.transform.parent = this.transform;
                    chunk.name = locate.ToString();
                    Chunks.Add(locate, chunk);
                }
            }
        }
    }
}