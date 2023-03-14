using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ss_logic_shield : unit_subsytems
{
    [Header ("Plugins")]
    [SerializeField] private unit_Manager _unitM;

    [Header("Shield Stats")]
    [SerializeField] private float _sMaxHP;
    [SerializeField] private float _sCurHP;
    [SerializeField] private float _sRegenRate;
    [SerializeField] private float _sDestroyedRegenRate;
    [SerializeField] private float _sRegenWaitTime;

    [Header("Shield Status")]
    [SerializeField] private bool _isRegenWaiting;

    [Header("Shield Internal")]
    [SerializeField] private float _sRegenWaitTimer;
    [SerializeField] private float _isDisableTimer;

    [Header("Visual Display")]
    [SerializeField] private Image _hpDisplay;

    //Unity Functions
    private void Update()
    {
        _sRegenWaitTimer += Time.deltaTime;
        _isDisableTimer += Time.deltaTime;
        UpdateRegen();
    }

    //Update Functions
    private void UpdateRegen()
    {
        if (_sRegenWaitTimer >= _sRegenWaitTime)
        {
            _isRegenWaiting = false;
            _sCurHP += _sRegenRate * Time.deltaTime;
        }
        else if (_sRegenWaitTimer >= _sRegenWaitTime)
            _sCurHP += _sDestroyedRegenRate * Time.deltaTime;
        else
            return;
    }

    //Shield Functions
    public void Disabled()
    {
       
    }
    public void Destroyed(float timer)
    {
         
        
    }
}
