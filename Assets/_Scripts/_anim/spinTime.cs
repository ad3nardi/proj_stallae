using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class spinTime : OptimizedBehaviour
{
    [SerializeField] private float _spd;
    [SerializeField] private Vector3 _rotationVec;

    private void Update()
    {
        CachedTransform.Rotate(_rotationVec, _spd *Time.deltaTime);
    }

}
