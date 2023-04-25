using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class wpn_torpedo : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private objectPooling _parentPool;
    [SerializeField] private VisualEffect _explosion;
    [SerializeField] private List<unit_subsytems> _damageTargets;
    
    [Header("Settings")]
    [SerializeField] private float _hitSize;
    [SerializeField] private float _areaOfEffect;
    [SerializeField] private float _damage;
    [SerializeField] private float _lifetime;
    [SerializeField] private float _moveSpeed;
    
    //UNITY FUNCTIONS
    private void Awake()
    {

        _damageTargets = new List<unit_subsytems>();
        _parentPool = GetComponentInParent<objectPooling>();
    }
    private void Start()
    {
        _layerSet = Helpers.LayerSet;
    }
    private void Update()
    {
        UpdatePosition();
        UpdateCollision();
    }
    
    //UPDATE FUNCTIONS
    private void UpdatePosition()
    {
        CachedTransform.position += CachedTransform.forward * _moveSpeed * Time.deltaTime;
    }
    private void UpdateCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(CachedTransform.position, _hitSize, _targetLayer);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                _damageTargets.Add(hitColliders[i].GetComponent<unit_subsytems>());
            }
            DamageTargets();
        }
        else
            return;
    }

    //WEAPON FUNCTIONS
    private void DamageTargets()
    {
        _explosion.Play();
        for (int i = 0; i < _damageTargets.Count; i++)
        {
            _damageTargets[i].ModifyHealth(-_damage);
        }
        _parentPool.ReturnObject(this);
    }
}
