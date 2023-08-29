using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ms_capPoints : missionStructure
{
    [Header("Plugins")]
    [SerializeField] private TagSet _tagSet;
    [SerializeField] private LayerSet _layerSet;
    [Header("Settings")]
    [SerializeField] private bool _taken;
    [SerializeField] private float _size;
    [SerializeField] private float _progress;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _timer;
    [SerializeField] private float _countBack;
    [SerializeField] private float _countBackRate;
    [SerializeField] private float _countBacktimer;
    [SerializeField] private LayerMask _layerMask;

    public static event Action<ms_capPoints> OnPctAdded = delegate { };
    public static event Action<ms_capPoints> OnPctRemoved = delegate { };
    public event Action<float> OnProgressPctChanged = delegate { };
    public event Action<float> OnProgressChanged = delegate { };
    public event Action<bool> OnCapturePointTaken = delegate { };

    private void Start()
    {
        _tagSet = Helpers.TagSet;
        _layerSet = Helpers.LayerSet;

        _timer = 0;
        ResetTimer();
    }

    

    private void Update()
    {
        UpdateCheckCollision();
    }
    public void ResetTimer()
    {
        _timer = 0;
        OnPctAdded(this);
    }
    private void UpdateCheckCollision()
    {
        if (!_taken)
        {
            bool playerPresent = Physics.OverlapSphere(CachedTransform.position, _size, _layerMask).Length > 0;
            if (playerPresent)
            {
                ModifyProgress();
            }
            if (!playerPresent)
            {
                UpdateCountBack();
            }
        }
    }

    private void UpdateCountBack()
    {
        if(_countBack >= _countBacktimer)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0)
                {
                    _timer = 0;
                }
            }
            else
                return;
        }
        if(_countBack < _countBacktimer)
        {
            _countBack += Time.deltaTime * _countBackRate;
        }
    }

    public void ModifyProgress()
    {
        _timer += Time.deltaTime;
        float curProgressPct = _timer / _maxTime;

        OnProgressPctChanged(curProgressPct);
        OnProgressChanged(_timer);
        if (_timer >= 0)
        {
            OnCapturePointTaken(false);
        }
        else
            OnCapturePointTaken(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(CachedTransform.position, _size);
    }
}
