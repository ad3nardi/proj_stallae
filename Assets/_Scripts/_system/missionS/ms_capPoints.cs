using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ms_capPoints : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private TagSet _tagSet;
    [SerializeField] private LayerSet _layerSet;
    [Header("Settings")]
    [SerializeField] private bool _taken;
    [SerializeField] private float _size;
    [SerializeField] private float _progress;
    [SerializeField] private float _maxTimeToTake;
    [SerializeField] private float _timer;
    [SerializeField] private float _countBack;
    [SerializeField] private float _countBackRate;
    [SerializeField] private float _countBacktimer;
    [SerializeField] private LayerMask _layerMask;
    
    private void Start()
    {
        _tagSet = Helpers.TagSet;
        _layerSet = Helpers.LayerSet;

        _timer = 0;
    }

    private void Update()
    {
        UpdateCheckCollision();
    }

    private void UpdateCheckCollision()
    {
        if (!_taken)
        {
            bool playerPresent = Physics.OverlapSphere(CachedTransform.position, _size, _layerMask).Length > 0;
            if (playerPresent)
            {
                UpdateTimers();
            }
            if (playerPresent)
            {
                UpdateCountBack();
            }
        }
    }

    private void UpdateTimers()
    {
        _timer += Time.deltaTime;
    }

    private void UpdateCountBack()
    {
        if(_countBack >= _countBacktimer)
        {
            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
                return;
        }
        if(_countBack < _countBacktimer)
        {
            _countBack += Time.deltaTime * _countBackRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(CachedTransform.position, _size);
    }
}
