using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_subsystem : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_subSystemManager _subsystemM;
    [SerializeField] public bool _activeSubsytem;
    [SerializeField] private bool _isDestroyed;
    [SerializeField] private bool _isDisabled;
    [SerializeField] private bool _isSubscribed;

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] private float _maxHP;
    [SerializeField] private float _disableTimer;

    [SerializeField] public float _curHP { get; private set; }

    public static event Action<unit_subsystem> OnHealthAdded = delegate { };
    public static event Action<unit_subsystem> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };
    public event Action<float> OnHealthChanged = delegate { };
    public event Action<bool> OnDestroyed = delegate { };
    public event Action<bool> OnDisabled = delegate { };

    //UNITY FUNCTIONS
    private void Awake()
    {
        _isSubscribed = false;
        _subsystemM = GetComponentInParent<unit_subSystemManager>();
    }
    private void Start()
    {
        if (_activeSubsytem && !_isSubscribed)
        {
            _isSubscribed = true;
            _subsystemM.SetMaxHealth += SetToMaxHP;
        }
    }
    private void OnEnable()
    {
        if(_activeSubsytem && !_isSubscribed)
        {
            _isSubscribed = true;
            _subsystemM.SetMaxHealth += SetToMaxHP;
        }
    }


    private void Update()
    {
        UpdateTimers(Time.deltaTime);
    }

    private void OnDisable()
    {
        if (_isSubscribed)
        {
            _subsystemM.SetMaxHealth -= SetToMaxHP;
            OnHealthRemoved(this);
        }
        
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
        OnHealthChanged(_curHP);
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
    hull,
    weapon1,
    weapon2,
    shield,
    engine,
    special
}
