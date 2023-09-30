using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_iTarget_link : OptimizedBehaviour, ITargetable
{
    ITargetable _targetLink;

    private void Awake()
    {
        _targetLink = GetComponentInChildren<ITargetable>();
    }
}
