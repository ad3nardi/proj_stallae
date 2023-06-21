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
                _unitManager = GetComponentInParent<unit_Manager>();
            return _unitManager;
        }
        set => _unitManager = value;
    }
    [SerializeField] private bool _isDestroyed;
    [SerializeField] private bool _isDisabled;

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] private float _maxHP;
    [SerializeField] private float _disableTimer;

    [SerializeField] public float _curHP { get; private set; }

    public static event Action<unit_subsytems> OnHealthAdded = delegate { };
    public static event Action<unit_subsytems> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };
    public event Action<bool> OnDestroyed = delegate { };
    public event Action<bool> OnDisabled = delegate { };

    //UNITY FUNCTIONS
    private void OnEnable()
    {
        SetToMaxHP();
        _unitManager = UnitManager;
        _maxHP = _unitManager._unit.unitMaxHitPoints / _unitManager._subsytems.Count;

    }
    private void Start()
    {
        SetToMaxHP();
    }

    private void Update()
    {
        UpdateTimers(Time.deltaTime);
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
        OnDestroyed(_isDestroyed);
        OnDisabled(_isDisabled);
    }

    public void ModifyHealth(float amount)
    {
        Debug.Log("Taking Damage");

        _curHP += amount;
        float currentHPpct = _curHP / _maxHP;
        OnHealthPctChanged(currentHPpct);
        if (_curHP >= 0)
        {
            OnDestroyed(false);
        }
        else
            OnDestroyed(true);   
    }

    public void SystemDisable(bool isDisabled)
    {
        _isDisabled = isDisabled;
        OnDisabled(_isDisabled);
    }

    public void SystemDestroy()
    {
        _isDestroyed = true;
        OnDestroyed(_isDestroyed);
    }

    private void UpdateTimers(float time)
    {
        _disableTimer += time;

        if (!_isDisabled)
            return;
        else if (_isDisabled)
        {
            if(_disableTimer >= 0)
            {
                SystemDisable(false);
            }
        }
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
