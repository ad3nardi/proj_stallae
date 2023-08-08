using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class env_selectable : OptimizedBehaviour, ISelectable
{
    [SerializeField] private GameObject highlihgtGO;

    private void Awake()
    {
        highlihgtGO.SetActive(false);
    }

    public void Select()
    {
        highlihgtGO.SetActive(true);

    }
    public void Deselect()
    {
        highlihgtGO.SetActive(false);

    }
}
