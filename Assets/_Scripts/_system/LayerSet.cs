using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerSet", menuName = "LayerSet")]
public class LayerSet : ScriptableObject
{
    [Header("individual")]
    public int layerDefault = 0;
    public int transparentFX = 1;
    public int layerIgnoreRaycast = 2;
    public int layerPlayerUnit = 3;
    public int layerEnemyUnit = 4;
    public int layerUI = 5;
    public int layerStatic = 6;
    public int layerEnvionrment = 7;

}
