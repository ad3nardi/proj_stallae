using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Building Generation/Roof/Point If Single")]
public class pointIfSingle : bgen_roofStrat
{
    public override roof GenerateRoof(bgen_bldgSettings settings, RectInt bounds)
    {
        if(bounds.size.x == 1 && bounds.size.y == 1)
        {
            return new roof(RoofType.Point);
        }
        else
        {
            return new roof((RoofType)Random.Range(1, 4));
        }
    }
}
