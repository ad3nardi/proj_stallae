using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VFX;

public class unit_combat : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] public TagSet tagSet;
    [SerializeField] public LayerSet layerSet;
    [Tooltip("Layer to check raycast hit against all possible obstacles, i.e - other units + map objects")]
    [SerializeField] public LayerMask targetCheckLayer;
    [SerializeField] public LayerMask targetLayer;

    [Header("Settings")]
    [SerializeField] private float _fireRate, _curFireTime;
    [SerializeField] private float _damageDelay, _curDamageDelay;
    [SerializeField] private int _maxColliders;
    [SerializeField] private bool _isFiring;
    [SerializeField] private float _rotSpeed;

    [Header("Targeting")]
    [SerializeField] private unit_Manager _bestTarget;
    [SerializeField] private unit_Manager _targetM;
    [SerializeField] private ITargetable _Itarget;
    [SerializeField] private unit_subSystemManager _targetSS;
    [SerializeField] private int _firingTarget;

    [Header("RangeFinder")]
    [SerializeField] private float _atkRange;
    [SerializeField] public bool _useAutoTarget { get; private set; }
    [SerializeField] public bool _targetInRange { get; private set; }

    [Header("Lists")]
    [SerializeField] public List<Transform> _targetsInRange;
    [SerializeField] private List<wpn_settings> _weaponsSet = new List<wpn_settings>();
    [SerializeField] private List<Transform> _weaponsPos = new List<Transform>();
    [SerializeField] private List<VisualEffect> _weaponVFX = new List<VisualEffect>();

    //UNITY FUNCTIONS
    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _fireRate = _unitM._unit.unitFireRate;
        _atkRange = _unitM._unit.unitAttackRange;
        _useAutoTarget = false;
    }
    private void Start()
    {
        layerSet = Helpers.LayerSet;
        tagSet = Helpers.TagSet;
        _bestTarget = null;

        _targetsInRange = new List<Transform>();
        _weaponVFX.Clear();
        if(_weaponsPos.Count > 0)
        {
            for (int i = 0; i < _weaponsPos.Count; i++)
            {
                _weaponVFX.Add(_weaponsPos[i].GetComponentInChildren<VisualEffect>());
            }
        }
        
    }
    private void Update()
    {
        updateFireTimer();
        updateDamageDelayTimer();
        updateWeaponRot();
        updateTargetsInRange();
        updateClosestTarget();
        if(_targetM != null)
            updateIfTargetInRange();
    }
    public void OnDrawGizmos()
    { 
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(CachedTransform.position, _atkRange);
        if(_targetM != null )
            Gizmos.DrawLine(CachedTransform.position, _targetM.CachedTransform.position);

    }

    //UPDATE FUNCTIONS
    private void updateTargetsInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(CachedTransform.position, _atkRange, targetLayer);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (_targetsInRange.Contains(hitColliders[i].transform))
                    continue;
                else
                    _targetsInRange.Add(hitColliders[i].transform);
            }
        }
        else if (hitColliders.Length != _targetsInRange.Count)
        {
            _targetsInRange.Clear();
        }
        else
        {
            return;
        }
    }
    private void updateIfTargetInRange()
    {
        if (_targetsInRange.Contains(_targetM.CachedTransform))
             _unitM._targetInRange = true;
        else
            _unitM._targetInRange = false;
    }
    private void updateClosestTarget()
    {
        if (_targetsInRange.Count != 0)
        {
            float closestDistSqr = Mathf.Infinity;
            Vector3 currentPos = CachedTransform.position;
            for (int i = 0; i < _targetsInRange.Count; i++)
            {
                Vector3 distanceToTarget = _targetsInRange[i].position - currentPos;
                float dSqrToTarget = distanceToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistSqr)
                {
                    closestDistSqr = dSqrToTarget;
                    _bestTarget = _targetsInRange[i].transform.GetComponent<unit_Manager>();
                    //AUTO TARGET ENABLED THROUGH UI:
                    if (_useAutoTarget)
                    {
                        _targetM = _bestTarget;
                    }
                }
            }
        }
        else
            return;
    }
    private void updateWeaponRot()
    {
        if (_targetM != null)
        {
            for (int i = 0; i < _weaponsPos.Count; i++)
            {
                Quaternion targetRot = Quaternion.LookRotation(_targetM.CachedTransform.position - CachedTransform.position);
                _weaponsPos[i].rotation = Quaternion.Slerp(_weaponsPos[i].rotation, targetRot, Time.deltaTime * _rotSpeed);
            }
        }
        else
            return;
    }
    private void updateFireTimer()
    {
        if (_fireRate > _curFireTime)
            _curFireTime += Time.deltaTime;
        else if (_fireRate <= _curFireTime && _targetM != null)
            Fire();
        else
            return;
    }
    private void updateDamageDelayTimer()
    {
        if (_isFiring)
        {
            if (_damageDelay > _curDamageDelay)
                _curDamageDelay += Time.deltaTime;
        }
        else
            return;
    }

    //Combat Functions
    public void TargetEnemy(unit_Manager target, int firingTarget)
    {
        _targetM = target;

        _Itarget = _targetM._targetComp;
        _firingTarget = firingTarget;
    }

    private void Fire()
    {
        _curFireTime = 0f;
        bool[] actives = _Itarget.GetActive();
        float[] hps = _Itarget.GetHP();


        if (actives[_firingTarget] != true)
        {
            for (int f = 0; f < 6; f++)
            {
                _firingTarget = f;
                if (actives[_firingTarget] == true)
                {
                    break;
                }
            }
        }

        if (_unitM._targetInRange == true)
        {
            if (actives[_firingTarget] == true)
            {
                _isFiring = true;
                for (int i = 0; i < _weaponVFX.Count; i++)
                {
                    _weaponVFX[i].Play();
                }
                for (int i = 0; i < _weaponsSet.Count; i++)
                {
                    _Itarget.TakeDamage(_firingTarget, -_weaponsSet[i].weapon_damage);
                }
            }
        }
        else
            return;
    }

    //UI Based Functions
    public void ToggleAutoTarget()
    {
        _useAutoTarget = !_useAutoTarget;
    }
}