using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ab_spawnSquadron : OptimizedBehaviour, IAbility
{
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private int _maxSquadCount;
    [SerializeField] private int _curSquadCount;
    [SerializeField] private float _spawnMove;

    [SerializeField] private Vector3 _hangerPos;

    [SerializeField] private float _abCooldownTime;
    [SerializeField] private float _abCooldownTimer;

    [SerializeField] private List<GameObject> _squadronList = new List<GameObject>();


    private void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        _unitM._iThisAbility = this;
        _abCooldownTime = _unitM._unit.abCooldownTime;
        _abCooldownTimer = 0f;
        _curSquadCount = 0;
        for (int i = 0; i < _squadronList.Count; i++)
        {
            _squadronList[i].SetActive(false);
        }
        _maxSquadCount = _squadronList.Count;
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
        if (_abCooldownTimer <= 0 && _curSquadCount < _maxSquadCount)
        {
            Instantiate(_squadronList[_curSquadCount], _hangerPos, Quaternion.identity, _unitM._fleetHolder.CachedTransform);
            _curSquadCount++;
            _unitM.Talk("Launching Squadron!");
        }
        else
            _unitM.Talk("Out of squadrons!");
    }

    public void Target()
    {

    }

    public void End()
    {
        _abCooldownTimer = _abCooldownTime;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_hangerPos, Vector3.one*3);
    }
}
