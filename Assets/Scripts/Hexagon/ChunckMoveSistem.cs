using System.Numerics;
using System;
using System.Drawing;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class ChunkMoveSistem : ScriptableObject
{
    private struct TranslationJob : IJobParallelForTransform
    {
        public int Size;
        public ComputeBuffer[,] HeightsBuffers;
        public NativeArray<HexCell.Hex> Hexes;
        public NativeArray<float> Heights;
        [WriteOnly] public NativeArray<float> Forces;
        public NativeArray<float> Speeds;

        public void Execute(int fid, TransformAccess transform)
        {
            var x = fid / Size;
            var y = fid % Size;
            var hId = fid + Size + 3 + x * 2;

            if ()


            Speeds[hId] += Forces[fid] / Hexes[fid].Mass

            transform.position += new Vector3(0, Heights[hId], 0);

            SetHeight(0, 0, x, y);
            if (x == 0)
            {
                SetHeight(-1, 0, x, y);
            }
            else if (x == Size)
            {
                SetHeight(1, 0, x, y);
            }
            if (y == 0)
            {
                SetHeight(0, -1, x, y);
            }
            else if (y == Size)
            {
                SetHeight(0, 1, x, y);
            }
        }

        private void SetHeight(int chX, int chY, int x, int y)
        {
            var myHId = (x * Size + y) + Size + 3 + x * 2;
            x += -chX * Size;
            y += -chY * Size;
            var hId = (x * Size + y) + Size + 3 + x * 2;
            HeightsBuffers[chX, chY].BeginWrite<float>(hId, 1);
            HeightsBuffers[chX, chY].SetData(Heights, myHId, hId, 1);
        }
    }

    public ChunkData HexChunkData
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
    private ChunkData _hexChunkData;
    private MainSettings _settings = MainSettings.Instance;
    private int size = MainSettings.Instance.ChunkSize;

    [SerializeField] private ComputeShader _phisicShaider;
    private TranslationJob _hexTranslationJob;
 
    public ComputeBuffer HexesBuffer;
    public NativeArray<HexCell.Hex>  Hexes;

    public ComputeBuffer HeightsBuffer;
    public NativeArray<float> Heights;

    public ComputeBuffer ForcesBuffer;
    public NativeArray<float> Forces;
    public NativeArray<float> Speeds;

    private int _calcForcesKernel;
    public Action<AsyncGPUReadbackRequest> ForcesUptdateDone;

    private int _xGroupThreads;
    private int _yGroupThreads;

    private IChunk[,] _neighbourHexChunk = new IChunk[3, 3];
    private ComputeBuffer[,] Buffers = new ComputeBuffer[3, 3];

    public ChunkMoveSistem()
    {
        InitializeShaiderBuffers();
    }


    public void InitializeShaiderBuffers()
    {
        HexesBuffer = new ComputeBuffer(size * size, sizeof(float) * 4);
        HeightsBuffer = new ComputeBuffer((size + 2) * (size + 2), sizeof(float));
        ForcesBuffer = new ComputeBuffer(size * size, sizeof(float));


        Heights = new NativeArray<float>((size + 2) * (size * 2), Allocator.Persistent);

        Hexes = new NativeArray<HexCell.Hex>(size * size, Allocator.Persistent);
        Forces = new NativeArray<float>(size * size, Allocator.Persistent);
        Speeds = new NativeArray<float>(size * size, Allocator.Persistent);

        // ������ �� ��������
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
                    FrictionСoefficient = hex.PhisicMaterial.FrictionCoefficient,
                };
                Heights[id] = hex.transform.position.y;
            }
        }

        HexesBuffer.SetData(Hexes);
        HeightsBuffer.SetData(Heights);
        ForcesBuffer.SetData(Forces);

        _phisicShaider.GetKernelThreadGroupSizes(_calcForcesKernel, out uint xThreads, out uint yThreads, out _);
        _xGroupThreads = _settings.ChunkSize / (int)xThreads;
        _yGroupThreads = _settings.ChunkSize / (int)yThreads;

        _calcForcesKernel = _phisicShaider.FindKernel("CalcForces");

        _phisicShaider.SetBuffer(_calcForcesKernel, "Forces", ForcesBuffer);
        _phisicShaider.SetBuffer(_calcForcesKernel, "Heights", HexesBuffer);
        _phisicShaider.SetBuffer(_calcForcesKernel, "Hexes", HexesBuffer);

        _hexTranslationJob = new TranslationJob()
        {
            Size = size,
            HeightsBuffer = HeightsBuffer,
            Heights = Heights,
            Forces = Forces,
            Speeds = Speeds,
        };

        ForcesUptdateDone += CalcHeights;
    }

    public void DeInitializeShaiderBuffers()
    {
        HexesBuffer.Dispose();
        HeightsBuffer.Dispose();
        ForcesBuffer.Dispose();
        Heights.Dispose();
        Forces.Dispose();
        Speeds.Dispose();
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

//   public void PrepairCalcForces()
//   {
//       GetNeighbourHeightForHorizontalLine(-1);
//       GetNeighbourHeightForHorizontalLine(size);
//       GetNeighbourHeightForVerticalLine(-1);
//       GetNeighbourHeightForVerticalLine(size);
//   }

    public void CalcForces()
    {
        _phisicShaider.Dispatch(_calcForcesKernel, _xGroupThreads, _yGroupThreads, 1);
        AsyncGPUReadback.Request(ForcesBuffer, ForcesUptdateDone);
    }

    public void CalcHeights(AsyncGPUReadbackRequest callback)
    {
        Forces = callback.GetData<float>();
        JobHandle jobHandle = _hexTranslationJob.Schedule();
    }

    private HexCell GetMyOrNeighbourHex(int x, int y)
    {
        x += size;
        y += size;

        var chunkX = x / size;
        var chunkY = y / size;
        var cellX = x % size;
        var cellY = y % size;


        var chunk = _neighbourHexChunk[chunkX, chunkY];

        return HexChunkData[cellX, cellY];
    }

    private void SearchNeighbourChunk()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Main.Instance.Chunks.TryGetValue((ChunkPosition + new Vector2Int(i, j)), out var hex))
                {
                    _neighbourHexChunk[i+1, j+1] = hex;
                }
                else
                {
                    var warning = "�� ������ ����: " + (ChunkPosition + new Vector2Int(i, j)).ToString();
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
            throw new Exception("����������� ������������� �������");
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

    public void OnDestroy()
    {
        DeInitializeShaiderBuffers();
    }
}
