using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ss_logic_engines : OptimizedBehaviour
{
    [Header("Engine Sub-System Settings")]
    private unit_subsystem _subSystem;
    [SerializeField] private float _destroyedPct;
    [SerializeField] private float _disabledPct;

    private void Awake()
    {
        _subSystem = GetComponent<unit_subsystem>();
    }

    private void OnEnable()
    {
        _subSystem.OnDestroyed += ReduceMoveSpeedDestoryed;
    }

    private void ReduceMoveSpeedDestoryed(bool des)
    {
        /*
        if (des)
        {
            _subSystem.MoveSpeedChange(_destroyedPct);
        }
        else
            return;
    */
        }

    private void OnDisable()
    {
        _subSystem.OnDestroyed -= ReduceMoveSpeedDestoryed;  
    }
}
