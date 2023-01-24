using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class unit_health : OptimizedBehaviour
{
    [SerializeField] private unit_Manager unitManager;

    [SerializeField] public float maxHP;
    [SerializeField] public float curHP { get; private set; }

    public static event Action<unit_health> OnHealthAdded = delegate { };
    public static event Action<unit_health> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };

    private void Awake()
    {
        unitManager = GetComponent<unit_Manager>();
    }

    private void Start()
    {
        maxHP = unitManager.unit.unitMaxHitPoints;
    }

    public void SetToMaxHP()
    {
        curHP = maxHP;
        OnHealthAdded(this);
    }

    public void ModifyHealth(float amount)
    {
        curHP += amount;
        float currentHPpct = curHP / maxHP;
        OnHealthPctChanged(currentHPpct);
    }
    private void OnDisable()
    {
        OnHealthRemoved(this);
    }
}