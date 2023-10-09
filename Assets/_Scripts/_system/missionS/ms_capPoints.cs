using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ms_capPoints : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private TagSet _tagSet;
    [SerializeField] private LayerSet _layerSet;

    [SerializeField] private float _fill;
    [SerializeField] private Image _img;
    [Header("Settings")]
    [SerializeField] public bool _taken;
    [SerializeField] private float _size;
    [SerializeField] private float _progress;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _timer;
    [SerializeField] private float _countBack;
    [SerializeField] private float _countBackRate;
    [SerializeField] private float _countBacktimer;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private OptimizedBehaviour _valeria;

    public event Action<bool> OnCapturePointTaken = delegate { };


    /*
    public static event Action<ms_capPoints> OnPctAdded = delegate { };
    public static event Action<ms_capPoints> OnPctRemoved = delegate { };
    public event Action<float> OnProgressPctChanged = delegate { };
    public event Action<float> OnProgressChanged = delegate { };
    public event Action<bool> OnCapturePointTaken = delegate { };
    */
    private void Start()
    {

        _tagSet = Helpers.TagSet;
        _layerSet = Helpers.LayerSet;

        _img.fillAmount = 0f;
        _timer = 0f;
        ResetTimer();
    }

    

    private void Update()
    {
        UpdateCheckCollision();
    }
    public void ResetTimer()
    {
        _timer = 0;
    }
    private void UpdateCheckCollision()
    {
        if (!_taken)
        {

                if (Vector3.Distance(_valeria.CachedTransform.position, CachedTransform.position) <= _size)
                {
                    ModifyProgress();
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
        _fill = _timer / _maxTime;
        _img.fillAmount= _fill;


        if (_fill < 1)
        {
            _taken = (false);
            OnCapturePointTaken(_taken);
        }
        else if (_fill >= 1)
        {
            _taken = (true);
            OnCapturePointTaken(_taken);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(CachedTransform.position, _size);
    }
}
