using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerSet", menuName = "LayerSet")]
public class LayerSet : ScriptableObject
{
    [Header("individual")]
    public int layerDefault = 0;
    public int layerTransparentFX = 1;
    public int layerIgnoreRaycast = 2;
    public int layerStatic = 3;
    public int layerWater = 4;
    public int layerUI = 5;
    public int layerPlayerUnit = 6;
    public int layerEnemyUnit = 7;
    public int layerNavigation = 8;

}
