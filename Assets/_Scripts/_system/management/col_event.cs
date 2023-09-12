using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class col_event : OptimizedBehaviour
{
    [SerializeField] public UnityEvent _genericEvent;
    [SerializeField] private bool _repeat;
    [SerializeField] private float _colRadius;
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private LayerMask _layerMask;

    private void Awake()
    {
        _layerSet = Helpers.LayerSet;
    }
    private void Update()
    {
        ColHit();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CachedTransform.position, _colRadius);
    }

    private void ColHit()
    {
        Collider[] col = Physics.OverlapSphere(CachedTransform.position, _colRadius, _layerMask);
        if(col.Length > 0)
        {
            _genericEvent.Invoke();
            if(_repeat == false)
            {
                CachedGameObject.SetActive(false);
            }
        }
        
    }
}
