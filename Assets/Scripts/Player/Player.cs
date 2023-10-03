using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float maxDistance;
    [SerializeField] private float forces;


    public void Update()
    {
        if (Input.GetAxis("Fire") > 0)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, maxDistance, layerMask))
            {
                var hexPosition = new HexPosition(hitInfo.point);
                if (MainSettings.Main.Chunks.TryGetValue(hexPosition.Chunk, out var chunk))
                {
                    chunk.forces[hexPosition.ChunkIndex] += forces;
                }
            }
        }
    }
}
