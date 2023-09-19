using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    private void HealthChange(float curHP)
    {

    }

    public void TakeDamage(int i, float dmg)
    {

    }
    public float GetUnitHealth()
    {
        return 0f;
    }
    public bool[] GetActive()
    {
        return null;
    }

    public float[] GetHP()
    {
        return null;
    }
}