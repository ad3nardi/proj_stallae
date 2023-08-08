using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;
using Pathfinding;
using Pathfinding.ClipperLib;

public class player_manager : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private ISelectable _selectable;

    [Header("Inputs (DO NOT SET IN EDITOR)")]
    [SerializeField] private Vector2 _mousePos;
    [SerializeField] private Vector2 _startMousePos;
    [SerializeField] private Vector3 _selectPos;
    [SerializeField] private float _mouseDownTime;

    //Bools to check input Hold
    [SerializeField] private bool _isMouseDown;
    [SerializeField] private bool _isAttackInput;
    [SerializeField] private bool _isAttackDragging;
    [SerializeField] private bool _boxSelectDragging;

    [Header("Input Modifications")]
    [SerializeField] private pm_settings _settings;
    [SerializeField] private float _dragDelay;
    [SerializeField] private LayerMask _inputMask;

    [Header("UI")]
    [SerializeField] private bool _isOverUI;
    [SerializeField] private RectTransform _selectionBoxUI;
    [SerializeField] private VisualEffect _moveVFX;
    [SerializeField] private GameObject _attackUI;
    [SerializeField] private GameObject _defaultUI;
    [SerializeField] private gui_radialMenu _radialMenu;

    [Header("Units")]
    [SerializeField] public Transform _targetUnitT;
    [SerializeField] public int _targetUnitSS;

    //UNITY FUNCTIONS
    private void Awake()
    {
        _layerSet = Helpers.LayerSet;
        _cam = Helpers.Camera;
        _settings = settingsHolder.PMSettings;
        _selectable = null;
        _isAttackInput = false;
        _isAttackDragging = false;
        _boxSelectDragging = false;
    }
    private void Start()
    {
        _radialMenu = GetComponent<gui_radialMenu>();

    }
    private void Update()
    {
        _isOverUI = Helpers.EventSystem.IsPointerOverGameObject();
        if (_isAttackInput && _mouseDownTime > _dragDelay)
            _isAttackDragging = true;
        else if (_mouseDownTime > _dragDelay)
            _boxSelectDragging = true;
        
        if (_isMouseDown) UpdateMouseDownTimer();
        if (_isAttackDragging) Input_AttackDragging();
        if (_boxSelectDragging) Input_SelectBoxResize();
    }

    //PLAYER INPUT FUNCTIONS
    public void InputSelect(bool inp)
    {
        Input_CheckSelection();
        _isMouseDown = true;
    }
    public void InputRelease(bool inp)
    {
        if (_isAttackInput)
        {
            Input_AttackDragRelease();
        }

        Input_SelectBoxRelease();

        _isMouseDown = false;
        _boxSelectDragging = false;
        _mouseDownTime = 0;
    }
    public void InputSelectPos(float x, float y)
    {
        _mousePos = new Vector2(x, y);
        _selectPos = _cam.ScreenToViewportPoint(new Vector3(x, y, 0));
    }

    private void UpdateMouseDownTimer()
    {
        _mouseDownTime += Time.deltaTime;
    }

    //UNIT INPUT SELECTION FUNCTIONS
    private void Input_CheckSelection()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit, Mathf.Infinity, _inputMask))
        {
            
            _selectable = hit.transform.GetComponent<ISelectable>();
            {
                if(_selectable != null) { }

            }
                if (hit.transform.gameObject.layer == _layerSet.layerUI)
                {
                    return;
                }
                else if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
                {
                    Input_SelectBoxStart();

                    SelectionMan.Instance.DeselectAll();
                    SelectionMan.Instance.Select(hit.transform.GetComponent<unit_Manager>());

                    return;
                }
                else if (SelectionMan.Instance.SelectedUnits.Count == 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
                {
                    Input_SelectBoxStart();
                    _selectable = null;
                    return;
                }
                else if (SelectionMan.Instance.SelectedUnits.Count > 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
                {
                    command_moveSelected(hit.point);
                    _selectable = null;
                    return;
                }
                else if (SelectionMan.Instance.SelectedUnits.Count > 0 && hit.transform.gameObject.layer == _layerSet.layerEnemyUnit)
                {
                    _targetUnitT = hit.transform.GetComponent<OptimizedBehaviour>().CachedTransform;
                    Input_AttackDragStart();
                    _selectable = null;
                    return;
                }
                else
                {
                    _selectable = null;
                    SelectionMan.Instance.DeselectAll();
                    return;
                }
            
        }
    }
    private void Input_SelectBoxStart()
    {
        _startMousePos = _mousePos;
        _selectionBoxUI.sizeDelta = Vector2.zero;
        _selectionBoxUI.gameObject.SetActive(true);     
    }
    private void Input_SelectBoxResize()
    {
        float width = _mousePos.x - _startMousePos.x;
        float height = _mousePos.y - _startMousePos.y;

        //Canvas Scaler -> Constant Pixel Size. Have Canvas specifically for SelectionBox
        _selectionBoxUI.anchoredPosition = _startMousePos + new Vector2(width / 2, height / 2);
        _selectionBoxUI.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        Bounds bounds = new Bounds(_selectionBoxUI.anchoredPosition, _selectionBoxUI.sizeDelta);

        SelectionMan.Instance.DeselectAll();        
        for (int i = 0; i < SelectionMan.Instance.AvaliableUnits.Count; i++)
        {
            if (UnitsInSelectionBox(_cam.WorldToScreenPoint(SelectionMan.Instance.AvaliableUnits[i].transform.position), bounds))
            {
                SelectionMan.Instance.Select(SelectionMan.Instance.AvaliableUnits[i]);
            }
            else
            {
                SelectionMan.Instance.Deselect(SelectionMan.Instance.AvaliableUnits[i]);
            }
        }
    }
    private void Input_SelectBoxRelease()
    {
        _mouseDownTime = 0f;
        _selectionBoxUI.sizeDelta = Vector2.zero;
        _selectionBoxUI.gameObject.SetActive(false);
    }
    private bool UnitsInSelectionBox(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x && position.x < bounds.max.x && position.y > bounds.min.y && position.y < bounds.max.y;
    }
    
    //UNIT INPUT ATTACK FUNCTIONS
    private void Input_AttackDragStart()
    {
        _isAttackInput = true;
        _defaultUI.SetActive(false);
        _attackUI.SetActive(true);
    }
    private void Input_AttackDragging()
    {
        _targetUnitSS = _radialMenu.CheckRadialMenu(_targetUnitT.GetComponent<unit_Manager>(), _mousePos.x, _mousePos.y);

    }
    private void Input_AttackDragRelease()
    {
        if(_isAttackDragging)
            command_manualTargetSubSystem(_targetUnitT, _targetUnitSS);
        else
            command_autoTargetSubSystem(_targetUnitT);

        _isAttackInput = false;
        _isAttackDragging = false;
        _attackUI.SetActive(false);
        _defaultUI.SetActive(true);
    }
    
    //UNIT COMMAND FUNCTIONS
    private void command_moveSelected(Vector3 hitPoint)
    {
        //Move all selected units to target location
        _moveVFX.transform.position = hitPoint;
        _moveVFX.Play();
        
        
        for (int i = 0; i < SelectionMan.Instance.SelectedUnits.Count; i++)
        {
            SelectionMan.Instance.SelectedUnits[i].mission_move(hitPoint, i, SelectionMan.Instance.SelectedUnits.Count);
        }
        
        SelectionMan.Instance.DeselectAll();  
    }
    private void command_manualTargetSubSystem(Transform targetUnitT, int targetSS)
    {
        unit_Manager targetUnitM = targetUnitT.GetComponent<unit_Manager>();

        for (int i = 0; i < SelectionMan.Instance.SelectedUnits.Count; i++)
        {
            SelectionMan.Instance.SelectedUnits[i].mission_attack(targetUnitM, targetSS, targetUnitT.position, i, SelectionMan.Instance.SelectedUnits.Count);
        }

        SelectionMan.Instance.DeselectAll();
    }
    private void command_autoTargetSubSystem(Transform targetUnitT)
    {

        //Choose random sub-system from target's list -ssi- if not designated
        unit_Manager targetUnitM = targetUnitT.GetComponent<unit_Manager>();

        targetUnitM.GetStatusSS += GetTargetInfo;
        targetUnitM.GetInfoSS();

        targetUnitM.GetStatusSS -= GetTargetInfo;
    }

    public void GetTargetInfo(unit_Manager target, float shipHP, bool[] actSS, float[] ssHP)
    {
        while (actSS[_targetUnitSS] != true)
        {
            _targetUnitSS = UnityEngine.Random.Range(0, 5);
            if (actSS[_targetUnitSS] == true)
            {
                break;
            }
            else
                continue;
        }
        for (int i = 0; i < SelectionMan.Instance.SelectedUnits.Count; i++)
        {
            SelectionMan.Instance.SelectedUnits[i].mission_attack(target, _targetUnitSS, target.CachedTransform.position, i, SelectionMan.Instance.SelectedUnits.Count);
        }
        SelectionMan.Instance.DeselectAll();
    }
}