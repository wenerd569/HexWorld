using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkSettings", menuName = "Hex/Chunk/ChunkSettings")]
public sealed class ChunkSettings : ScriptableObject
{
    private static ChunkSettings instance;
    [SerializeField] private float minimumMovementThreshold;
    [SerializeField] private int chunckSize;
    [SerializeField] private GameObject chunkPrefab;


    public static float MinimumMovementThreshold 
    {
        get { return instance.minimumMovementThreshold; }
    }
    public static int ChunkSize
    {
        get { return instance.chunckSize; }
    }
    public static GameObject ChunkPrefab
    {
        get { return instance.chunkPrefab; }
    }

    public void Init()
    {
        instance = this;
    }
}
