using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_highlight : OptimizedBehaviour
{
    private unit_Manager _unitM;
    private MeshRenderer _meshRend;

    private void OnEnable()
    {
        _meshRend = GetComponent<MeshRenderer>();
        _unitM = GetComponentInParent<unit_Manager>();
        _unitM.Selected += OnSelect;
        _unitM.Deselected += OnDeselect;
        _meshRend.enabled = false;
    }

    private void OnSelect()
    {
        _meshRend.enabled = true;
    }

    private void OnDeselect()
    {
        _meshRend.enabled = false;
    }

    private void OnDisable()
    {
        _unitM.Selected -= OnSelect;
        _unitM.Deselected -= OnDeselect;
    }
}
