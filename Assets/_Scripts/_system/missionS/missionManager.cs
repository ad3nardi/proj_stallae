using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManager : MonoBehaviour
{
    public missionStructure _activeMission;
    [SerializeField] private List<missionStructure> _missions = new List<missionStructure>();

    [Header("UI Settings")]
    [SerializeField] private GameObject _objParent;
    private Transform _tParent;

    [SerializeField] gui_capPoint _capPointUIPrefab;

    private Dictionary<ms_capPoints, gui_capPoint> _progressBars = new Dictionary<ms_capPoints, gui_capPoint>();

    private void Awake()
    {
        ms_capPoints.OnPctAdded += AddProgressBar;
        ms_capPoints.OnPctRemoved += RemoveProgressBar;
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