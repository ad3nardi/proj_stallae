using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_squadTarget : OptimizedBehaviour, ITargetable
{
    [Header("Plugins")]
    private unit_Manager _unitM;
    public float _maxHP { get; private set; }
    public float _curHP { get; private set; }

    public bool[] _activeSquad = new bool[6];
    public float[] _hpSquad = new float[6];

    public float _activeSquadCount;
    public event Action<bool> SquadMembDestroyed = delegate { };
    public event Action<bool> UnitDestoryed = delegate { };

    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _unitM._iThisTarget = this;
        _unitM._subSystemMan = null;
        _maxHP = _unitM._unit.unitMaxHitPoints;
        for (int i = 0; i < 6; i++)
        {
            if (_activeSquad[i] == true)
            {
                _activeSquadCount++;
            }
        }
    }

    private void Start()
    {
        SetMaxHealth();
    }

    private void SetMaxHealth()
    {
        for (int i = 0; i < 6; i++)
        {
            if (_activeSquad[i])
            {
                _hpSquad[i] = _unitM._unit.unitMaxHitPoints / _activeSquadCount;

            }
        }
    }

    public void TakeDamage(int i, float dmg)
    {
        if (_activeSquad[i] == true)
        {
            _hpSquad[i] -= dmg;
            if (_hpSquad[i] <= 0)
            {
                _activeSquad[i] = false;
                _activeSquadCount--;
                SquadMembDestroyed(true);
                if (_activeSquadCount == 0)
                {
                    UnitDestoryed(true);
                }
            }
        }
        HealthChange(dmg);

    }

    private void HealthChange(float curHP)
    {
        _curHP -= curHP;
    } 
    
    public Transform GetTransform()
    {
        return CachedTransform;
    }
    public GameObject GetGameObject()
    {
        return CachedGameObject;
    }
    public float GetUnitHealth()
    {
        return _curHP;
    }
    public bool[] GetActive()
    {
        return _activeSquad;
    }
    public float[] GetHP()
    {
        return _hpSquad;
    }
}
