using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_unit_manager : OptimizedBehaviour
{
    [SerializeField] private player_manager _pm;
    //Unity Function
    private void Start()
    {
        if (_pm == null)
            _pm = GetComponent<player_manager>();
        else
            return;
    }

    //UI FUNCTIONS
    public void uiSetStop()
    {
        if (_pm._selectedUnits.Count != 0)
        {
            for (int i = 0; i < _pm._selectedUnits.Count; i++)
            {
                _pm._selectedUnits[i].mission_stop();
                _pm.manage_deselectAll();
            }
        }
        else
            return;
    }

    public void uiSetGuard()
    {
        if (_pm._selectedUnits.Count != 0)
        {
            for (int i = 0; i < _pm._selectedUnits.Count; i++)
            {
                _pm._selectedUnits[i].missoin_guard();
            }
        }
        else
            return;
    }
}
