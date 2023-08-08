using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class enem_battlegroupMan : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] public TagSet tagSet;
    [SerializeField] public LayerSet layerSet;

    [Header("Battlegroup Units")]
    [SerializeField] public List<Transform> _unitsT = new List<Transform>();
    [SerializeField] private List<enem_unitMan> _unitsEM = new List<enem_unitMan>();
    [SerializeField] private List<unit_Manager> _unitsM = new List<unit_Manager>();
    [SerializeField] private List<Vector3> _unitsP = new List<Vector3>();

    [Header("Behaviour Settings")]
    public OrdersBeh _ordersBeh;
    [SerializeField] private float _aggro;
    [SerializeField] private float _spread;
    [SerializeField] private bool _ifChangeTarget;
    [SerializeField] private OptimizedBehaviour _targetOB;
    [SerializeField] private OptimizedBehaviour _closestTarget;


    [Header("Settings Front Zone Check")]
    [SerializeField] private bool _enemInZone;
    [SerializeField] private int _enemCountZone;
    [SerializeField] private Vector3 _zoneOffsetPos, _zonetOffsetRot, _zoneOffsetScale;

    [Header("Settings Flank Check")]
    [SerializeField] private float _flankCheckSize;
    [SerializeField] private bool _enemInFlankR, _enemInFlankL, _enemInFlankB;
    [SerializeField] private int _enemCountZoneR, _enemCountZoneL, _enemCountZoneB;

    [Header("Settings Range Check")]
    [SerializeField] private LayerMask _checkLayer;
    [SerializeField] private float _checkRangeBandOne, _checkRangeBandTwo, _checkRangeBandThree;
    [SerializeField] private bool _inBand1, _inBand2, _inBand3;

    [Header("Spread Settings")]
    [SerializeField] private bool _inFormation, _moveOut;
    [SerializeField] private float _spreadRadius;

    [Header("Attack Target Choice")]
    [SerializeField] private float _targetWeight;

    [Header("Route Settings")]
    [SerializeField] private float _toCommonPoint, _toFarPoint, _toFleet;

    [Header("Distance Engagement Settings")]
    [SerializeField] private float _squadronns;
    [SerializeField] private float _outerRange;
    [SerializeField] private float _targets;

    [Header("Threshold Inputs")]
    [SerializeField] private float _range;
    [SerializeField] private float _enemFriendsCheckRange;
    [SerializeField] private float _hp;
    [SerializeField] private float _pwr;
    [SerializeField] private float _enemDist;
    [SerializeField] private float _enemCountNear;
    [SerializeField] private float _enemCountFar;
    [SerializeField] private float _enemCountMid;
    [SerializeField] private float _enemCountClose; 
    [SerializeField] private float _distFleet;
    [SerializeField] private float _distObj;
    [SerializeField] private float _strVwk;

    [Header("Threshold Adjustments")]
    [SerializeField] private float _adjEnemNear;
    [SerializeField] private float _adjEnemAlone;
    [SerializeField] private float _adjEnemFriends;

    [Header("Threshold Levels")]
    [SerializeField] private float _lvlEnemNear;
    [SerializeField] private float _lvlEnemAlone;
    [SerializeField] private float _lvlEnemFriends;

    [Header("Thresholds")]
    [SerializeField] private float _threshEngage;
    [SerializeField] private float _threshEnemNear;
    [SerializeField] private float _threshEnemAlone;
    [SerializeField] private float _threshEnemFriends;

    private void Awake()
    {
        _ifChangeTarget = true;
        _inFormation = false;
        _inBand1 = false;
        _inBand2 = false;
        _inBand3 = false;

        _moveOut = false;
        _ordersBeh = OrdersBeh.None;

        ResetFlank();
        layerSet = Helpers.LayerSet;
        tagSet = Helpers.TagSet;
    }

    private void Start()
    {
        _unitsEM.Clear();
        for (int i = 0; i < _unitsT.Count; i++)
        {
            _unitsT[i] = _unitsT[i].GetComponent<OptimizedBehaviour>().CachedTransform;
            _unitsEM.Add(_unitsT[i].GetComponent<enem_unitMan>());
            _unitsM.Add(_unitsT[i].GetComponent<unit_Manager>());
            _unitsEM[i]._bgMan = this;
        }
    }

    private void Update()
    {
        UpdateBandRangeCheck();
        UpdateZoneFrontCheck();
        UpdateZoneFlankCheck();

        if (_moveOut)
        {
            RecenterBattlegroup();
        }
        UpdateThresholds();
        UpdateOrders();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(CachedTransform.position, _flankCheckSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CachedTransform.position, _checkRangeBandOne);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CachedTransform.position, _checkRangeBandTwo);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CachedTransform.position, _checkRangeBandThree);
        
        Gizmos.color = Color.grey;
        if(_closestTarget != null)
            Gizmos.DrawSphere(_closestTarget.CachedTransform.position, _enemFriendsCheckRange);


        Gizmos.color = Color.cyan;
        Matrix4x4 prevMatrix = Gizmos.matrix;

        Gizmos.matrix = CachedTransform.localToWorldMatrix;

        Vector3 boxPosition = CachedTransform.position;

        // convert from world position to local position 
        boxPosition = CachedTransform.InverseTransformPoint(boxPosition);

        Vector3 boxSize = _zoneOffsetScale * 2;
        Gizmos.DrawWireCube(boxPosition + _zoneOffsetPos, boxSize);

        // restore previous Gizmos settings
        Gizmos.matrix = prevMatrix;
    }

    //Battlegroup Management
    private void SetUnitFormation()
    {
        for (int i = 0; i < _unitsEM.Count; i++)
        {
            float angle = i * Mathf.PI * 2 / _unitsEM.Count + (1 % 2 != 0 ? 0 : 0);

            float radius = _spreadRadius;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            float y = 0f;

            Vector3 pos = new Vector3(x, y, z) + CachedTransform.position;
            _unitsEM[i].UnitForceMove(pos);
        }
        _inFormation = true;
    }

    private void RecenterBattlegroup()
    {
        for (int i = 0; i < _unitsT.Count; i++)
        {
            _unitsP.Add(_unitsT[i].position);
        }
        CachedTransform.position = GetMeanVector(_unitsP);
        _moveOut = false;
        _unitsP.Clear();
    }

    private Vector3 GetMeanVector(List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Vector3 pos in positions)
        {
            meanVector += pos;
        }

        return (meanVector / positions.Count);
    }

    //UPDATE FUNCTIONS
    private void UpdateThresholds()
    {
        _lvlEnemNear= 0;

        if (_enemInZone)
            _lvlEnemNear += _adjEnemNear * 0.2f;
        if(_enemCountNear != 0)
        {
            float flankBAdj = Convert.ToInt16(_enemInFlankB) * 5f;
            float flankRLAdj = Convert.ToInt16(_enemInFlankB) * 3f;

            if (_inBand1)
                _lvlEnemNear += _adjEnemNear * (_enemCountFar + flankBAdj + flankRLAdj) * 0.3f;
            if (_inBand2)
                _lvlEnemNear += _adjEnemNear * (_enemCountFar + flankBAdj + flankRLAdj) * 0.5f;
            if (_inBand3)
                _lvlEnemNear += _adjEnemNear * (_enemCountFar + flankBAdj + flankRLAdj);
        }
    }
    
    private void UpdateOrders()
    {
        int orderType = 0;
        if(_lvlEnemNear + _lvlEnemFriends + _lvlEnemAlone >= _threshEngage)
            _ordersBeh = OrdersBeh.Engage;

        if (_lvlEnemNear >= _threshEnemNear)
        {
            orderType = (int)EngageBeh.Direct;
        }
        if (_lvlEnemFriends >= _threshEnemFriends)
        {
            orderType = (int)EngageBeh.Flank;
        }

        UpdateConfirmOrders(orderType);
    }

    private void UpdateConfirmOrders(int orderType)
    {
        if (_ordersBeh == OrdersBeh.None && !_inFormation)
        {
            SetUnitFormation();
        }
        if (_ordersBeh == OrdersBeh.Engage)
        {
            if(_targetOB != null)
            {
                Engage(_targetOB, orderType);
            }
        }
        if(_ordersBeh == OrdersBeh.Route)
        {

        }
        if(_ordersBeh == OrdersBeh.DistEngage)
        {

        }
    }

    private void UpdateBandRangeCheck()
    {
        _enemCountNear = 0f;
        _inBand1 = false; 
        Collider[] hitColB1 = Physics.OverlapSphere(CachedTransform.position, _checkRangeBandOne, _checkLayer);
        if (hitColB1.Length > 0)
        {
            _inBand1 = true;
            UpdateTargetChoice(hitColB1);
            _enemCountFar = hitColB1.Length;
            _enemCountNear = hitColB1.Length;

            _enemCountMid = 0;
            _inBand2 = false;

            Collider[] hitColB2 = Physics.OverlapSphere(CachedTransform.position, _checkRangeBandTwo, _checkLayer);
            if (hitColB2.Length > 0)
            {
                _inBand2 = true;
                UpdateTargetChoice(hitColB2);
                _enemCountMid = hitColB2.Length;
                for (int i = 0; i < hitColB1.Length; i++)
                {
                    //if (hitColB1[i].transform == hitColB2[i].transform)
                    if (hitColB2.Contains(hitColB1[i]))
                    {
                        _enemCountFar --;
                        if(_enemCountFar == 0)
                        {
                            _inBand1 = false;
                        }
                    }
                }
                _enemCountClose = 0;
                _inBand3 = false;

                Collider[] hitColB3 = Physics.OverlapSphere(CachedTransform.position, _checkRangeBandThree, _checkLayer);
                if (hitColB3.Length > 0)
                {
                    _inBand3 = true;
                    UpdateTargetChoice(hitColB3);
                    _enemCountClose = hitColB3.Length;
                    for (int i = 0; i < hitColB2.Length; i++)
                    {
                        //if (hitColB2[i].transform == hitColB3[i].transform)
                        if (hitColB3.Contains(hitColB2[i]))
                        {
                            _enemCountMid -= 1;
                            if (_enemCountMid == 0)
                            {
                                _inBand2 = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateZoneFrontCheck()
    {
        Quaternion zonetOffsetRot = Quaternion.Euler(_zonetOffsetRot);
        Vector3 boxPos = CachedTransform.InverseTransformPoint(CachedTransform.position);

        Collider[] hitColFront = Physics.OverlapBox(boxPos +  _zoneOffsetPos, _zoneOffsetScale, transform.rotation * zonetOffsetRot, _checkLayer);
        _enemCountZone = hitColFront.Length;
        if (_enemCountZone > 0)
        {
            _enemInZone = true;
        }
        else
            _enemInZone = false;
    }

    private void UpdateZoneFlankCheck()
    {
        ResetFlank();
        
        Collider[] hitCol = Physics.OverlapSphere(CachedTransform.position, _flankCheckSize, _checkLayer);
        int enemCountTot = hitCol.Length;
        if (hitCol.Length > 0 )
        {
            for (int i = 0; i < enemCountTot; i++)
            {
                Transform enemShip = hitCol[i].transform;            
                Vector2 shipPos = new Vector2 (enemShip.position.x - CachedTransform.position.x, enemShip.position.z - CachedTransform.position.z);
                shipPos.Normalize();
                if(shipPos != Vector2.zero)
                {
                    float angle = Mathf.Atan2(shipPos.y, -shipPos.x) / Mathf.PI;
                    angle *= 180;
                    angle -= 90;
                    if (angle < 0)
                        angle += 360;

                    angle -= CachedTransform.rotation.eulerAngles.y;
                    if (angle < 0)
                        angle += 360;


                    if (angle > 45 && angle < 135)
                    {
                        _enemCountZoneR++;
                        _enemInFlankR = true;
                    }
                    else if (angle > 135 && angle < 225)
                    {
                        _enemCountZoneB++;
                        _enemInFlankB = true;
                    }
                    else if (angle > 225 && angle < 315)
                    {
                        _enemCountZoneL++;
                        _enemInFlankL = true;
                    }
                }
                
            }
        } 
    }

    private void UpdateTargetChoice(Collider[] array)
    {
        if (_ifChangeTarget)
        {
            if (array.Length > 1)
            {
                float closestDistSqr = Mathf.Infinity;
                Vector3 currentPos = CachedTransform.position;

                List<unit_Manager> targetsM = new List<unit_Manager>();

                for (int u = 0; u < array.Length; u++)
                {
                    targetsM.Add(array[u].transform.GetComponent<unit_Manager>());
                }

                for (int i = 0; i < array.Length; i++)
                {
                    Vector3 distanceToTarget = targetsM[i].CachedTransform.position - currentPos;

                    float dSqrToTarget = distanceToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistSqr)
                    {
                        closestDistSqr = dSqrToTarget;
                        _closestTarget = targetsM[i];
                    }
                }
            }
            else
                _closestTarget = array[0].transform.GetComponent<unit_Manager>();

            Collider[] hitCol = Physics.OverlapSphere(_closestTarget.CachedTransform.position, _enemFriendsCheckRange, _checkLayer);
            if(hitCol.Length <= 0)
            {
                ChangeTarget(_closestTarget);
            }
            else
            {
                _lvlEnemFriends += _adjEnemFriends * hitCol.Length;
                ChangeTarget(_closestTarget);
            }
        }
        else
            return;
    }

    private void ChangeTarget(OptimizedBehaviour targetOb)
    {
        _targetOB = targetOb;
        _ifChangeTarget = false;
    } 

    private void ResetFlank()
    {
        _enemCountZoneR = 0;
        _enemCountZoneL = 0;
        _enemCountZoneB = 0;
        _enemInFlankR = false;
        _enemInFlankL = false;
        _enemInFlankB = false;
    }

    //ORDERS
    public void Engage(OptimizedBehaviour targetOB, int orderType)
    {
        for (int i = 0; i < _unitsEM.Count; i++)
        {
            _unitsEM[i].UpdateOrdersType(OrdersBeh.Engage);
        }

        switch (orderType)
        {
            case 0: //None
                break;
            case 1: //Direct
                for (int i = 0; i < _unitsEM.Count; i++)
                {
                    _unitsEM[i].Engage(targetOB, i, _unitsEM.Count, FlankState.None);
                    
                }
                break;

            case 2: //Flank
                for (int i = 0; i < _unitsEM.Count; i++)
                {
                    _unitsEM[i].Engage(targetOB, i, _unitsEM.Count, FlankState.FlankMove);
                }
                orderType = 0;
                _targetOB = null;
                break;

            case 3: //Fire Support

                break;
        }
    }

    public void Route(int orderType)
    {
        switch (orderType)
        {
            case 0: //None
                break;
            case 1: //To Point Common
               
                break;

            case 2: //To Point Far
                
                break;

            case 3: //To Fleet

                break;
        }
    }

    public void DistEngage(int orderType)
    {
        switch (orderType)
        {
            case 0: //None
                break;
            case 1: //To Point Common

                break;

            case 2: //To Point Far

                break;

            case 3: //To Fleet

                break;
        }
    }
}
public enum OrdersBeh
{
    None,
    Route,
    Engage,
    DistEngage
}
public enum EngageBeh
{
    None,
    Direct,
    Flank,
    Fsup
}
public enum RouteBeh
{
    None,
    ToPointCommon,
    ToPointFar,
    ToFleet
}
public enum DistEngBeh
{
    None
}