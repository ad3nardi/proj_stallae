using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class unit_highlight : OptimizedBehaviour
{
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private OptimizedBehaviour _selectUIobj;
    [SerializeField] private OptimizedBehaviour _atkRngUI;
    [SerializeField] private MeshRenderer _meshRend;
    [SerializeField] private Vector3 _atkRngUIAnimSize;


    private void Awake()
    {
        _unitM = GetComponentInParent<unit_Manager>();
        
    }
    private void Start()
    {
        _atkRngUIAnimSize = new Vector3(_unitM._unit.unitAttackRange, 1, _unitM._unit.unitAttackRange);
        _atkRngUI.CachedTransform.DOScale(Vector3.zero, 0.1f);
    }

    private void OnEnable()
    {
        _meshRend = _selectUIobj.GetComponent<MeshRenderer>();
        
        _unitM.Selected += OnSelect;
        _unitM.Deselected += OnDeselect;
        _meshRend.enabled = false;
    }

    private void OnSelect()
    {
        _meshRend.enabled = true;
        _atkRngUI.CachedTransform.DOScale(_atkRngUIAnimSize, 0.4f);
    }

    private void OnDeselect()
    {
        _meshRend.enabled = false;
        _atkRngUI.CachedTransform.DOScale(Vector3.zero, 0.1f);
    }

    private void OnDisable()
    {
        _unitM.Selected -= OnSelect;
        _unitM.Deselected -= OnDeselect;
        _atkRngUI.CachedGameObject.SetActive(false);
    }
}
