using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Building Settings")]
public class bgen_bldgSettings : ScriptableObject
{
    public Vector2Int buildingSize;
    
    public bgen_roofStrat roofStrategy;



    public Vector2Int size { get { return buildingSize; } }

}
