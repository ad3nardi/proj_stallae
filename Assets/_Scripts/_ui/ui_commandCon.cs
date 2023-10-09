using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ui_commandCon : OptimizedBehaviour
{
    [SerializeField] private RectTransform cachedRectTransform;
    [SerializeField] private Vector3 animPos;
    [SerializeField] private Vector3 negAnimPos;

    [SerializeField] private OptimizedBehaviour _quickMenu;
    [SerializeField] private OptimizedBehaviour _moveMenu;
    [SerializeField] private OptimizedBehaviour _attackMenu;

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
        cachedRectTransform.position = negAnimPos;
    }

    public void quickMenu()
    {
        _quickMenu.CachedGameObject.SetActive(true);
        _moveMenu.CachedGameObject.SetActive(false);
        _attackMenu.CachedGameObject.SetActive(false);
    }

    public void moveMenu()
    {
        _quickMenu.CachedGameObject.SetActive(false);
        _moveMenu.CachedGameObject.SetActive(true);
        _attackMenu.CachedGameObject.SetActive(false);
    }

    public void attackMenu()
    {
        _quickMenu.CachedGameObject.SetActive(false);
        _moveMenu.CachedGameObject.SetActive(false);
        _attackMenu.CachedGameObject.SetActive(true);
    }
    public void ActIn()
    {
        cachedRectTransform.DOLocalMove(animPos, animTime);
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

    public void ActOut()
    {
        cachedRectTransform.DOLocalMove(negAnimPos, animTime);

    }
}
