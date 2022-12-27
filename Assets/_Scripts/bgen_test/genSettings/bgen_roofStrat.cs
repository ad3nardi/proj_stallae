using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class bgen_roofStrat : ScriptableObject
{
    public abstract roof GenerateRoof(bgen_bldgSettings settings, RectInt bounds);
}
