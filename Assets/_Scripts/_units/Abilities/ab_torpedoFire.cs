using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ab_torpedoFire : OptimizedBehaviour, IAbility
{
    [Header("Ability Settings")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private float _abCooldownTime;
    [SerializeField] private float _abCooldownTimer;
    [SerializeField] private float _activeTime;
    [SerializeField] private float _activeTimer;
    [SerializeField] private bool _isActive;

    [Header("Ability Inputs")]
    [SerializeField] private GameObject _pfTorpedo;



    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();

        _unitM._iThisAbility = this;
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
        if (_abCooldownTimer <= 0)
            return;

        _abCooldownTimer -= Time.deltaTime;
    }
    private void UpdateActiveTime()
    {
        if (_activeTimer <= 0)
        {
            End();
            return;
        }
        if (_isActive)
        {
            _activeTimer -= Time.deltaTime;
        }
    }

    public void Activate()
    {
        if (_abCooldownTimer > 0)
            return;
        Instantiate(_pfTorpedo);
        
        _isActive = true;
    }
    public void Target()
    {

    }
    public void End()
    {
        _isActive = false;
        _activeTimer = _activeTime;
        _abCooldownTimer = _abCooldownTime;

    }
}
