using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private bool _useAutoTarget;

    [Header("Lists")]
    [SerializeField] private List<Transform> targetsInRange;
    [SerializeField] private List<Transform> weapons;
    [SerializeField] private List<ParticleSystem> weaponsPS;

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
        targetsInRange = new List<Transform>();
        weapons = new List<Transform>();
        weaponsPS = new List<ParticleSystem>();
    }

    private void Update()
    {
        updateFireTimer();
        updateDamageDelayTimer();
        updateTargetsInRange();
        updateClosestTarget();
    }
    private void TargetOverideMoveStop()
    {

    }
    public void TargetEnemy()
    {

    }

    public void Fire()
    {
        _curFireTime = 0f;
        //Damage Enemy Unit
    }

    private void updateFireTimer()
    {
        if (_fireRate > _curFireTime)
            _curFireTime += Time.deltaTime;
        else
            return;
    }

    public void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CachedTransform.position, _atkRange);
    }
    private void updateTargetsInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(CachedTransform.position, _atkRange, targetLayer);
        Debug.Log("Hit Col Length: " + hitColliders.Length);
        Debug.Log("Target Length: " + targetsInRange.Count);
        if (hitColliders.Length > 0)
        {

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (targetsInRange.Contains(hitColliders[i].transform))
                    return;
                else
                 targetsInRange.Add(hitColliders[i].transform);
            }

        }
        else if (hitColliders.Length != targetsInRange.Count)
        {
            targetsInRange.Clear();
        }
        else
        {
            return;
        }
    }

    private void updateClosestTarget()
    {
        if (targetsInRange.Count != 0)
        {
            float closestDistSqr = Mathf.Infinity;
            Vector3 currentPos = CachedTransform.position;
            for (int i = 0; i < targetsInRange.Count; i++)
            {
                Vector3 distanceToTarget = targetsInRange[i].position - currentPos;
                float dSqrToTarget = distanceToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistSqr)
                {
                    closestDistSqr = dSqrToTarget;
                    _bestTarget = targetsInRange[i].transform;
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
}
