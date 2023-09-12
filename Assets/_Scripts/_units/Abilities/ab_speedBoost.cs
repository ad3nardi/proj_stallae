using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ab_speedBoost : OptimizedBehaviour, IAbility
{
    [Header("Ability Settings")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private float _abCooldownTime;
    [SerializeField] private float _abCooldownTimer;
    [SerializeField] private float _activeTime;
    [SerializeField] private float _activeTimer;

    [Header("Ability Inputs")]
    [SerializeField] private float _originalSpeed;
    [SerializeField] private float _speedBoostAmnt;

    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _originalSpeed = _unitM._unit.unitMaxSpeed;
        _abCooldownTime = _unitM._unit.abCooldownTime;
        _activeTime = _unitM._unit.abActiveTime;
    }

    private void Update()
    {
        UpdateTimer();
        UpdateActiveTime();
    }

    private void UpdateTimer()
    {
        if(_abCooldownTimer <= 0) return;
        _abCooldownTimer -= Time.deltaTime;
    }
    public void Activate()
    {
        if (_abCooldownTimer > 0) return;
        _unitM._AImovement.maxSpeed += _speedBoostAmnt;
        _activeTimer = _activeTime;
    }
    public void Target()
    {

    }
    private void UpdateActiveTime()
    {
        if(_activeTimer <= 0)
        {
            End();
            return;
        }
        _activeTimer -= Time.deltaTime;
    }

    public void End()
    {
        _abCooldownTimer = _abCooldownTime;
        _unitM._AImovement.maxSpeed = _originalSpeed;
    }


}
