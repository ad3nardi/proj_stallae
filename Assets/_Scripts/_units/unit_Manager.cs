using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using System;
using Pathfinding.ClipperLib;

[RequireComponent(typeof(RichAI))]
[RequireComponent(typeof(unit_combat))]
[RequireComponent(typeof(unit_movement))]

public class unit_Manager : OptimizedBehaviour, ISelectable
{
    [Header("Plugins")]
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public TagSet tagSet;
    [SerializeField] public unit_settings _unit;
    [SerializeField] public OptimizedBehaviour _debrisHolder;
    [SerializeField] public OptimizedBehaviour _prefabDebris;
    [SerializeField] public OptimizedBehaviour _prefabfloatHull;
    [SerializeField] public int _sizeTag;

    [Header("Unit Plugins")]
    [Range(0, 1)]
    [SerializeField] private float _idleMultiplier;
    [SerializeField] private unit_movement _movement;
    [SerializeField] private unit_combat _combat;
    [SerializeField] public unit_subSystemManager _subSystemMan;
    [SerializeField] public RichAI _AImovement { get; private set; }
    [SerializeField] private float _radius;

    [Header("Unit Sub-Sysetms")]
    [SerializeField] private float _hitPoints;

    [Header("Unit Status")]
    [SerializeField] public currentMission _cMission;
    [SerializeField] public Vector3 _targetPosition;
    [SerializeField] public unit_Manager _target;
    [SerializeField] public ITargetable _targetComp;
    [SerializeField] public bool _isIdle;
    [SerializeField] public bool _targetInRange;
    [SerializeField] public int _targetSS;
    [SerializeField] private int _index, _group;

    public static event Action<string, float, float, float, float, float, float, float> OnSelected = delegate { };
    public event Action<unit_Manager, float, bool[], float[]> GetStatusSS = delegate { };
    public event Action Selected = delegate { };
    public event Action Deselected = delegate { };

    //UNITY FUNCTIONS
    private void Awake()
    {
        //Cache
        layerSet = Helpers.LayerSet;
        tagSet = Helpers.TagSet;
        _AImovement = GetComponent<RichAI>();
        _movement = GetComponent<unit_movement>();
        _combat = GetComponent<unit_combat>();
        _targetComp = GetComponent<ITargetable>();
        _subSystemMan = GetComponentInChildren<unit_subSystemManager>();
        if(CachedGameObject.layer == layerSet.layerPlayerUnit)
        {
            SelectionMan.Instance.AvaliableUnits.Add(this);
        }
    }   
    private void Start()
    {
        _target = null;
        _sizeTag = ((int)_unit.sizeTag);
        _movement.SetDefaults();
        _radius = _AImovement.radius;
        //Set Unit to Idle on its spawn Position
        mission_none();
        
    }
    private void LateUpdate()
    {
        UpdateDisplayCard();
        UpdateCurrentMission();
        
        if (_isIdle)
            IdleMove();
        if (_AImovement.reachedDestination)
        {
            mission_none();
        }
        
    }

    private void OnEnable()
    {
        if (_subSystemMan != null)
        {
            _subSystemMan.SubSystemDestroyed += SubsystemDestory;
            _subSystemMan.UnitDestoryed += UnitDestroy;
        }
    }
    private void OnDisable()
    {
        if (_subSystemMan != null)
        {
            _subSystemMan.SubSystemDestroyed -= SubsystemDestory;
            _subSystemMan.UnitDestoryed -= UnitDestroy;
        }
    }
    private void SubsystemDestory(bool trig)
    {
        if (CachedGameObject.layer == layerSet.layerPlayerUnit)
        {
            SelectionMan.Instance.AvaliableUnits.Remove(this);
        }
        _AImovement.canMove= false;
        GameObject go = Instantiate(_prefabDebris.CachedGameObject, CachedTransform.position, Quaternion.identity, _debrisHolder.CachedTransform);
        CachedGameObject.SetActive(false);
    }
    private void UnitDestroy(bool trig)
    {
        GameObject go = Instantiate(_prefabfloatHull.CachedGameObject, CachedTransform.position, Quaternion.identity, _debrisHolder.CachedTransform);
    }

