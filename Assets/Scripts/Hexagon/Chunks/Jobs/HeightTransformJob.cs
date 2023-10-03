using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct HeightTransformJob : IJobParallelForTransform
{
    [ReadOnly] public NativeArray<float> Height;
    public float MinimumMovementThreshold;

    public void Execute(int index, TransformAccess transform)
    {
        var currPosition = transform.position;
        var height = Height[index];
        if (Mathf.Abs(height - currPosition.y) > MinimumMovementThreshold)
        {
            transform.position = new Vector3(currPosition.x, height, currPosition.z);
        }
    }
}
