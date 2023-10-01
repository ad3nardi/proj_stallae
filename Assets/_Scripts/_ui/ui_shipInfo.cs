using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ui_shipInfo : OptimizedBehaviour
{
    [Header("Update Components")]
    [SerializeField] private TextMeshProUGUI tmp_shipName;
    [SerializeField] private TextMeshProUGUI tmp_ssHp_hull;
    [SerializeField] private TextMeshProUGUI tmp_ssHp_weapon1;
    [SerializeField] private TextMeshProUGUI tmp_ssHp_weapon2;
    [SerializeField] private TextMeshProUGUI tmp_ssHp_shields;
    [SerializeField] private TextMeshProUGUI tmp_ssHp_engine;
    [SerializeField] private TextMeshProUGUI tmp_ssHp_utility;

    private RectTransform cachedRectTransform;
    private Vector3 animPos;
    private Vector3 negAnimPos;

    [SerializeField] private float animTime;


    private void Awake()
    {
        cachedRectTransform = GetComponent<RectTransform>();
        animPos = cachedRectTransform.localPosition;
        negAnimPos = new Vector3(animPos.x - 400f, animPos.y, animPos.z);
        cachedRectTransform.DOMove(negAnimPos, animTime);

        tmp_shipName = tmp_shipName.GetComponent<TextMeshProUGUI>();
        tmp_ssHp_hull = tmp_ssHp_hull.GetComponent<TextMeshProUGUI>();
        tmp_ssHp_weapon1 = tmp_ssHp_weapon1.GetComponent<TextMeshProUGUI>();
        tmp_ssHp_weapon2 = tmp_ssHp_weapon2.GetComponent<TextMeshProUGUI>();
        tmp_ssHp_shields = tmp_ssHp_shields.GetComponent<TextMeshProUGUI>();
        tmp_ssHp_engine = tmp_ssHp_engine.GetComponent<TextMeshProUGUI>();
        tmp_ssHp_utility = tmp_ssHp_utility.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        cachedRectTransform.DOMove(animPos, animTime);
        if (SelectionMan.Instance.SelectedUnits.Count > 0)
        {
            ITargetable iTarget = (ITargetable)SelectionMan.Instance.SelectedUnits[0].CachedGameObject.GetComponent<ITargetable>() as ITargetable;
            string name = SelectionMan.Instance.SelectedUnits[0]._unit.title;
            UpdateDetails(name, iTarget.GetHP());
        }
    }

    public void UpdateDetails(string name, float[] unitHP)    
    {
        tmp_shipName.text = name;
        tmp_ssHp_hull.text = "Hull: " + unitHP[0].ToString();
        tmp_ssHp_weapon1.text = "Weapon 1: " + unitHP[1].ToString();
        tmp_ssHp_weapon2.text = "Weapon 2: " + unitHP[2].ToString();
        tmp_ssHp_shields.text = "Shields: " + unitHP[3].ToString();
        tmp_ssHp_engine.text = "Engine: " + unitHP[4].ToString();
        tmp_ssHp_utility.text = "Utility: " + unitHP[5].ToString();
    }
    private void OnDisable()
    {
        cachedRectTransform.DOMove(negAnimPos, animTime);
        
    }
}
