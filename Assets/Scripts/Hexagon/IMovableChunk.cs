using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveableChunk : IChunk
{
    public ChunkMoveSistem ChunkMoveSistem { get; protected set; }
}
