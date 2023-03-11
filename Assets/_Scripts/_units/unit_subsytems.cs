using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_subsytems : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager _unitManager;
    public unit_Manager UnitManager
    {
        get
        {
            if (_unitManager == null) _unitManager = GetComponent<unit_Manager>();
            return _unitManager;
        }
        set => _unitManager = value;
    }

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] private float _maxHP;

    [SerializeField] public float curHP { get; private set; }

    public static event Action<unit_subsytems> OnHealthAdded = delegate { };
    public static event Action<unit_subsytems> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };

    //UNITY FUNCTIONS
    private void Start()
    {
        Debug.Log(_unitManager.unit.unitMaxHitPoints);
        _maxHP = _unitManager.unit.unitMaxHitPoints / _unitManager._subsytems.Count;
        SetToMaxHP();
    }

    private void OnDisable()
    {
        OnHealthRemoved(this);
    }

    //HEALTH & DAMAGE FUNCTIONS
    public void SetToMaxHP()
    {
        curHP = _maxHP;
        OnHealthAdded(this);
    }
    public void ModifyHealth(float amount)
    {
        curHP += amount;
        float currentHPpct = curHP / _maxHP;
        OnHealthPctChanged(currentHPpct);
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
