using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warpGate : OptimizedBehaviour
{
    [SerializeField] private TagSet _tagSet;
    [SerializeField] private LayerSet _layerSet;

    [SerializeField] private LayerMask _checkLayers;
    [SerializeField] private float _checkScale;
    [SerializeField] private float _warpTime;

    public Vector3 _WarpPos;
    public Vector3 _WarpPoint;

    private void Awake()
    {
        _tagSet = Helpers.TagSet;
        _layerSet = Helpers.LayerSet;
    }

    private void Update()
    {
        CheckProximityForWarp(); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(CachedTransform.position, _checkScale);
    }

    private void CheckProximityForWarp()
    {
        Collider[] hitCol = Physics.OverlapSphere(CachedTransform.position, _checkScale);
        if (hitCol.Length > 0)
        {
            for (int i = 0; i < hitCol.Length; i++)
            {
                Debug.Log("Can teleport" + hitCol[i]);
            }
        }
    }
}