    //ISELECTABLE
    public void Select()
    {
        Selected();
    }
    public void Deselect()
    {
        Deselected();
    }
    public void GetInfoShip()
    {
        
    }
    public void GetInfoSS()
    {
        float shipHP = _targetComp.GetUnitHealth();
        bool[] ssActive = _targetComp.GetActive();
        float[] ssHP = _targetComp.GetHP();
        GetStatusSS(this, shipHP, ssActive, ssHP);
    }
    
    //UNIT SUB-SYSTEM FUNCTIONS
    private void IdleMove()
    {
        CachedTransform.position += CachedTransform.forward * _idleMultiplier * Time.deltaTime;
    }
    public void MoveSpeedChange(float mveSpd)
    {
        _AImovement.maxSpeed = _unit.unitMaxSpeed * mveSpd;
    }

    //UNIT MISSIONS
    public void mission_none()
    {
        _cMission = currentMission.mNone;
        _isIdle = true;
    }
    public void mission_move(Vector3 moveP, int i, int c)
    {
        _cMission = currentMission.mMove;
        _isIdle = false;
        _movement.SetIsStop(false);
        _index = i;
        _group = c;
        _AImovement.destination = command_moveMath(moveP);
    }
    public void mission_forceMove(Vector3 moveP)
    {
        _cMission = currentMission.mMove;
        _isIdle = false;
        _movement.SetIsStop(false);
        _AImovement.destination = moveP;
    }
    public void mission_attack(unit_Manager target, int targetSS, Vector3 targetPos, int i, int c)
    {
        _cMission = currentMission.mAttack;
        _isIdle  = false;
        _target = target;
        _targetSS = targetSS;
        _index = i;
        _group = c;
        _movement.SetIsStop(false);
        _combat.TargetEnemy(_target, _targetSS);
        
        _AImovement.destination = command_moveMath(target.CachedTransform.position);
        

    }
    public void mission_retreat()
    {

    }
    public void missoin_guard()
    {

    }
    public void mission_sticky()
    {

    }
    public void mission_enter()
    {

    }
    public void mission_capture()
    {

    }
    public void mission_guardArea()
    {

    }
    public void mission_return()
    {

    }
    public void mission_stop()
    {
        _cMission = currentMission.mStop;
        _movement.SetIsStop(true);
    }
    public void mission_ambush()
    {

    }
    public void mission_hunt()
    {

    }
    public void mission_timedHunt()
    {

    }

    //Combat Command Passthrough
    public void set_autoFire()
    {
        _combat.ToggleAutoTarget();
    }
    private void UpdateDisplayCard()
    {

    }
    private void UpdateCurrentMission()
    {
        switch (_cMission)
        {
            case currentMission.mNone:
                IdleMove();
                break;
            case currentMission.mAttack:
                if (_targetInRange)
                {
                    _movement.SetIsStop(true);
                }
                else
                {
                    _AImovement.destination = command_moveMath(_target.CachedTransform.position);
                    _movement.SetIsStop(false);
                }
                break;
            case currentMission.mMove:
                if (_AImovement.reachedDestination)
                    mission_none();
                break;
            case currentMission.mRetreat:
                break;
            case currentMission.mGuard:
                break;
            case currentMission.mSticky:
                break;
            case currentMission.mEnter:
                break;
            case currentMission.mCapture:
                break;
            case currentMission.mGuardArea:
                break;
            case currentMission.mReturn:
                break;
            case currentMission.mStop:
                break;
            case currentMission.mAmbush:
                break;
            case currentMission.mHunt:
                break;
            case currentMission.mTimedHunt:
                break;
            default:
                break;
        }
    }

    //Movement Functions
    private Vector3 command_moveMath(Vector3 hitPoint)
    {
        float radius = _radius / (Mathf.PI);
        radius *= 2f;

        float deg = 2 * Mathf.PI * _index / _group;
        Vector3 p = hitPoint + new Vector3(Mathf.Cos(deg), 0, Mathf.Sin(deg)) * radius;

        return p;
    }
}
public enum currentMission
{
    mNone,
    mAttack,
    mMove,
    mRetreat,
    mGuard,
    mSticky,
    mEnter,
    mCapture,
    mGuardArea,
    mReturn,
    mStop,
    mAmbush,
    mHunt,
    mTimedHunt
} 