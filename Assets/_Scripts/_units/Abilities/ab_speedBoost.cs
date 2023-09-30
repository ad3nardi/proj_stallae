using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ab_speedBoost : OptimizedBehaviour, IAbility
{
    [Header("Ability Settings")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private float _abCooldownTime;
    [SerializeField] private float _abCooldownTimer;
    [SerializeField] private float _activeTime;
    [SerializeField] private float _activeTimer;
    [SerializeField] private bool _isActive;

    [Header("Ability Inputs")]
    [SerializeField] private float _originalSpeed;
    [SerializeField] private float _originalAcceleration;
    [SerializeField] private float _speedBoostAmnt;
    [SerializeField] private float _accelBoostAmnt;

    [Header("Ability Visuals")]
    [SerializeField] private VisualEffect _vfxBoost;
    [SerializeField] private VisualEffect _vfxActive;


    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _vfxActive.Stop();

        _unitM._iThisAbility= this;
        _originalSpeed = _unitM._unit.unitMaxSpeed;
        _originalAcceleration = _unitM._unit.unitAcceleration;
        _abCooldownTime = _unitM._unit.abCooldownTime;
        _activeTime = _unitM._unit.abActiveTime;

        _abCooldownTimer = 0f;
        _activeTimer = _activeTime;
    }

    private void Update()
    {
        UpdateCooldownTimer();
        UpdateActiveTime();
    }

    private void UpdateCooldownTimer()
    {
        if(_abCooldownTimer <= 0)
            return;

        _abCooldownTimer -= Time.deltaTime;
    }
    private void UpdateActiveTime()
    {
        if(_activeTimer <= 0)
        {
            End();
            return;
        }
        if(_isActive)
        {
            _activeTimer -= Time.deltaTime;
        }
    }

    public void Activate()
    {
        if (_abCooldownTimer > 0)
            return;

        _unitM._AImovement.maxSpeed += _speedBoostAmnt;
        _unitM._AImovement.acceleration += _accelBoostAmnt;
        _vfxBoost.Play();
        _vfxActive.Play();
        _isActive = true;
    }
    public void Target()
    {

    }
    public void End()
    {
        _isActive = false;
        _vfxActive.Stop();
        _activeTimer = _activeTime;
        _abCooldownTimer = _abCooldownTime;
        _unitM._AImovement.maxSpeed = _originalSpeed;
        _unitM._AImovement.acceleration = _originalAcceleration;
    }


}
