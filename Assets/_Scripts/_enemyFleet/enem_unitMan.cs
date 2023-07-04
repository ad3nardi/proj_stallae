using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enem_unitMan : OptimizedBehaviour
{

    [Header("Plugins")]
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public TagSet tagSet;
    [SerializeField] public unit_settings _unit;
    [SerializeField] public int _sizeTag;

    [Header("Mission Parameter")]
    [SerializeField] private currentMission _cMission;
    [SerializeField] public Transform _targetT;
    [SerializeField] private unit_Manager _targetM;
    [SerializeField] private Vector3 _targetP;
    

    public void SelectTarget(Transform t)
    {
        _targetM = t.GetComponent< unit_Manager>();
        _targetP = t.position;

    }
}
