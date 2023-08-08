using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_selectable : MonoBehaviour, ISelectable
{
    public void Select()
    {
        GetInfo();
    }

    public void Deselect()
    {
        GetInfo();
    }

    public void GetInfo()
    {

    }

}
