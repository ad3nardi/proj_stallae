using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ss_logic_shield : OptimizedBehaviour
{
    [Header ("Plugins")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private unit_subsytems _unitSS;

    [Header("Shield Stats")]
    [SerializeField] private float _maxShieldHP;
    [SerializeField] private float _curShieldHP;
    [SerializeField] private float _sRegenRate;
    [SerializeField] private float _sDestroyedRegenRate;
    [SerializeField] private float _sRegenWaitTime;

    [Header("Shield Status")]
    [SerializeField] private bool _isRegenWaiting;

    [Header("Shield Internal")]
    [SerializeField] private float _sRegenWaitTimer;
    [SerializeField] private float _isDisableTimer;
    [SerializeField] private bool _isDisabled;
    [SerializeField] private bool _isDestroyed;

    [Header("Visual Display")]
    [SerializeField] private Image _hpDisplay;

    private void Start()
    {
        if(_unitSS == null)
        {
            _unitSS = GetComponent<unit_subsytems>();
        }
        _unitSS.OnDisabled += Disabled;
        _unitSS.OnDestroyed += Destroyed;

}
//Unity Functions
private void Update()
    {
        UpdateTimers(Time.deltaTime);
        UpdateRegen();
    }

    //Update Functions
    private void UpdateTimers(float time)
    {
        _sRegenWaitTimer += time;
        _isDisableTimer += time;
    }
    private void UpdateRegen()
    {
        if (!_isDestroyed)
        {
            if (_sRegenWaitTimer >= _sRegenWaitTime)
            {
                _isRegenWaiting = false;
                _curShieldHP += _sRegenRate * Time.deltaTime;
            }
            else
                return;
        }
        else
            return;
    }

    //Shield Functions
    private void Disabled(bool isDisabled)
    {
        _isDisabled = isDisabled;
    }
    private void Destroyed(bool isDestroyed)
    {
        _isDestroyed = isDestroyed;
    }
}
