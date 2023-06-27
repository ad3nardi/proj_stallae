using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private bool _failed;

    [SerializeField] private GameObject PcShip;
    [SerializeField] private OptimizedBehaviour _pcShip;

    [SerializeField] private ui_guiCon _guiCon;

    [SerializeField] private List<Transform> _defendList = new List<Transform>();


    private void Start()
    {
        _failed = false;
        _pcShip = PcShip.GetComponent<OptimizedBehaviour>();
        _guiCon = GetComponent<ui_guiCon>();
    }

    private void MissionFailed()
    {
        //_guiCon.missionFailed();
    }
}
