using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgen_defaultRoof : bgen_roofStrat
{
    public override roof GenerateRoof(bgen_bldgSettings settings, RectInt bounds)
    {
        return new roof();
    }
}
