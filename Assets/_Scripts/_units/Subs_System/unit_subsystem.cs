using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class unit_subsystem : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_subSystemManager _subsystemM;
    [SerializeField] private bool _isDestroyed;
    [SerializeField] private bool _isDisabled;

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] private float _maxHP;
    [SerializeField] private float _disableTimer;

    [SerializeField] public float _curHP { get; private set; }

    public static event Action<unit_subsystem> OnHealthAdded = delegate { };
    public static event Action<unit_subsystem> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };
    public event Action<bool> OnDestroyed = delegate { };
    public event Action<bool> OnDisabled = delegate { };

    //UNITY FUNCTIONS
    private void Awake()
    {
        _subsystemM = GetComponentInParent<unit_subSystemManager>();
    }
    private void OnEnable()
    {
        _subsystemM.SetMaxHealth += SetToMaxHP;
    }


    private void Update()
    {
        UpdateTimers(Time.deltaTime);
    }

    private void OnDisable()
    {
        _subsystemM.SetMaxHealth -= SetToMaxHP;
        OnHealthRemoved(this);
    }

    //HEALTH & DAMAGE FUNCTIONS
    public void SetToMaxHP(float maxHp, float subSystemCount)
    {
        _maxHP = maxHp / subSystemCount;
        _curHP = _maxHP;
        OnHealthAdded(this);
        _isDisabled = false;
        _isDestroyed = false;
        OnDestroyed(_isDestroyed);
        OnDisabled(_isDisabled);
    }

    public void ModifyHealth(float amount)
    {
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
