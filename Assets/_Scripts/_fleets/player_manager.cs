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
    [SerializeField] public unit_Manager _targetUnitM;
    [SerializeField] public unit_subsytems _targetUnitSS;

    //UNITY FUNCTIONS
    private void Awake()
    {
        _cam = Camera.main;
        _attackDragging = false;
        _boxSelectDragging = false;
    }
    private void Start()
    {
        _selectedUnits = new List<unit_Manager>();
        _selectedUnits.Clear();
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
        Input_SelectBoxStart();
        Input_AttackDragStart();
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
            RaycastHit hit;
            if (Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit))
            {
                if (hit.transform.gameObject.layer == _layerSet.layerEnemyUnit)
                {
                    _attackDragStartPos = Vector3.zero;
                    _attackUI.SetActive(true);
                    _targetUnitM = hit.transform.GetComponent<unit_Manager>();

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

        command_autoTargetSubSystem();
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

        /*
        float angle = 60; // angular step
        int countOnCircle = (int)(360 / angle); // max number in one round
        int count = _selectedUnits.Count; // number of agents
        float step = 1; // circle number
        int i = 1; // agent serial number
        float randomizeAngle = Random.Range(0, angle);
        while (count > 1)
        {
            var vec = Vector3.forward;
            vec = Quaternion.Euler(0, angle * (countOnCircle - 1) + randomizeAngle, 0) * vec;
            _selectedUnits[i].missionMove(myAgent.destination + vec * (myAgent.radius + meshAgents[i].radius + 0.5f) * step);
            countOnCircle--;
            count--;
            i++;
            if (countOnCircle == 0)
            {
                if (step != 3 && step != 4 && step < 6 || step == 10) { angle /= 2f; }
                countOnCircle = (int)(360 / angle);
                step++;
                randomizeAngle = Random.Range(0, angle);
            }
        }
        */
    }
    private void command_autoTargetSubSystem()
    {
        //Choose random sub-system from target's list -ssi- if not designated
        int ssi = Random.Range(0, _targetUnitM._subsytems.Count);
        _targetUnitSS = _targetUnitM._subsytems[ssi];
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].mission_attack(_targetUnitSS);
        }
    }
}