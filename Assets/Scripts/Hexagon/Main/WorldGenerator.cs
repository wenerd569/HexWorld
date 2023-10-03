using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WorldGenerator
{
    protected Main main;

    public WorldGenerator(Main main)
    {
        this.main = main;
    }

    public void LoadChunk(Chunk chunk)
    {
        //коооод
    }


    public Chunk GenerateChunk(Vector2Int chunkCoordinate, GameObject cellPrefab)
    {
        var size = ChunkSettings.ChunkSize;
        var startHeights = new NativeArray<float>(size * size, Allocator.Persistent);
        var gameObjects = new GameObject[size * size];
        var cells = new NativeArray<CellType>(size * size, Allocator.Persistent);
        var gameObject = GameObject.Instantiate(ChunkSettings.ChunkPrefab, HexPosition.ConvertToWorldCoordinate(chunkCoordinate), Quaternion.identity);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var index = x * size + y;
                var cellPosition = chunkCoordinate * size + new Vector2Int(x, y);


                startHeights[index] = Random.Range(0, 3);
                gameObjects[index] = MonoBehaviour.Instantiate(cellPrefab, HexPosition.ConvertToWorldCoordinate(cellPosition), Quaternion.identity);
                gameObjects[index].name = new Vector2Int(x, y).ToString();
                cells[index] = CellType.HightSpringRock;
            }
        }
        gameObject.transform.parent = main.transform;
        foreach (var cell in gameObjects)
        {
            cell.transform.parent = gameObject.transform;
        }


        var chunk = new Chunk(main, gameObject, chunkCoordinate, size, startHeights, cells, gameObjects);
        return chunk;
    }
}
