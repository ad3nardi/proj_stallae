using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    public void Activate()
    {

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
