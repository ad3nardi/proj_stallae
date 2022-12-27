using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_movement : MonoBehaviour
{
    [SerializeField] private unit_Manager unitManager;

    private void Awake()
    {
        unitManager = GetComponent<unit_Manager>();
    }


}
