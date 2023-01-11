using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(unit_health))]
[RequireComponent(typeof(unit_movement))]
[RequireComponent(typeof(unit_combat))]
public class unit_Manager : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] public unit_settings unit;
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public GameObject _highlightGO;

    [Header("Unit Plugins")]
    [SerializeField] public unit_health _health;
    [SerializeField] public unit_movement _movement;
    [SerializeField] public unit_combat _combat;

    [Header("Unit Status")]
    [SerializeField] public bool _isSelected;
    [SerializeField] public Transform _target;
    [SerializeField] public Vector3 _targetPosition;


    private void Awake()
    {
        //Cache
        _health = GetComponent<unit_health>();
        _movement = GetComponent<unit_movement>();
        _combat = GetComponent<unit_combat>();

        SetDefaults();
        UnitDeselected();
    }

    private void Start()
    {
        SetMoveTarget(CachedTransform.position);
    }

    public void UnitSelected()
    {
        _isSelected = true;
        _highlightGO.SetActive(true);
    }

    public void UnitDeselected()
    {
        _isSelected = false;
        _highlightGO.SetActive(false);
    }
    public void SetMoveTarget(Vector3 target)
    {
        Debug.Log("UnitM - Set Move Target: " + target);
        _movement.SetTarget(target);
        _movement.RecalculatePath();
    }

    private void SetAttackTarget()
    {

    }


    private void SetDefaults()
    {
        UnitDeselected();
        _target = null;

    }
}

