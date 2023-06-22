using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MoveableChunk : MonoBehaviour, IMoveableChunk
{
    public ChunkData ChunkData
    {
        get
        {
            return _chunkData;
        }
        set
        {
            //допиши плиз
        }
    }
    [SerializeField] private ChunkData _chunkData;
    private MainSettings _settings;
    private int size;

    public ChunkMoveSistem ChunkMoveSistem
    {
        get
        {
            return _chunkMoveSistem;
        }
        set
        {

        }
    }
    [SerializeField] private ChunkMoveSistem _chunkMoveSistem;











    public void SpawnPlane()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y <= size; y++)
            {
                var cell = _chunkData[x, y];
                AddHexCellToRender(x, y, cell);
            }
        }
    }

    private void AddHexCellToRender(int x, int y, HexCell cell)
    {
        var clellOfset = _settings.HexBasis.x * x + _settings.HexBasis.y * y;
        var currentHexCell = Instantiate(
                cell,
                new Vector3(clellOfset.x, 0, clellOfset.y) + transform.position,
                _settings.UnityStupidRotationFix
                );

        currentHexCell.transform.parent = this.transform;
        currentHexCell.name = x.ToString() + y.ToString();
        _chunkData[x, y] = currentHexCell;
    }
}
