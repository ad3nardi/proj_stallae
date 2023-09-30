using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ui_guiCon : OptimizedBehaviour
{
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public TagSet tagSet;

    [SerializeField] private OptimizedBehaviour _pauseMenu;
    [SerializeField] private GameObject _objGUI;
    [SerializeField] private GameObject _objSelectionBox;
    [SerializeField] private GameObject _objAttackUI;
    [SerializeField] private GameObject _objTimer;
    [SerializeField] private GameObject _objCommandCon;
    [SerializeField] private camera_con _cameraController;

    private TextMeshProUGUI _tmpTimer;

    private GameObject _pauseMenuGO;

    private OptimizedBehaviour _commandCon;
    private bool _isPaused;

    private void Awake()
    {
        layerSet = Helpers.LayerSet;
        tagSet = Helpers.TagSet;
        _isPaused = false;


        _tmpTimer = _objTimer.GetComponent<TextMeshProUGUI>();
        _commandCon = _objCommandCon.GetComponent<OptimizedBehaviour>();
    }

    private void Start()
    {
        _objGUI.SetActive(false);
        _objSelectionBox.SetActive(true);
        _objAttackUI.SetActive(false);
        _cameraController = _cameraController.GetComponent<camera_con>();
        _pauseMenuGO = _pauseMenu.GetComponent<OptimizedBehaviour>().CachedGameObject;
        man_cursor.Instance.ActivateMainCursor();

        _pauseMenuGO.gameObject.SetActive(false);

        _commandCon.CachedGameObject.SetActive(false);
    }
    private void Update()
    {
        UpdateTimer();
        
        if(SelectionMan.Instance.SelectedUnits.Count > 0)
        {
            if(!_commandCon.CachedGameObject.activeSelf)
                _commandCon.CachedGameObject.SetActive(true);
        }
        if (SelectionMan.Instance.SelectedUnits.Count <= 0)
        {
            if (_commandCon.CachedGameObject.activeSelf)
                _commandCon.CachedGameObject.SetActive(false);
        }

    }

    public void UpdateTimer()
    {

        _tmpTimer.text = Mathf.Floor(Time.time).ToString();
    }

    public void PauseMenuToggle()
    {
        if (_pauseMenuGO != null)
        {
            _pauseMenuGO.SetActive(!_pauseMenuGO.activeSelf);
            _isPaused = _pauseMenuGO.activeSelf;
            PauseTime();
        }
    }

    private void PauseTime()
    {
        if (_isPaused)
        {
            _cameraController.enabled = false;
            Time.timeScale= 0.0f;

        }

        if (!_isPaused)
        {
            _cameraController.enabled = true;
            Time.timeScale = 1.0f;
        }
    }

    public void SelectedCard()
    {

    }

    public void CardSelect()
    {

    }
}
