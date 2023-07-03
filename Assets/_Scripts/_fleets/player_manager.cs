using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;
using Pathfinding;

public class player_manager : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] private Camera _cam;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private LayerSet _layerSet;

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
    [SerializeField] private float _dragDelay;
    [SerializeField] private LayerMask _inputMask;

    [Header("UI")]
    [SerializeField] private RectTransform _selectionBoxUI;
    [SerializeField] private VisualEffect _moveVFX;
    [SerializeField] private GameObject _attackUI;
    [SerializeField] private GameObject _defaultUI;
    [SerializeField] private gui_radialMenu _radialMenu;

    [Header("Units")]
    [SerializeField] public List<unit_Manager> _allyUnits;
    [SerializeField] public List<unit_Manager> _selectedUnits;
    [SerializeField] public Transform _targetUnitT;
    [SerializeField] public int _targetUnitSS;

    //UNITY FUNCTIONS
    private void Awake()
    {
        _layerSet = Helpers.LayerSet;
        _cam = Helpers.Camera;
        _eventSystem = Helpers.EventSystem;
        _isAttackInput = false;
        _isAttackDragging = false;
        _boxSelectDragging = false;
    }
    private void Start()
    {
        _radialMenu = GetComponent<gui_radialMenu>();
        _selectedUnits = new List<unit_Manager>();
        _selectedUnits.Clear();
    }
    private void Update()
    {
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
        if (!_eventSystem.IsPointerOverGameObject())
        {
            Input_CheckSelection();
            _isMouseDown = true;
        }
        else
            return;
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

        if (Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit, _inputMask))
        {
            if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
            {
                Input_SelectBoxStart();
                manage_deselectAll();
                manage_addSelected(hit.transform.GetComponent<unit_Manager>());
                return;
            }
            else if (_selectedUnits.Count == 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
            {
                Input_SelectBoxStart();
                return;
            }
            else if (_selectedUnits.Count > 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
            {
                command_moveSelected(hit.point);
                return;
            }
            else if (_selectedUnits.Count > 0 && hit.transform.gameObject.layer == _layerSet.layerEnemyUnit)
            {
                _targetUnitT = hit.transform.GetComponent<Transform>();
                Input_AttackDragStart();
                return;
            }
            else
            {
                manage_deselectAll();
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

        manage_deselectAll();
            for (int i = 0; i < _allyUnits.Count; i++)
            {
                if (UnitsInSelectionBox(_cam.WorldToScreenPoint(_allyUnits[i].transform.position), bounds))
                {
                    manage_addSelected(_allyUnits[i].transform.GetComponent<unit_Manager>());
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
        _targetUnitSS = _radialMenu.CheckRadialMenu(_mousePos.x, _mousePos.y);

    }
    private void Input_AttackDragRelease()
    {
        if(_isAttackDragging)
            command_manualTargetSubSystem(_targetUnitT, _targetUnitSS);
        else
            command_autoTargetSubSystem(_targetUnitT);

        _isAttackDragging = false;
        _isAttackInput = false;
        _attackUI.SetActive(false);
        _defaultUI.SetActive(true);
    }
    
    //UNIT MANAGEMENT FUNCTIONS
    public void manage_addSelected(unit_Manager unitM)
    {
        _selectedUnits.Add(unitM);

        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].UnitSelected();
        }
    }
    public void manage_deselectAll()
    {
        if (_selectedUnits.Count == 0)
            return;
        else
        {
            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                _selectedUnits[i].UnitDeselected();
            }
            _selectedUnits.Clear();
        }
    }

    //UNIT COMMAND FUNCTIONS
    private void command_moveSelected(Vector3 hitPoint)
    {
        //Move all selected units to target location
        _moveVFX.transform.position = hitPoint;
        _moveVFX.Play();

        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].mission_move(command_moveMath(hitPoint, i));
            
        }
        manage_deselectAll();  
    }
    private void command_manualTargetSubSystem(Transform targetUnitT, int targetSS)
    {
        unit_Manager targetUnitM = targetUnitT.GetComponent<unit_Manager>();
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].mission_attack(targetUnitM, targetSS, command_moveMath(targetUnitT.position, i));
        }
        manage_deselectAll();
    }
    private void command_autoTargetSubSystem(Transform targetUnitT)
    {

        //Choose random sub-system from target's list -ssi- if not designated
        unit_Manager targetUnitM = targetUnitT.GetComponent<unit_Manager>();
        _targetUnitSS = UnityEngine.Random.Range(0, 2);
        
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].mission_attack(targetUnitM, _targetUnitSS, command_moveMath(targetUnitT.position, i));
        }
        manage_deselectAll();
    }
    private Vector3 command_moveMath(Vector3 hitPoint, int i)
    {
        float radsum = 0;
        for (int r = 0; r < _selectedUnits.Count; r++)
        {
            radsum += _selectedUnits[r].transform.GetComponent<RichAI>().radius;
        }

        float radius = radsum / (Mathf.PI);
        radius *= 2f;

        float deg = 2 * Mathf.PI * i / _selectedUnits.Count;
        Vector3 p = hitPoint + new Vector3(Mathf.Cos(deg), 0, Mathf.Sin(deg)) * radius;

        return p;
    }
}