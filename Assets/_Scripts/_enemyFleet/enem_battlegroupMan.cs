using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class enem_battlegroupMan : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] public TagSet tagSet;
    [SerializeField] public LayerSet layerSet;

    [Header("Battlegroup Units")]
    [SerializeField] private List<Transform> _unitsT = new List<Transform>();
    [SerializeField] private List<enem_unitMan> _unitsEM = new List<enem_unitMan>();
    [SerializeField] private List<Vector3> _unitsP = new List<Vector3>();

    [Header("Behaviour Settings")]
    [SerializeField] private float _aggro;
    [SerializeField] private float _spread;

    [Header("Orders Type")]
    public OrdersBeh _ordersBeh;
    public RouteBeh _routeBeh;
    public EngageBeh _engageBeh;
    public DistEngBeh _distEngBeh;

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

    [Header("Engage Settings")]
    [SerializeField] private OptimizedBehaviour _targetOB;

    [Header("Distance Engagement Settings")]
    [SerializeField] private float _squadronns;
    [SerializeField] private float _outerRange;
    [SerializeField] private float _targets;

    [Header("Thresholds")]
    [SerializeField] private float _range;
    [SerializeField] private float _hp;
    [SerializeField] private float _pwr;
    [SerializeField] private float _enemDist;
    [SerializeField] private float _enemCountFar;
    [SerializeField] private float _enemCountMid;
    [SerializeField] private float _enemCountClose; 
    [SerializeField] private float _distFleet;
    [SerializeField] private float _distObj;
    [SerializeField] private float _strVwk;    

    private void Awake()
    {
        _inFormation = false;
        _inBand1 = false;
        _inBand2 = false;
        _inBand3 = false;

        _moveOut = false;
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

        UpdateThresholdLevels();
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

    private void UpdateOrders()
    {
        if (_ordersBeh == OrdersBeh.None && !_inFormation)
        {
            SetUnitFormation();
        }
        if(_ordersBeh == OrdersBeh.Engage)
        {
            Engage(_targetOB);
        }

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
            _unitsEM[i].UnitMove(pos);
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
    private void UpdateThresholdLevels()
    {
        

    }

    private void UpdateBandRangeCheck()
    {
        _inBand1 = false; 
        Collider[] hitColB1 = Physics.OverlapSphere(CachedTransform.position, _checkRangeBandOne, _checkLayer);
        if (hitColB1.Length > 0)
        {
            _inBand1 = true;
            _enemCountFar = hitColB1.Length;

            _enemCountMid = 0;
            _inBand2 = false;

            Collider[] hitColB2 = Physics.OverlapSphere(CachedTransform.position, _checkRangeBandTwo, _checkLayer);
            if (hitColB2.Length > 0)
            {
                _inBand2 = true;
                _enemCountMid = hitColB2.Length;
                for (int i = 0; i < hitColB1.Length; i++)
                {
                    if (hitColB1[i].transform == hitColB2[i].transform)
                    {
                        _enemCountFar -= 1;
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
                    _enemCountClose = hitColB3.Length;
                    for (int i = 0; i < hitColB2.Length; i++)
                    {
                        if (hitColB2[i].transform == hitColB3[i].transform)
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


                    Debug.Log(angle);
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
    public void Engage(OptimizedBehaviour targetOB)
    {
        for (int i = 0; i < _unitsEM.Count; i++)
        {
            _unitsEM[i].UpdateOrdersType(OrdersBeh.Engage);
        }

        if (_engageBeh == EngageBeh.Direct)
        {

        }


        if (_engageBeh == EngageBeh.Flank)
        {
            for (int i = 0; i < _unitsEM.Count; i++)
            {
                _unitsEM[i].EngageFlank(targetOB);
            }
            _engageBeh = EngageBeh.None;
        }


        if (_engageBeh == EngageBeh.Fsup)
        {

        }
    }

    public void Route()
    {
        if (_engageBeh == EngageBeh.Direct)
        {

        }
        if (_engageBeh == EngageBeh.Flank)
        {

        }
        if (_engageBeh == EngageBeh.Fsup)
        {

        }
    }

    public void DistEngage()
    {
        if (_engageBeh == EngageBeh.Direct)
        {

        }
        if (_engageBeh == EngageBeh.Flank)
        {

        }
        if (_engageBeh == EngageBeh.Fsup)
        {

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