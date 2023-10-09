using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ab_bomberDrop : OptimizedBehaviour, IAbility
{
    [SerializeField] private float _cdTime, _cdTimer;
    [SerializeField] private GameObject _bomb;

    public void Activate()
    {
        if(_cdTimer <= 0)
        {
            Instantiate(_bomb);
        }
    }

    public void Target()
    {

    }

    public void End()
    {

    }

    public float GetCooldownTime()
    {
        return 0;
    }

    public float GetCooldownTimer()
    {
        return 0;
    }
}
