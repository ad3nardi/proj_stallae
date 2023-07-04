using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eFl_missionMan : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private GameObject _vShipObj;
    private Transform _vShip;

    [SerializeField] private List<Transform> _enemFleetList = new List<Transform>();

    private void Start()
    {
        _vShipObj = _vShipObj.GetComponent<OptimizedBehaviour>().CachedGameObject;
        _vShip = _vShipObj.GetComponent<OptimizedBehaviour>().CachedTransform;
    
    }




}

