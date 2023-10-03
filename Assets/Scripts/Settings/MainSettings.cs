using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MainSettings", menuName = "Hex/Main/MainSettings")]
public sealed class MainSettings : ScriptableObject
{
    private static MainSettings instance;
    [SerializeField] private int worldSimulateRadius;
    [SerializeField] private int worldLoadedRadius;
    [SerializeField] private Main main;


    public static int WorldSimulateRadius
    {
        get { return instance.worldSimulateRadius; }
    }
    public static int WorldLoadRadius
    {
        get { return instance.worldLoadedRadius; }
    }
    public static Main Main
    {
        get { return instance.main; }
    }


    public void Init(Main main)
    {
        instance = this;
        this.main = main;
    }
}
