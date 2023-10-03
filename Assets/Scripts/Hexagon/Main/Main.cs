using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Dictionary<Vector2Int, Chunk> Chunks;
    public Dictionary<Vector2Int, Chunk> LoadedChunks;
    protected WorldGenerator worldGenerator;

    public void Init(WorldGenerator worldGenerator)
    {
        Chunks = new Dictionary<Vector2Int, Chunk>();
        LoadedChunks = new Dictionary<Vector2Int, Chunk>();
        this.worldGenerator = worldGenerator;
    }

    public void GenerateStartPlane()
    {
        var loadRadius = MainSettings.WorldLoadRadius;

        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                GenerateOrLoadChunk(new Vector2Int(x, y));
            }
        }
    }

    public void Update()
    {
        foreach (var chunk in LoadedChunks)
        {
            chunk.Value.ForcesUpdate();
        }
        foreach (var chunk in LoadedChunks)
        {
            chunk.Value.HeightUpdate();
        }
    }




    private void GenerateOrLoadChunk(Vector2Int chunkPosition)
    {
        Chunk chunk;
        if (Chunks.TryGetValue(chunkPosition, out chunk))
        {
            worldGenerator.LoadChunk(chunk);
            LoadedChunks.Add(chunkPosition, chunk);
        }
        else
        {
            chunk = worldGenerator.GenerateChunk(chunkPosition, CellSettings.CellPrefabs[CellType.HightSpringRock]);
            Chunks.Add(chunkPosition, chunk);
            LoadedChunks.Add(chunkPosition, chunk);
        }
    }
}
