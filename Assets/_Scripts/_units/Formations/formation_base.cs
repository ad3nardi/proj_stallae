using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class formation_base : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] protected float _noise = 0;
    [SerializeField] protected float Spread = 1;
    public abstract IEnumerable<Vector3> EvaluatePoints();

    public Vector3 GetNoise(Vector3 pos)
    {
        var noise = Mathf.PerlinNoise(pos.x * _noise, pos.z * _noise);
        return new Vector3(noise, noise, noise);
    }
}