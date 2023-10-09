using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class valiera_addIn : OptimizedBehaviour
{
    [SerializeField] private unit_Manager unit_manager;

    private void Start()
    {
        unit_manager = GetComponent<unit_Manager>();
        SelectionMan.Instance.valeria = unit_manager;
    }
}
