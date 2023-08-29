using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Settings/Player Manager Settings")]
public class pm_settings : ScriptableObject
{
    [Header("Input Settings")]
    [SerializeField] private float _dragDelay;
    [SerializeField] private LayerMask _inputMask;

    [Header("Prefabs To Load Settings")]
    [SerializeField] private RectTransform _selectionBoxUI;
    [SerializeField] private VisualEffect _moveVFX;
    [SerializeField] private GameObject _attackUI;
    [SerializeField] private GameObject _defaultUI;
    [SerializeField] private gui_radialMenu _radialMenu;
}
