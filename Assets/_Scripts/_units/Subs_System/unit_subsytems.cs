using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class unit_subsytems : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager _unitManager;
    public unit_Manager UnitManager
    {
        get
        {
            if (_unitManager == null)
                _unitManager = GetComponent<unit_Manager>();
            return _unitManager;
        }
        set => _unitManager = value;
    }
    [SerializeField] public bool _isDestroyed;
    [SerializeField] public bool _isDisabled;

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] private float _maxHP;

    [SerializeField] public float _curHP { get; private set; }

    public static event Action<unit_subsytems> OnHealthAdded = delegate { };
    public static event Action<unit_subsytems> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };

    //UNITY FUNCTIONS
    private void Start()
    {
        _unitManager = UnitManager;
        _maxHP = _unitManager.unit.unitMaxHitPoints / _unitManager._subsytems.Count;
        Debug.Log(CachedGameObject + "Unit Max HP:" + _maxHP );
        SetToMaxHP();
    }

    private void OnDisable()
    {
        OnHealthRemoved(this);
    }

    //HEALTH & DAMAGE FUNCTIONS
    public void SetToMaxHP()
    {
        _curHP = _maxHP;
        OnHealthAdded(this);
    }
    public void ModifyHealth(float amount)
    {
        _curHP += amount;
        float currentHPpct = _curHP / _maxHP;
        OnHealthPctChanged(currentHPpct);
        if (_curHP >= 0)
        {
            _unitManager.SubsSystemDestoryed(this);
        }
        else
            return;   
    }
    public void SystemDisable()
    {
        _isDisabled = true;
    }
    public void SystemDestroy()
    {
        _isDestroyed = true;
    }
}

public enum subsytemType
{
    squadron,
    hull,
    engine,
    weapon,
    shield
}
