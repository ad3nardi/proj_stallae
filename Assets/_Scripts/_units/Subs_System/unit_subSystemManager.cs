using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_subSystemManager : OptimizedBehaviour, ITargetable
{
    public unit_Manager _unitM { get; private set; }
    public float _maxHP { get; private set; }
    public float _curHP { get; private set; }

    public List<Transform> _TsubSystems = new List<Transform>();
    public List<unit_subsystem> _subsystems { get; private set; } = new List<unit_subsystem>();

    public bool[] _activeSubsytems = new bool[6];
    public float[] _subSystemHP = new float[6];
    private float _totalSubsytems;

    public float _activeSSCount;
    public event Action<float, float> SetMaxHealth = delegate { };
    public event Action<bool> UnitDestoryed = delegate { };
    public event Action<bool> SubSystemDestroyed = delegate { };


    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _unitM._iThisTarget = this;
        _unitM._subSystemMan = this;
        _maxHP = _unitM._unit.unitMaxHitPoints;
        for (int i = 0; i < 6; i++)
        {
            _subsystems.Add(_TsubSystems[i].GetComponent<unit_subsystem>());
            if (_activeSubsytems[i] == true)
            {
                _activeSSCount++;
                _subsystems[i]._activeSubsytem = true;
            }
        }
        _totalSubsytems = _activeSSCount;
    }
    private void OnEnable()
    {
        for (int i = 0; i < _subsystems.Count; i++)
        {
            _subsystems[i].OnHealthChanged += HealthChange;
        }
    }

    private void Start()
    {
        SetMaxHealth(_maxHP, _activeSSCount);
        for (int i = 0; i < 6; i++)
        {
            _subSystemHP[i] = _subsystems[i]._curHP;

        }       
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
        return _activeSubsytems;
    }
    public float[] GetHP()
    {
        return _subSystemHP;
    }

    private void HealthChange(float curHP)
    {
        for (int i = 0; i < 6; i++)
        {
            _subSystemHP[i] = _subsystems[i]._curHP;
        }
        _curHP -= curHP;
    }

    public void TakeDamage(int i, float dmg)
    {
        if (_subsystems[i]._activeSubsytem)
        {
            _subsystems[i].ModifyHealth(dmg);
            _subSystemHP[i] -= dmg;
            if(_subSystemHP[i] <= 0)
            {
                _activeSubsytems[i] = false;
                _activeSSCount--;
                SubSystemDestroyed(true);
                if (_activeSSCount == 0)
                {
                    UnitDestoryed(true);
                }
            }
        }

    }

    public float GetMaxHP()
    {
        return _maxHP/ _totalSubsytems;
    }
}
