using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ss_logic_hanger : unit_subsytems
{
    [Header("Plugins")]
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private unit_subsytems _unitSS;

    [Header("Hanger Stats")]
    [SerializeField] private float _sMaxSquadHold;
    [SerializeField] private float _sCurSquadHold;
    [SerializeField] private float _sRegenSquadRate;
    [SerializeField] private float _sRegenWaitTime;
    [SerializeField] private List<unit_Manager> _curSquads = new List<unit_Manager>();

    [Header("Destroyed Stats")]
    [SerializeField] private float _sDestroyedRegenRate;
    [SerializeField] private float _sDestroyedSquadMaxHold;
    [SerializeField] private float _sDestroyedRegenWaitTime;

    [Header("Hanger Status")]
    [SerializeField] private bool _isDestroyed;
    [SerializeField] private bool _isDisabled;
    [SerializeField] private bool _isRegenWaiting;

    [Header("Hanger Internal")]
    [SerializeField] private float _sRegenWaitTimer;
    [SerializeField] private float _sDeployTimer;

    [Header("Visual Display")]
    [SerializeField] private Image _squadDisplay;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
