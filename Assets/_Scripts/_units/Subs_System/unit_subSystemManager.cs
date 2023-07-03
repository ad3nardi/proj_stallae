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

    public event Action<float, float> SetMaxHealth = delegate { };


    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _maxHP = _unitM._unit.unitMaxHitPoints;
    }

    private void Start()
    {
        SetMaxHealth(_maxHP, _TsubSystems.Count);
        _subsystems.Clear();
        for (int i = 0; i < _TsubSystems.Count; i++)
        {
            _subsystems.Add(_TsubSystems[i].GetComponent<unit_subsystem>());
        }
    }

    public void TakeDamage(int i, float dmg)
    {
        _subsystems[i].ModifyHealth(dmg);
    }


    public void MoveSpeedChange(float pct)
    {
        pct = pct;
    }
}
