using UnityEngine;

public interface IWorldGenerator
{
    public void TryLoadChunk(Vector2Int chunkPosition);
    public Chunk GenerateChunk(Vector2Int chunkPosition);
}
