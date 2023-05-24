using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ApplyHeight : IJobParallelForTransform
{
    NativeArray<float> Heights;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position += new Vector3(0, Heights[index], 0);
    }
}
