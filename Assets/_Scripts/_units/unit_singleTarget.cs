using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_singleTarget : OptimizedBehaviour, ITargetable
{
    [Header("Plugins")]
    private unit_Manager _unitM;
    private unit_settings _unit;
    public float _maxHP { get; private set; }
    public float _curHP { get; private set; }

    float[] _array = new float[6];

    public event Action<bool> SquadMembDestroyed = delegate { };
    public event Action<bool> UnitDestoryed = delegate { };

    private void Start()
    {
        SetMaxHealth();
    }

    private void SetMaxHealth()
    {
        _maxHP = _unit.unitMaxHitPoints;
        _curHP = _maxHP;
        _array[0] = _maxHP;
    }

    public void TakeDamage(int i, float dmg)
    {
        _curHP -= dmg;
        if(_curHP <= 0)
        {
            UnitDestoryed(true);
        }
        HealthChange(dmg);
    }
    public Transform GetTransform()
    {
        return CachedTransform;
    }
    public GameObject GetGameObject()
    {
        return CachedGameObject;
    }
    private void HealthChange(float curHP)
    {
        _curHP -= curHP;
    }
    public float GetUnitHealth()
    {
        return _curHP;
    }
    public float GetMaxHP()
    {
        return _maxHP;
    }
}
