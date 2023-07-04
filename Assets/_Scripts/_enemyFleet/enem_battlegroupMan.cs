using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enem_battlegroupMan : MonoBehaviour
{
    [SerializeField] private List<Transform> _unitsT = new List<Transform>();
    private List<unit_Manager> _unitsM = new List<unit_Manager>();

    [Header("Thresholds")]
    [SerializeField] private float _aggro;
    [SerializeField] private float _spread;


    [Header("Thresholds")]
    [SerializeField] private float _range;
    [SerializeField] private float _hp;
    [SerializeField] private float _pwr;
    [SerializeField] private float _distFleet;
    [SerializeField] private float _distObj;
    [SerializeField] private float _strVwk;


    private void Start()
    {
        for (int i = 0; i < _unitsT.Count; i++)
        {
            _unitsT[i] = _unitsT[i].GetComponent<OptimizedBehaviour>().CachedTransform;
            _unitsM.Add(_unitsT[i].GetComponent<unit_Manager>());
        }
    }


}
