using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Pathfinding;

public class player_manager : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerSet _layerSet;

    [Header("Inputs (DO NOT SET IN EDITOR)")]
    [SerializeField] private Vector2 _mousePos;
    [SerializeField] private Vector2 _startMousePos;
    [SerializeField] private Vector3 _selectPos;
    [SerializeField] private Vector3 _attackDragStartPos;
    [SerializeField] private Vector3 _attackDragEndPos;
    [SerializeField] private float _mouseDownTime;
    //Bools to check input Hold
    [SerializeField] private bool _attackDragging;
    [SerializeField] private bool _boxSelectDragging;

    [Header("Input Modifications")]
    [SerializeField] private float _dragDelay;

    [Header("UI")]
    [SerializeField] private RectTransform _selectionBoxUI;
    [SerializeField] private VisualEffect _moveVFX;
    [SerializeField] private GameObject _attackUI;

    [Header("Units")]
    [SerializeField] public List<unit_Manager> _allyUnits;
    [SerializeField] public List<unit_Manager> _selectedUnits;
    [SerializeField] public unit_subsytems _targetUnitSS;

    //UNITY FUNCTIONS
    private void Awake()
    {
        _cam = Helpers.Camera;
        _attackDragging = false;
        _boxSelectDragging = false;
    }
    private void Start()
    {
        _selectedUnits = new List<unit_Manager>();
        _selectedUnits.Clear();
        _targetUnitSS = null;
    }
    private void Update()
    {
        if (_attackDragging)
            Input_AttackDragging();
        if (_boxSelectDragging && _mouseDownTime + _dragDelay > Time.time)
            Input_SelectBoxResize();

        UpdateMouseDownTimer();

    }

    public void OnDrawGizmos()
    {
        Physics.Raycast(_cam.ViewportPointToRay(_selectPos));
    }

    //PLAYER INPUT FUNCTIONS
    public void InputSelect(bool inp)
    {
        Input_AttackDragStart();
        Input_SelectBoxStart();
    }
    public void InputRelease(bool inp)
    {
        Input_AttackDragRelease();
        Input_SelectBoxRelease();
    }
    public void InputSelectPos(float x, float y)
    {
        _mousePos = new Vector2(x, y);
        _selectPos = _cam.ScreenToViewportPoint(new Vector3(x, y, 0));
    }
    private void UpdateMouseDownTimer()
    {
        _mouseDownTime = Time.time;
    }
    
    //UNIT INPUT SELECTION FUNCTIONS
    private void Input_SelectBoxStart()
    {
        _boxSelectDragging = true;
        _startMousePos = _mousePos;
        _selectionBoxUI.sizeDelta = Vector2.zero;
        _selectionBoxUI.gameObject.SetActive(true);

        RaycastHit hit;
        if (Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit))
        {
            if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
            {
                manage_deselectAll();
                manage_addSelected(hit.transform.GetComponent<unit_Manager>());
                return;
            }
            else if (_selectedUnits.Count != 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
            {
                command_moveSelected(hit.point);
                return;
            }
            else
            {
                manage_deselectAll();
                return;
            }
        }
    }
    private void Input_SelectBoxResize()
    {
        float width = _mousePos.x - _startMousePos.x;
        float height = _mousePos.y - _startMousePos.y;

        //Canvas Scaler -> Constant Pixel Size. Have Canvas specifically for SelectionBox
        _selectionBoxUI.anchoredPosition = _startMousePos + new Vector2(width / 2, height / 2);
        _selectionBoxUI.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        Bounds bounds = new Bounds(_selectionBoxUI.anchoredPosition, _selectionBoxUI.sizeDelta);

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
        _boxSelectDragging = false;
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
        if (_selectedUnits.Count != 0)
        {
            Debug.Log("Input Attack Sucess");
            RaycastHit hit;
            if (Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit))
            {
                if (hit.transform.gameObject.layer == _layerSet.layerEnemyUnit)
                {
                    Debug.Log("Attack: " + hit);
                    _attackDragStartPos = Vector3.zero;
                    _attackUI.SetActive(true);
                    command_autoTargetSubSystem(hit.transform);

                    _attackDragging = true;
                    return;
                }
                else if (_selectedUnits.Count != 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
                {
                    command_moveSelected(hit.point);
                    return;
                }
                else
                    return;
            }
        }
        else
            return;
    }
    private void Input_AttackDragging()
    {
        Vector3 draggingPoint = _mousePos;// _cam.ScreenToWorldPoint(_selectPos);

        Debug.Log("Drag Point " + draggingPoint);
    }
    private void Input_AttackDragRelease()
    {
        _attackDragEndPos = _selectPos;
        _attackDragging = false;

        Debug.Log("End Pos " + _attackDragEndPos);
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
        float radsum = 0;
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            radsum += _selectedUnits[i].transform.GetComponent<RichAI>().radius;
        }

        float radius = radsum / (Mathf.PI);
        radius *= 2f;

        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            float deg = 2 * Mathf.PI * i / _selectedUnits.Count;
            Vector3 p = hitPoint + new Vector3(Mathf.Cos(deg), 0, Mathf.Sin(deg)) * radius;

            _selectedUnits[i].mission_move(p);
        }

        manage_deselectAll();  
    }
    private void command_autoTargetSubSystem(Transform targetUnitT)
    {
        //Choose random sub-system from target's list -ssi- if not designated
        
        unit_Manager targetUnitM = targetUnitT.GetComponent<unit_Manager>();
        int ssi = Random.Range(0, targetUnitM._subsytems.Count);
        _targetUnitSS = targetUnitM._subsytems[ssi];
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].mission_attack(targetUnitT, _targetUnitSS);
        }
    }
}