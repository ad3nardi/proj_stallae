using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class missionManager : OptimizedBehaviour
{
    [SerializeField] private ms_capPoints _capPoint1;
    [SerializeField] private ms_capPoints _capPoint2;
    [SerializeField] private ms_capPoints _capPoint3;

    [SerializeField] private TextMeshProUGUI _tmpText;
    [SerializeField] private int _takenNo;

    [Header("Gates for 1")]
    [SerializeField] private OptimizedBehaviour _gateOne;
    [SerializeField] private OptimizedBehaviour _gateTwo;
    [SerializeField] private float _gateTime;

    [Header("Gates for 2")]
    [SerializeField] private OptimizedBehaviour _gateThree;
    [SerializeField] private OptimizedBehaviour _gateFour;

    [SerializeField] private unit_Manager _valeria;

    public ui_guiCon uiCon;

    public bool isWin;
    public bool isLose;


    private void Awake()
    {
        
        _takenNo = 0;
        _capPoint1.OnCapturePointTaken += UpdateTaken1;
        _capPoint2.OnCapturePointTaken += UpdateTaken2;
        _capPoint3.OnCapturePointTaken += UpdateTaken3;
        UpdateText();

        isWin = false;
        isLose = false;

        


    }

    private void UpdateText()
    {
        _tmpText.text = "Secure both relay areas: " + _takenNo.ToString() + "/2";
        _takenNo++;

        if (_takenNo >= 2)
            _tmpText.text = "Escape the foundry.";

        CheckWinLose();
    }
    private void CheckWinLose()
    {
        if(_takenNo == 3)
        {
            isWin=true;
            uiCon.WinScreen();
        }

        if (_valeria._isDestoryed)
        {
            uiCon.LoseScreen();
        }

    }
    private void UpdateTaken1(bool b)
    {
        if (b)
        {
            UpdateText();
            _gateOne.CachedTransform.DOMoveZ(_gateOne.CachedTransform.position.z + 65f, _gateTime);
            _gateTwo.CachedTransform.DOMoveZ(_gateOne.CachedTransform.position.z - 65f, _gateTime);

        }
    }
    private void UpdateTaken2(bool b)
    {
        if (b)
        {
            UpdateText();
            _gateThree.CachedTransform.DOMoveZ(_gateOne.CachedTransform.position.z - 65f, _gateTime);
            _gateFour.CachedTransform.DOMoveZ(_gateOne.CachedTransform.position.z - 65f, _gateTime);

        }

    }
    private void UpdateTaken3(bool b)
    {

    }
}

public enum missionType
{
    none,
    valeria,
    capPoint,
    capArea,
    defendObj,
    hunt,
    koth
}

/*
public missionStructure _activeMission;
[SerializeField] private List<missionStructure> _missions = new List<missionStructure>();

[Header("UI Settings")]
[SerializeField] private GameObject _objParent;
private Transform _tParent;

[SerializeField] gui_capPoint _capPointUIPrefab;

private Dictionary<ms_capPoints, gui_capPoint> _progressBars = new Dictionary<ms_capPoints, gui_capPoint>();

private void Awake()
{
    //ms_capPoints.OnPctAdded += AddProgressBar;
    //ms_capPoints.OnPctRemoved += RemoveProgressBar;
    _tParent = _objParent.GetComponent<Transform>();

    if (_missions.Count >= 1)
    {
        _activeMission = _missions[0];
    }
}

private void AddProgressBar(ms_capPoints pctTaken)
{
    if (_progressBars.ContainsKey(pctTaken) == false)
    {
        var progressBar = Instantiate(_capPointUIPrefab, _tParent);
        _progressBars.Add(pctTaken, progressBar);
        progressBar.SetProgress(pctTaken);
    }
}

private void RemoveProgressBar(ms_capPoints pctTaken)
{
    if (_progressBars.ContainsKey(pctTaken))
    {
        Destroy(_progressBars[pctTaken].gameObject);
        _progressBars.Remove(pctTaken);
    }
}
*/
