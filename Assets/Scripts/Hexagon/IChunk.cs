using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChunk
{
    public ChunkData ChunkData { get; set; }
    public void SpawnPlane();
}
