using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ab_spawnSquadron : OptimizedBehaviour, IAbility
{
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private int _squadCount;
    [SerializeField] private int _activeSquadCount;

    [SerializeField] private float _abCooldownTime;
    [SerializeField] private float _abCooldownTimer;

    [SerializeField] private List<GameObject> _squadronList;

    [SerializeField] private OptimizedBehaviour _uiSelect;

    private void Awake()
    {
        _unitM.GetComponent<unit_Manager>();
        _unitM._iThisAbility = this;
        _abCooldownTime = _unitM._unit.abCooldownTime;
        _abCooldownTimer = 0f;
        for (int i = 0; i < _squadronList.Count; i++)
        {
            _squadronList[i].SetActive(false);
        }
    }

    private void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (_abCooldownTimer <= 0) return;
        _abCooldownTimer -= Time.deltaTime;
    }


    public void Activate()
    {
        UIInteract();
        
    }

    private void UIInteract()
    {
        if (_abCooldownTimer <= 0 && _squadCount <= 0)
        {
            _squadCount--;
            _activeSquadCount++;
        }
    }

    public void Target()
    {

    }

    public void End()
    {
        _abCooldownTimer = _abCooldownTime;
    }
}
