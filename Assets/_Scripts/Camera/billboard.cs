using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class billboard : OptimizedBehaviour
{
    private Camera _cam;
    private OptimizedBehaviour _optimizedBehaviour;
    private Transform _camT;
    private void Awake()
    {
        _cam = Helpers.Camera;
        _optimizedBehaviour = _cam.GetComponent<OptimizedBehaviour>();
        _camT = _optimizedBehaviour.CachedTransform;

    }
    private void Update()
    {
        CachedTransform.LookAt(_camT);
        CachedTransform.rotation = Quaternion.Euler(CachedTransform.rotation.x, CachedTransform.rotation.y + 180, CachedTransform.rotation.z); ;
    }
}
