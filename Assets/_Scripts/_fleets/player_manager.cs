using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_manager : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] public unit_Manager unit;
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerSet _layerSet;
    [Header("Dynamic (do not set)")]
    [SerializeField] private Transform _camPos;
    [SerializeField] private Vector3 _selectPos;

    [Header("Units")]
    [SerializeField] public Transform _selectedUnit;

    private void Awake()
    {
        _cam = Camera.main;
        _camPos = _cam.transform;
        unit = null;
        _selectedUnit = null;
    }

    public void InputSelectPos(float x, float y)
    {
        _selectPos = _cam.ScreenToViewportPoint(new Vector3(x, y, 0));
    }

    public void InputSelect(bool inp)
    {
        Debug.Log("select");

        Deselect();
        RaycastHit hit;
        if(Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit))
        {
            if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
            {
                _selectedUnit = hit.transform;
                unit = _selectedUnit.GetComponent<unit_Manager>();
                unit.UnitSelected();
            }
            else
                return;
        }
    }

    public void CommandSelected()
    {
        RaycastHit hit;
        Physics.Raycast(_cam.ViewportPointToRay(_selectPos), out hit);
        if (unit != null)
        {
            if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
            {

            }
            else if (hit.transform.gameObject.layer == _layerSet.layerPlayerUnit)
            {

            }
            else
                return;
        }
        else
            return;
    }

    public void Deselect()
    {
        if (unit == null)
            return;
        else
        {
            unit.UnitDeselected();
            unit = null;
        }
        
    }
}