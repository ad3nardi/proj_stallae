using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ui_commandCon : OptimizedBehaviour
{
    private RectTransform cachedRectTransform;
    private Vector3 animPos;
    private Vector3 negAnimPos;

    [SerializeField] private float animTime;

    private float _abCooldownTime;
    private float _abCooldownTimer;

    [Header("Update Components")]
    [SerializeField] TextMeshProUGUI _missionName;
    [SerializeField] TextMeshProUGUI _abNameTMP;
    [SerializeField] TextMeshProUGUI _abCooldownTMP;

    private void Awake()
    {
        cachedRectTransform = GetComponent<RectTransform>();
        animPos = cachedRectTransform.localPosition;
        negAnimPos = new Vector3(animPos.x - 400f, animPos.y, animPos.z);
        cachedRectTransform.DOMove(negAnimPos, animTime);
    }

    private void OnEnable()
    {
        UpdateFields();
        
    }
    private void UpdateFields()
    {
        cachedRectTransform.DOMove(animPos, animTime);
        if (SelectionMan.Instance.SelectedUnits.Count > 0)
        {
            _missionName.text = SelectionMan.Instance.SelectedUnits[0]._cMission.ToString();
            _abNameTMP.text = SelectionMan.Instance.SelectedUnits[0]._abilityName;
            IAbility iAbility = (IAbility)SelectionMan.Instance.SelectedUnits[0].CachedGameObject.GetComponent<IAbility>() as IAbility;
            _abCooldownTime = iAbility.GetCooldownTime();
            _abCooldownTimer = iAbility.GetCooldownTimer();
        }
    }
    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (_abCooldownTimer >= 0)
        {
            _abCooldownTimer -= Time.deltaTime;
            _abCooldownTMP.text = Mathf.Floor(_abCooldownTimer).ToString();
        }
        else
            _abCooldownTMP.text = "Ready";

    }

    private void OnDisable()
    {
        cachedRectTransform.DOMove(animPos, animTime);

    }
}
