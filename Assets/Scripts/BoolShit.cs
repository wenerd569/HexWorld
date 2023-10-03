using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class BoolShit : MonoBehaviour
{
    private Dictionary<Vector2Int, GameObject> cells = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] GameObject cellPrefab;


    public void Run()
    {
        for (int x = -ChunkSettings.ChunkSize; x <= ChunkSettings.ChunkSize; x++)
        {
            for (int y = -ChunkSettings.ChunkSize; y <= ChunkSettings.ChunkSize; y++)
            {
                var position = new HexPosition(new Vector2Int(x, y));
                var cell = Instantiate(cellPrefab, position.WorldCoordinate, Quaternion.identity);

                cells.Add(position.CellInWorld, cell);
            }
        }
    }


    void Update()
    {
        foreach (var cell in cells)
        {
            var hex = cell.Value;
            var transform = new Vector3(0, Random.value - 0.5f, 0);
            hex.transform.Translate(transform);
        }
    }
}
