using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class convoManager : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private TagSet _tagSet;
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private StorylineManager _sm;
    [SerializeField] private so_storylines _convo;

    [Header("Settings")]
    [SerializeField] private Vector3 _bounds;
    [SerializeField] private LayerMask _layer;

    private void Awake()
    {
        _tagSet = Helpers.TagSet;
        _layerSet = Helpers.LayerSet;
    }

    private void Start()
    {
        _sm = GetComponentInParent<StorylineManager>();
        
    }

    private void Update()
    {
        CheckCol();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(CachedTransform.position, _bounds*2);
    }

    private void CheckCol()
    {
        Collider[] col = Physics.OverlapBox(CachedTransform.position, _bounds, Quaternion.identity, _layer);
        if (col.Length == 0) return;
        if (col.Length >= 1)
        {
            _sm.PlayConvo(_convo);
            CachedGameObject.SetActive(false);
        }

    }
}
