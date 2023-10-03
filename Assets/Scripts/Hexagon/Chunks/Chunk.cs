using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;
using Unity.VisualScripting;
using System;
using UnityEditor;

public class Chunk
{
    protected Main main;
    protected GameObject myGameObject;
    protected Vector2Int position;
    protected int size;
    protected NativeArray<NativeArray<float>> neighbourChunkHeightDifference;
    protected Chunk[] neighbourChunk;


    protected HeightCalculateJob heightCalculateJob;
    protected HeightTransformJob heightTransformJob;
    protected SpringJob springJob;


    protected GameObject[] cellGameObjects;
    protected Transform[] cellTransforms;
    protected TransformAccessArray transformAccessArray;


    protected NativeArray<CellType> cells;
    public NativeArray<ForcesComponent> forces;
    protected NativeArray<float> heightDiferences;
    protected NativeArray<float> startHeights;


    public void AddChunkInNeighbour(Vector2Int nieghbourPosition, Chunk chunk)
    {
        var relativePosition = nieghbourPosition - position; //.. íó ÿ äóìàþ ïîíÿòíî
        var index = (relativePosition.x + 1) * 3 + relativePosition.y + 1;

        neighbourChunk[index] = chunk;
        neighbourChunkHeightDifference[index] = chunk.heightDiferences;
    }

    private void CallNieghbourChunk()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(Math.Abs(x + y) < 2)
                {
                    var nieghbourChunkPosition = position + new Vector2Int(x, y);
                    if (main.Chunks.TryGetValue(nieghbourChunkPosition, out var nieghbourChunk))
                    {
                        nieghbourChunk.AddChunkInNeighbour(position, this);
                        AddChunkInNeighbour(nieghbourChunkPosition, nieghbourChunk);
                    }
                }
            }
        }
    }


    public Chunk(Main main, GameObject myGameObject, Vector2Int position, int size, NativeArray<float> startHeights, NativeArray<CellType> cells, GameObject[] cellGameObjects)
    {
        this.main = main;
        this.myGameObject = myGameObject;
        this.position = position;
        this.size = size;
        this.startHeights = startHeights;
        this.cellGameObjects = cellGameObjects;
        this.cells = cells;

        neighbourChunk = new Chunk[9];
        neighbourChunkHeightDifference = new NativeArray<NativeArray<float>>(9, Allocator.Persistent);
        heightDiferences = new NativeArray<float>(size * size, Allocator.Persistent);
        forces = new NativeArray<float>(size * size, Allocator.Persistent);
        speeds = new NativeArray<float>(size * size, Allocator.Persistent);

        cellTransforms = new Transform[size * size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var id = x * size + y;

                cellTransforms[id] = cellGameObjects[id].transform;
            }
        }
        transformAccessArray = new TransformAccessArray(cellTransforms);

        heightCalculateJob = new HeightCalculateJob()
        {
            Cells = cells,
            Forces = forces,
            Speed = speeds,
            HeightDifference = heightDiferences,
        };

        heightTransformJob = new HeightTransformJob()
        {
            Height = heightDiferences,
            MinimumMovementThreshold = ChunkSettings.MinimumMovementThreshold,
        };

        springJob = new SpringJob()
        {
            Cells = cells,
            CellÑharacteristic = CellSettings.CellTypes.NativeArray,
            NeighbourChunkHeightDifference = neighbourChunkHeightDifference,
            Forces = forces,
            ChunkSize = size,
        };

        CallNieghbourChunk();
    }


    public virtual JobHandle HeightUpdate()
    {
        var heightCalculateJobHandle = heightCalculateJob.Schedule(size * size, size);
        var heightTransformJobHandle = heightTransformJob.Schedule(transformAccessArray, heightCalculateJobHandle);
        return heightTransformJobHandle;
    }

    public virtual JobHandle ForcesUpdate()
    {
        var sptingJobHandle = springJob.Schedule(size * size, size);
        return sptingJobHandle;
    }
}