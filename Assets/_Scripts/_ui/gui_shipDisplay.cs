using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gui_shipDisplay : Singleton<gui_shipDisplay>
{
    [SerializeField] private GameObject _objDisplayPanel;
    [SerializeField] private ui_guiCon _guiCon;

    public GameObject _objDisplayNameTxt;
    public GameObject _objStatusTxt;
    public GameObject _objABCooldownTxt;
    public GameObject _objSubTxt_1;
    public GameObject _objSubTxt_2;
    public GameObject _objSubTxt_3;
    public GameObject _objSubTxt_4;
    public GameObject _objSubTxt_5;
    public GameObject _objSubTxt_6;

    private TextMeshProUGUI _txtDisplayName;
    private TextMeshProUGUI _txtStatus;
    private TextMeshProUGUI _txtABCooldown;
    private TextMeshProUGUI _txtSub_1;
    private TextMeshProUGUI _txtSub_2;
    private TextMeshProUGUI _txtSub_3;
    private TextMeshProUGUI _txtSub_4;
    private TextMeshProUGUI _txtSub_5;
    private TextMeshProUGUI _txtSub_6;

    private void Start()
    {
        _txtDisplayName = _objDisplayNameTxt.GetComponent<TextMeshProUGUI>();
        _txtStatus = _objStatusTxt.GetComponent<TextMeshProUGUI>();
        _txtABCooldown = _objABCooldownTxt.GetComponent<TextMeshProUGUI>();
        _txtSub_1 = _objSubTxt_1.GetComponent<TextMeshProUGUI>();
        _txtSub_2 = _objSubTxt_2.GetComponent<TextMeshProUGUI>();
        _txtSub_3 = _objSubTxt_3.GetComponent<TextMeshProUGUI>();
        _txtSub_4 = _objSubTxt_4.GetComponent<TextMeshProUGUI>();
        _txtSub_5 = _objSubTxt_5.GetComponent<TextMeshProUGUI>();
        _txtSub_6 = _objSubTxt_6.GetComponent<TextMeshProUGUI>();

        _objDisplayPanel.SetActive(false);
    }

    public void ToggleDisplayPanel()
    {
        _objDisplayPanel.SetActive(!_objDisplayPanel.activeSelf);
    }
}
