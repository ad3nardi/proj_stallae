using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMan
{
    private static SelectionMan _instance;
    public static SelectionMan Instance
    {
        get
        { 
            if(_instance == null)
            {
                _instance = new SelectionMan();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    public List<unit_Manager> SelectedUnits = new List<unit_Manager>();
    public List<unit_Manager> AvaliableUnits = new List<unit_Manager>();

    public unit_Manager valeria;
    private SelectionMan() { }
    public void Select(unit_Manager unit)
    {
        SelectedUnits.Add(unit);
        unit.Select();
    }
    public void Deselect(unit_Manager unit)
    {
        unit.Deselect();
        SelectedUnits.Remove(unit);
    }
    public void DeselectAll()
    {
        for (int i = 0; i < SelectedUnits.Count; i++)
        {
            SelectedUnits[i].Deselect();
        }
        SelectedUnits.Clear();
    }
    public void SelectAll()
    {
        DeselectAll();
        for (int i = 0; i < AvaliableUnits.Count; i++)
        {
            if (AvaliableUnits[i] == valeria)
            {
                continue;
            }
            Select(AvaliableUnits[i]);
        }
    }
    public bool IsSelected(unit_Manager unit)
    {
        return SelectedUnits.Contains(unit);
    }
}
