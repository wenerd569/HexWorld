using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
    public HexChunkData HexChunkData
    {
        get
        {
            return _hexChunkData;
        }
        set
        {
            _hexChunkData = value;
            if (IsSimulate)
            {
                DeInitializeShaiderBuffers();
                InitializeShaiderBuffers();
            }
        }
    }
    public bool IsSimulate
    {
        get
        {
            return _isSimulate;
        }
        set
        {
            if (_isSimulate != value)
            {
                _isSimulate = value;
                if (_isSimulate)
                {
                    SimulateOn();
                }
                else
                {
                    SimulateOff();
                }
            }
        }
    }
    public Vector2Int ChunkPosition { get; private set; }

    private bool _isSimulate;
    private HexChunkData _hexChunkData;
    private HexMainSettings _settings;
    private int size;
    [SerializeField] private HexCell _standartCell;
    [SerializeField] private HexPhisicMaterial _standartPhisicMaterial;
    [SerializeField] private ComputeShader _phisicShaider;
 
    public ComputeBuffer HexesBuffer;
    public HexCell.Hex[] Hexes;
    public ComputeBuffer HeightsBuffer;
    public NativeArray<float> Heights;
    public ComputeBuffer ForcesBuffer;
    public float[] Forces;
    public ComputeBuffer SpeedsBuffer;
    public float[] Speeds;

    private int _calcForcesKernel;
    private int _calcHeightsKernel;

    private int _xGroupThreads;
    private int _yGroupThreads;

    private HexChunk[,] _neighbourHexChunk = new HexChunk[3,3];

    public void Start()
    {
        _settings = HexMainSettings.Instance;
        _hexChunkData = new HexChunkData(_settings.ChunkSize);
        ChunkPosition = new HexPosition(transform.position).Chunk;
        GenerateStandartPlane();
    }

    public void InitializeShaiderBuffers()
    {
        HexesBuffer = new ComputeBuffer((size + 2) * (size + 2), sizeof(float) * 4);
        HeightsBuffer = new ComputeBuffer((size + 2) * (size + 2), sizeof(float));
        ForcesBuffer = new ComputeBuffer(size * size, sizeof(float));
        SpeedsBuffer = new ComputeBuffer(size * size, sizeof(float));

        Hexes = new HexCell.Hex[(size + 2) * (size * 2)];
        Heights = new NativeArray<float>((size + 2) * (size * 2), Allocator.Persistent);
        Forces = new float[size * size];
        Speeds = new float[size * size];

        // çàïèñü ïî ñòîëáöàì
        for (var i = -1; i < size; i++)
        {
            for (var j = -1; j < size; j++)
            {
                var id = i * size + j;
                var hex = GetMyOrNeighbourHex(i, j);
                Hexes[id] = new HexCell.Hex
                {
                    StartHeight = hex.StartHeight,
                    SpringRate = hex.PhisicMaterial.SpringRate,
                    Mass = hex.PhisicMaterial.Mass,
                    FrictionÑoefficient = hex.PhisicMaterial.FrictionÑoefficient
                };

                Heights[id] = hex.transform.position.y;

                // ñêîðîñòè ïî íóëÿì
            }
        }

        HexesBuffer.SetData(Hexes);
        HeightsBuffer.SetData(Heights);
        ForcesBuffer.SetData(Forces);
        SpeedsBuffer.SetData(Speeds);

        _phisicShaider.GetKernelThreadGroupSizes(_calcForcesKernel, out uint xThreads, out uint yThreads, out _);
        _xGroupThreads = _settings.ChunkSize / (int)xThreads;
        _yGroupThreads = _settings.ChunkSize / (int)yThreads;

        _calcForcesKernel = _phisicShaider.FindKernel("CalcForces");

        _phisicShaider.SetBuffer(_calcForcesKernel, "Forces", ForcesBuffer);
        _phisicShaider.SetBuffer(_calcForcesKernel, "Heights", HexesBuffer);
        _phisicShaider.SetBuffer(_calcForcesKernel, "Hexes", HexesBuffer);

        _calcHeightsKernel = _phisicShaider.FindKernel("CalcHeights");

        _phisicShaider.SetBuffer(_calcHeightsKernel, "Forces", ForcesBuffer);
        _phisicShaider.SetBuffer(_calcHeightsKernel, "Heights", HexesBuffer);
        _phisicShaider.SetBuffer(_calcHeightsKernel, "Hexes", HexesBuffer);
        _phisicShaider.SetBuffer(_calcHeightsKernel, "Speeds", SpeedsBuffer);
    }

    public void DeInitializeShaiderBuffers()
    {
        HexesBuffer.Dispose();
        HeightsBuffer.Dispose();
        ForcesBuffer.Dispose();
        SpeedsBuffer.Dispose();
        Heights.Dispose();
    }

    private void SimulateOn()
    {
        SearchNeighbourChunk();
        InitializeShaiderBuffers();
    }

    private void SimulateOff()
    {
        DeInitializeShaiderBuffers();
    }




    public void PrepairCalcForces()
    {
        GetNeighbourHeightForHorizontalLine(-1);
        GetNeighbourHeightForHorizontalLine(size);
        GetNeighbourHeightForVerticalLine(-1);
        GetNeighbourHeightForVerticalLine(size);
    }

    public ComputeShader CalcForces()
    {
        HeightsBuffer.SetData(Heights);
        _phisicShaider.Dispatch(_calcForcesKernel, _xGroupThreads, _yGroupThreads, 1);
        return _phisicShaider;
    }

    public ComputeShader CalcHeights()
    {
        _phisicShaider.Dispatch(_calcHeightsKernel, _xGroupThreads, _yGroupThreads, 1);
        return _phisicShaider;
    }



    private HexCell GetMyOrNeighbourHex(int x, int y)
    {
        x += size;
        y += size;

        var chunkX = x / size;
        var chunkY = y / size;
        var cellX = x % size;
        var cellY = y % size;

        return _neighbourHexChunk[chunkX, chunkY].HexChunkData[cellX, cellY];
    }

    private void SearchNeighbourChunk()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (HexMain.Instance.Chunks.TryGetValue((ChunkPosition + new Vector2Int(i, j)), out var hex))
                {
                    _neighbourHexChunk[i+1, j+1] = hex;
                }
                else
                {
                    var warning = "Íå íàéäåí ÷àíê: " + (ChunkPosition + new Vector2Int(i, j)).ToString();
                    throw new Exception(warning);
                }
            }
        }
    }

    private void GetNeighbourHeightForHorizontalLine(int x)
    {
        for (int y = -1; y <= size;  y++)
        {
            var id = GetId(x, y);
            Heights[id.My] = _neighbourHexChunk[id.xChunkOffset, id.yChunkOffset].Heights[id.Neighbour];
        }
    }

    private void GetNeighbourHeightForVerticalLine(int y)
    {
        var start = GetId(0, y);
        var end = GetId(size - 1, y);
        if (start.xChunkOffset != end.xChunkOffset || start.yChunkOffset != end.yChunkOffset)
        {
            throw new Exception("Íåïðàâèëüíî èñïîëüçîâàííà ôóíêöèÿ");
        }
        var lenght = start.Neighbour - end.Neighbour;

        Array.Copy(_neighbourHexChunk[start.xChunkOffset, start.yChunkOffset].Heights,
            start.Neighbour,
            Heights,
            start.My,
            lenght
            );
    }

    private (int My, int Neighbour, int xChunkOffset, int yChunkOffset) GetId(int x, int y)
    {
        (int My, int Neighbour, int xChunkOffset, int yChunkOffset) id;
        id.My = x * (size + 2) + size + 3 + y;

        x += size;
        y += size;

        var chunkX = x / size;
        var chunkY = y / size;
        var cellX = x % size;
        var cellY = y % size;

        id.xChunkOffset = chunkX;
        id.yChunkOffset = chunkY;

        id.Neighbour = cellX * (size + 2) + size + 3 + cellY;
        return id;
    }




    private void GenerateStandartPlane()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y <= size; y++)
            {
                var cell = new HexCell(transform.position.y, _standartPhisicMaterial);
                AddHexCellToRender(x, y, cell);
            }
        }
    }

    public void AddHexCellToRender(int x, int y, HexCell cell)
    {
        var clellOfset = _settings.HexBasis.x * x + _settings.HexBasis.y * y;
        var currentHexCell = Instantiate(
                cell,
                new Vector3(clellOfset.x, 0, clellOfset.y) + transform.position,
                _settings.UnityStupidRotationFix
                );

        currentHexCell.transform.parent = this.transform;
        currentHexCell.name = x.ToString() + y.ToString();
        _hexChunkData[x, y] = currentHexCell;
    }

    public void OnDestroy()
    {
        DeInitializeShaiderBuffers();
    }
}
