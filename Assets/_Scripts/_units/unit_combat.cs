using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.Progress;

public class unit_combat : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] public TagSet tagSet;
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public LayerMask targetLayer;

    [Header("Settings")]
    [SerializeField] private float _fireRate, _curFireTime;
    [SerializeField] private float _damageDelay, _curDamageDelay;
    [SerializeField] private int _maxColliders;
    [SerializeField] private bool _isFiring;
    [SerializeField] private float _rotSpeed;

    [Header("RangeFinder")]
    [SerializeField] private float _atkRange;
    [SerializeField] private Transform _bestTarget;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private unit_Manager _target;
    [SerializeField] private int _firingTarget;
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
        for (int i = 0; i < _weaponsPos.Count; i++)
        {
            _weaponVFX.Add(_weaponsPos[i].GetComponentInChildren<VisualEffect>());
        }
    }
    private void Update()
    {
        updateFireTimer();
        updateDamageDelayTimer();
        updateWeaponRot();
        updateTargetsInRange();
        updateClosestTarget();
        if(_target != null)
            updateIfTargetInRange();
    }
    public void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CachedTransform.position, _atkRange);
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
                    return;
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
        for (int i = 0; i < _targetsInRange.Count; i++)
        {
            if (_targetsInRange.Contains(_target.transform))
            {

                _unitM._isIdle = true;
                
                return;
            }
            else
                return;
        }
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
                    _bestTarget = _targetsInRange[i].transform;
                    //if (_useAutoTarget)
                        //_target = _bestTarget;
                }
            }
        }
        else
            return;
    }
    private void updateWeaponRot()
    {
        if (_target != null)
        {
            for (int i = 0; i < _weaponsPos.Count; i++)
            {
                Quaternion targetRot = Quaternion.Euler(0, _target.CachedTransform.rotation.y, _weaponsPos[i].rotation.eulerAngles.z);
                _weaponsPos[i].rotation = Quaternion.Lerp(_weaponsPos[i].rotation, targetRot, Time.deltaTime * _rotSpeed);
                Debug.Log("Updating Rotation " + i);
            }
        }
    }
    private void updateFireTimer()
    {
        if (_fireRate > _curFireTime)
            _curFireTime += Time.deltaTime;
        else if (_fireRate <= _curFireTime && _target != null)
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
    private void Fire()
    {
        _curFireTime = 0f;
        for (int i = 0; i < _weaponsSet.Count; i++)
        {
            _target.TakeDamage(_firingTarget, _weaponsSet[i].weapon_damage);
        }
        for (int i = 0; i < _weaponVFX.Count; i++)
        {
            _weaponVFX[i].Play();
        }
    }

    public void TargetEnemy(unit_Manager target, int firingTarget)
    {
        _target = target;
        _firingTarget = firingTarget;
        _targetTransform = _target._subsytems[_firingTarget].CachedTransform;
    }

    //UI Based Functions
    public void ToggleAutoTarget()
    {
        _useAutoTarget = !_useAutoTarget;
    }
}