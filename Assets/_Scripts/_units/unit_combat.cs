using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class unit_combat : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public LayerMask targetLayer;

    [Header("Settings")]
    [SerializeField] private float _fireRate, _curFireTime;
    [SerializeField] private float _damageDelay, _curDamageDelay;
    [SerializeField] private int _maxColliders;
    [SerializeField] private bool _isFiring;

    [Header("RangeFinder")]
    [SerializeField] private float _atkRange;
    [SerializeField] private Transform _bestTarget;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _useAutoTarget;

    [Header("Lists")]
    [SerializeField] public List<Transform> _targetsInRange;
    [SerializeField] private List<wpn_settings> _weaponsSet;
    [SerializeField] private List<Transform> _weaponsPos;
    [SerializeField] private List<VisualEffect> _weaponsVis;

    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _fireRate = _unitM.unit.unitFireRate;
        _atkRange = _unitM.unit.unitAttackRange;
        _useAutoTarget = false;
    }

    private void Start()
    {
        layerSet = _unitM.layerSet;
        _bestTarget = null;
        //Instantiate Lists
        _targetsInRange = new List<Transform>();
        _weaponsSet = new List<wpn_settings>();
        _weaponsPos = new List<Transform>();
        _weaponsVis = new List<VisualEffect>();
    }
    private void Update()
    {
        updateFireTimer();
        updateDamageDelayTimer();
        updateTargetsInRange();
        updateClosestTarget();
        updateWeaponRot();
    }
    public void Fire()
    {
        _curFireTime = 0f;
        for (int i = 0; i < _weaponsSet.Count; i++)
        {
            _target.GetComponent<unit_health>().ModifyHealth(_weaponsSet[i].weapon_damage);
        }
        for (int i = 0; i < _weaponsVis.Count; i++)
        {
            _weaponsVis[i].Play();
        }
        
    }

    private void TargetOverideMoveStop()
    {

    }
    public void TargetEnemy()
    {
        
    }
    private void updateWeaponRot()
    {
        for (int i = 0; i < _weaponsPos.Count; i++)
        {
            _weaponsPos[i].LookAt(_target);
        }
    }
    private void updateFireTimer()
    {
        if (_fireRate > _curFireTime)
            _curFireTime += Time.deltaTime;
        else
            return;
    }
    private void updateTargetsInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(CachedTransform.position, _atkRange, targetLayer);
        Debug.Log("Hit Col Length: " + hitColliders.Length);
        Debug.Log("Target Length: " + _targetsInRange.Count);
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
                }
            }
        }
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
    public void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CachedTransform.position, _atkRange);
    }
}
