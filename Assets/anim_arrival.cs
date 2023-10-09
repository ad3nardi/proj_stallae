using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_arrival : OptimizedBehaviour
{
    [SerializeField] private OptimizedBehaviour _vis;
    [SerializeField] private float _animTime;
    [SerializeField] private Vector3 _stretchScale;

    private void OnEnable()
    {
        _vis.CachedTransform.localScale= _stretchScale;
        _vis.CachedTransform.DOScale(Vector3.one, _animTime);
    }
}
