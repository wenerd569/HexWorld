using UnityEngine;

public sealed class EntryPoint : MonoBehaviour
{
    [SerializeField] MainSettings mainSettings;
    [SerializeField] ChunkSettings chunkSettings;
    [SerializeField] CellSettings cellSettings;
    [SerializeField] Main main;

    void Awake()
    {
        mainSettings.Init(main);
        chunkSettings.Init();
        cellSettings.Init();
        cellSettings.cellTypes.Init();
        main.Init(new WorldGenerator(main));

        main.GenerateStartPlane();
    }
}
