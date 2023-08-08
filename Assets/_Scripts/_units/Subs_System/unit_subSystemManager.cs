using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_subSystemManager : OptimizedBehaviour
{
    [Header("Plugins")]
    private unit_Manager _unitM;
    public float _maxHP { get; private set; }
    public float _curHP { get; private set; }

    public List<Transform> _TsubSystems = new List<Transform>();
    public List<unit_subsystem> _subsystems { get; private set; } = new List<unit_subsystem>();

    public bool[] _activeSubsytems = new bool[6];
    public float[] _subSystemHP = new float[6];

    public float _activeSSCount;
    public event Action<float, float> SetMaxHealth = delegate { };


    private void Awake()
    {
        _unitM = GetComponentInParent<unit_Manager>();
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
        }
    }

    public void MoveSpeedChange(float pct)
    {
        pct = pct;
    }
}
