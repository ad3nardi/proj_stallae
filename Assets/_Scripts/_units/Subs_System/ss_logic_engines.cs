using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ss_logic_engines : unit_subsytems
{
    [Header("Engine Sub-System Settings")]
    [SerializeField] private float _destroyedPct;
    [SerializeField] private float _disabledPct;

    private void OnEnable()
    {
        OnDestroyed += ReduceMoveSpeedDestoryed;
    }

    private void ReduceMoveSpeedDestoryed(bool des)
    {
        if (des)
        {
            UnitManager.MoveSpeedChange(_destroyedPct);
        }
        else
            return;
    }

    private void OnDisable()
    {
        OnDestroyed -= ReduceMoveSpeedDestoryed;  
    }
}
