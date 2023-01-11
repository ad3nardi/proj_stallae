using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.RVO;

public class player_manager : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerSet _layerSet;
    [Header("Dynamic (do not set)")]
    [SerializeField] private Transform _camPos;
    [SerializeField] private Vector3 _selectPos;

    [Header("Units")]
    [SerializeField] public List<unit_Manager> _selectedUnits;

    private void Awake()
    {
        _cam = Camera.main;
        _camPos = _cam.transform;
        
    }
    private void Start()
    {
        _selectedUnits = new List<unit_Manager>();
        _selectedUnits.Clear();
    }

    public void InputSelectPos(float x, float y)
    {
        _selectPos = _cam.ScreenToViewportPoint(new Vector3( x, y, 0));
    }

    public void OnDrawGizmos()
    {
        Physics.Raycast(_cam.ViewportPointToRay(_selectPos));
    }

    public void InputSelect(bool inp)
    {
        RaycastHit hit;

        if(Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit))
        {
            if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
            {
                Deselect();
                AddSelect(hit.transform.GetComponent<unit_Manager>());
                return;
            }
            else if (_selectedUnits.Count != 0 && hit.transform.gameObject.layer == _layerSet.layerNavigation)
            {
                CommandMoveSelected(hit.point);
                return;
            }
            else
            {
                Deselect();
                return;
            }
        }
    }

    public void AddSelect(unit_Manager unitM)
    {
        _selectedUnits.Add(unitM);
        
        for (int i = 0; i<_selectedUnits.Count; i++)
        {   
            _selectedUnits[i].UnitSelected();
        }
    }

    public void CommandMoveSelected(Vector3 hitPoint)
    {
        float radsum = 0;
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            radsum += _selectedUnits[i].transform.GetComponent<RVOController>().radius;
        }

        float radius = radsum / (Mathf.PI);
        radius *= 2f;

        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            float deg = 2 * Mathf.PI * i / _selectedUnits.Count;
            Vector3 p = hitPoint + new Vector3(Mathf.Cos(deg), 0, Mathf.Sin(deg)) * radius;

            _selectedUnits[i].SetMoveTarget(p);
        }
        Deselect();
    }

    public void CommandAttackSelected()
    {

    }


    public void Deselect()
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
}