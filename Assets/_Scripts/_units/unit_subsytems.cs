using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_subsytems : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager unitManager;

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] public float maxHP;
    [SerializeField] public float curHP { get; private set; }

    public static event Action<unit_subsytems> OnHealthAdded = delegate { };
    public static event Action<unit_subsytems> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };

    //UNITY FUNCTIONS
    private void Awake()
    {
        unitManager = GetComponent<unit_Manager>();
    }
    private void Start()
    {
        maxHP = unitManager.unit.unitMaxHitPoints;
    }
    private void OnDisable()
    {
        OnHealthRemoved(this);
    }

    //HEALTH & DAMAGE FUNCTIONS
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
}

public enum subsytemType
{
    hull,
    engine,
    weapon,

}
