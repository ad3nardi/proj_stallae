using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enem_unitMan : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public TagSet tagSet;
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] public unit_settings _unit;
    [SerializeField] public enem_battlegroupMan _bgMan;
    [SerializeField] public int _sizeTag;

    [Header("Mission Parameter")]
    [SerializeField] private OrdersBeh _orders;
    [SerializeField] private FlankState _flankState;
    [SerializeField] public Transform _targetT;
    [SerializeField] private unit_Manager _targetM;
    [SerializeField] private int _targetUnitSS;
    [SerializeField] private Vector3 _targetP;
    [SerializeField] private Vector3 _movePoint;
    [SerializeField] private float _movePointCheckBuffer;

    [Header("Engage - Flank Settings")]
    [SerializeField] private int _id;
    [SerializeField] private int _count;

    [Header("Engage - Flank Settings")]
    [Range(0, 1)][SerializeField] private float _flankPointClose = 0.5f;
    [SerializeField] private float _flankPointOffset = 15f;
    [SerializeField] private float _flankOffsetSide = 1;
    [SerializeField] private Vector3 _flankCheckSize = new Vector3(20, 5, 30);
    [SerializeField] private LayerMask _flankLayer;

    public void Awake()
    {
        _unitM = GetComponent<unit_Manager>();
        EnemyFleetMan.Instance.AvaliableUnits.Add(this);

        _unit = _unitM._unit;
        layerSet = Helpers.LayerSet;
        tagSet = Helpers.TagSet;
        _sizeTag = (int)_unit.sizeTag;
        _flankState = FlankState.None;
    }

    public void Update()
    {
        UpdateOrders();
        
    }

    public void UpdateOrdersType(OrdersBeh orderBeh)
    {
        _orders = orderBeh;
    }

    public void UpdateOrders()
    {
        if (_orders == OrdersBeh.Engage)
        {
            if (_flankState == FlankState.None)
            {
                
                EngageDirect();
            }
            if (_flankState == FlankState.FlankMove)
            {
                UpdateOffsetSide();
                EngageFlankMove();
            }
        }
    }

    public void SelectTarget(OptimizedBehaviour t)
    {
        _targetT = t.CachedTransform;
        _targetM = _targetT.GetComponent<unit_Manager>();
        _targetP = _targetT.position;
    }
    public void ClearTarget()
    {
        _targetT = null;
        _targetM = null;
        _targetP = Vector3.zero;
    }

    public void UnitMove(Vector3 pos, int i, int c)
    {
        _unitM.mission_move(pos, i, c);
    }
    public void UnitForceMove(Vector3 moveP)
    {
        _unitM.mission_forceMove(moveP);
    }
    public void Engage(OptimizedBehaviour target, int i, int c, FlankState flankQ)
    {
        SelectTarget(target);
        _id = i;
        _count = c;
        _flankState = flankQ;
        //Choose random sub-system from target's list -ssi- if not designated
        unit_Manager targetUnitM = target.GetComponent<unit_Manager>();

        targetUnitM.GetStatusSS += GetTargetInfo;
        targetUnitM.GetInfoSS();
        targetUnitM.GetStatusSS -= GetTargetInfo;
    }

    public void EngageDirect()
    {
        _unitM.mission_attack(_targetM, _targetUnitSS, _targetP, _id, _count);
    }

    public void GetTargetInfo(unit_Manager target, float shipHP, bool[] actSS, float[] ssHP)
    {
        while (actSS[_targetUnitSS] != true)
        {
            _targetUnitSS = UnityEngine.Random.Range(0, 5);
            if (actSS[_targetUnitSS] == true)
            {
                break;
            }
            else
                continue;
        }
    }


    public void EngageFlank(OptimizedBehaviour target, int i, int c)
    {
        SelectTarget(target);

        Vector3 midPoint = (CachedTransform.position + target.CachedTransform.position) * _flankPointClose;
        float offset = CheckOffsetSide();
        _flankOffsetSide = offset;
        Vector3 flankPoint = new Vector3(midPoint.x + _flankPointOffset * offset, 0, midPoint.z);
        _flankState = FlankState.FlankMove;
        _unitM.mission_move(flankPoint, i, c);
        _movePoint = flankPoint;
        _id = i;
        _count = c;

        unit_Manager targetUnitM = target.GetComponent<unit_Manager>();

        targetUnitM.GetStatusSS += GetTargetInfo;
        targetUnitM.GetInfoSS();
        targetUnitM.GetStatusSS -= GetTargetInfo;
    }

    private void EngageFlankMove()
    {
        /*
        float offset = CheckOffsetSide();
        if(_flankOffsetSide != offset)
        {
            _movePoint = new Vector3(_movePoint.x * offset, 0, _movePoint.z);
            _unitM.mission_move(_movePoint);
            _flankOffsetSide = offset;
        }
        */
        
        if (Vector3.Distance(CachedTransform.position, _movePoint) <= 0 + _movePointCheckBuffer)
        {
            _flankState = FlankState.FlankAttack;
            EngageFlankAttack();
        }

    }

    private void EngageFlankAttack()
    {
        if(_flankState == FlankState.FlankAttack)
        {
            _targetP = _targetM.CachedTransform.position;
            _unitM.mission_attack(_targetM, _targetUnitSS, _targetP, _id, _count);
            if (_unitM._cMission == currentMission.mAttack)
                _flankState = FlankState.Attack;
        }
    }

    private void UpdateOffsetSide()
    {
        if (_targetT != null)
        {
            Collider[] col = Physics.OverlapBox(_targetP+ _flankCheckSize/2, _flankCheckSize, _targetT.rotation, _flankLayer);
            Collider[] col2 = Physics.OverlapBox(_targetP - _flankCheckSize / 2, _flankCheckSize, _targetT.rotation, _flankLayer);
            if(col.Length > col2.Length)
            {

            }
            else if (col.Length > col2.Length)
            {

            }

        }
        else
            return;
    }

    private float CheckOffsetSide()
    {
        return 1f;
    }
}

public enum FlankState
{
    None,
    FlankMove,
    FlankAttack,
    Attack
}
    