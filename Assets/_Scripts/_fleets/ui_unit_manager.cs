using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_unit_manager : OptimizedBehaviour
{
    private SelectionMan _selectionMan;
    [SerializeField] private camera_con _camCon;
    //UI FUNCTIONS
    private void Awake()
    {
        _selectionMan = SelectionMan.Instance;
    }
    public void uiSetStop()
    {
        int count = _selectionMan.SelectedUnits.Count;

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                _selectionMan.SelectedUnits[i].mission_stop();
                _selectionMan.DeselectAll();
            }
        }
        else
            return;
    }

    public void uiSetGuard()
    {
        int count = _selectionMan.SelectedUnits.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                _selectionMan.SelectedUnits[i].missoin_guard();
                _selectionMan.DeselectAll();
            }
        }
        else
            return;
    }

    public void uiSetAutoFire()
    {
        int count = _selectionMan.SelectedUnits.Count;

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                _selectionMan.SelectedUnits[i].set_autoFire();
                _selectionMan.DeselectAll();
            }
        }
        else
            return;
    }

    public void uiFrameCameraOnTarget()
    {
        if(SelectionMan.Instance.SelectedUnits.Count > 0)
        {
            _camCon.FrameCamera(SelectionMan.Instance.SelectedUnits[0]);
        }
    }

    public void uiAbilityActivate()
    {
        if (SelectionMan.Instance.SelectedUnits.Count > 0)
        {
            SelectionMan.Instance.SelectedUnits[0].ActivateAbility();
        }
    }
}
