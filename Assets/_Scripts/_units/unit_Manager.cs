using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using System;
using Pathfinding.ClipperLib;
using TMPro;
using DG.Tweening;

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
    [SerializeField] public int _sizeTag;

    [Header("Unit Plugins")]
    [Range(0, 1)]
    [SerializeField] private float _idleMultiplier;
    [SerializeField] private unit_movement _movement;
    [SerializeField] private unit_combat _combat;
    [SerializeField] public unit_subSystemManager _subSystemMan;
    [SerializeField] public unit_squadTarget _squadMan;
    [SerializeField] public RichAI _AImovement { get; private set; }
    [SerializeField] private float _radius;
    [SerializeField] public bool _isSquadronType { get; private set; }
    private Quaternion _lockRot;

    [Header("Unit Sub-Sysetms")]
    [SerializeField] private float _hitPoints;

    [Header("Unit Status")]
    [SerializeField] public currentMission _cMission;
    [SerializeField] public Vector3 _targetPosition;
    [SerializeField] public Transform _target;
    [SerializeField] public ITargetable _iThisTarget;
    [SerializeField] public IAbility _iThisAbility;
    [SerializeField] public string _abilityName;
    [SerializeField] public bool _isIdle;
    [SerializeField] public bool _targetInRange;
    [SerializeField] public int _targetSS;
    [SerializeField] private int _index, _group;
    [SerializeField] private TextMeshProUGUI _talkBubble;
    [SerializeField] private OptimizedBehaviour _talkUI;
    [SerializeField] private RectTransform _cachedTalkUI;

    [SerializeField] private float _talkAnimTime;
    [SerializeField] private float _talkTime;
    private float _talkTimer;
    private bool _talkAnimActive;

    public OptimizedBehaviour _fleetHolder;
    private OptimizedBehaviour _movePoint;

    public event Action Selected = delegate { };
    public event Action Deselected = delegate { };

    public bool _isDestoryed;

    //UNITY FUNCTIONS
    private void Awake()
    {
        
    }
    private void Update()
    {
        UpdateTimer();
    }

    private void Start()
    {
        
    }
    private void LateUpdate()
    {
        UpdateDisplayCard();
        UpdateCurrentMission();
        
        if (_isIdle)
        {
            IdleMove();

        }
        if (_AImovement.reachedDestination)
        {
            mission_none();
        }
        
    }

    private void OnEnable()
    {
        //Cache
        layerSet = Helpers.LayerSet;
        tagSet = Helpers.TagSet;
        _AImovement = GetComponent<RichAI>();
        _movement = GetComponent<unit_movement>();
        _combat = GetComponent<unit_combat>();
        _subSystemMan = GetComponent<unit_subSystemManager>();
        _isSquadronType = _unit._isSquadron;
        _abilityName = _unit._abilityName;

        _isDestoryed = false;

        _talkAnimActive = false;
        _talkTimer = 0;

        GameObject mp = Instantiate(OnFly_manager.Instance._onFlyResources._PFonMoveUI, OnFly_manager.Instance.CachedTransform);
        _movePoint = mp.GetComponent<OptimizedBehaviour>();
        _movePoint.CachedGameObject.SetActive(false);

        if (CachedGameObject.layer == layerSet.layerPlayerUnit)
        {
            SelectionMan.Instance.AvaliableUnits.Add(this);
        }
        _movement.SetDefaults();

        if (_subSystemMan != null)
        {
            _subSystemMan.SubSystemDestroyed += SubsystemDestory;
            _subSystemMan.UnitDestoryed += UnitDestroy;
        }
        if(_squadMan != null)
        {
            _squadMan.SquadMembDestroyed += SubsystemDestory;
            _squadMan.UnitDestoryed += UnitDestroy;
        }
        _target = null;
        _sizeTag = ((int)_unit.sizeTag);
        _radius = _unit.unitRadius;

        _fleetHolder = GetComponentInParent<OptimizedBehaviour>();
        _debrisHolder = DebrisHolder.Instance.HolderGO;
        //Set Unit to Idle on its spawn Position
        mission_none();

        _cachedTalkUI = _talkUI.CachedTransform.GetComponent<RectTransform>();
        _cachedTalkUI.localScale = Vector3.zero;
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
        if(_unit.debris != null)
        {
            GameObject go = Instantiate(_unit.debris.CachedGameObject, CachedTransform.position, Quaternion.identity, _debrisHolder.CachedTransform);
        }
    }
    private void UnitDestroy(bool trig)
    {
        _isDestoryed = true;
        if (CachedGameObject.layer == layerSet.layerPlayerUnit)
        {
            SelectionMan.Instance.AvaliableUnits.Remove(this);
        }
        _AImovement.canMove= false;
        if (_unit.debrisHull != null)
        {
            GameObject go = Instantiate(_unit.debrisHull.CachedGameObject, CachedTransform.position, Quaternion.identity, _debrisHolder.CachedTransform);
        }
        CachedGameObject.SetActive(false);
    }

    //ISELECTABLE
    public void Select()
    {
        Selected();
        _movePoint.CachedGameObject.SetActive(true);

    }
    public void Deselect()
    {
        Deselected();
        _movePoint.CachedGameObject.SetActive(false);

    }

    //IAbility
    public void ActivateAbility()
    {
        _iThisAbility.Activate();
    }
    
    //UNIT SUB-SYSTEM FUNCTIONS
    private void IdleMove()
    {
        CachedTransform.position += CachedTransform.forward * _idleMultiplier * Time.deltaTime;
    }

    //Unit Speach
    public void Talk(string str)
    {
        _cachedTalkUI.DOScale(Vector3.one, _talkAnimTime);
        _talkBubble.text = str;
        _talkAnimActive = true;
        _talkTimer = _talkTime;
    }
    private void TalkOut()
    {
        _cachedTalkUI.DOScale(Vector3.zero, _talkAnimTime);

    }
    private void UpdateTimer()
    {
        if (!_talkAnimActive)
            return;
        else
        {
            _talkTimer -= Time.deltaTime;
            if (_talkTimer <= 0)
            {
                _talkAnimActive = false;
                TalkOut();
            }
        }
            
    }

    //UNIT MISSIONS
    public void mission_none()
    {
        _cMission = currentMission.mNone;
        _isIdle = true;
        _movePoint.CachedGameObject.SetActive(false);
    }
    public void mission_move(Vector3 moveP, int i, int c)
    {
        _cMission = currentMission.mMove;
        _isIdle = false;
        _movement.SetIsStop(false);
        _index = i;
        _group = c;
        _AImovement.destination = command_moveMath(moveP);
        _movePoint.CachedTransform.position = moveP;
        Talk("Moving Out");
    }
    public void mission_forceMove(Vector3 moveP)
    {
        _cMission = currentMission.mMove;
        _isIdle = false;
        _movement.SetIsStop(false);
        _AImovement.destination = moveP;
        _movePoint.CachedTransform.position = moveP;
    }
    public void mission_attack(Transform target, int targetSS, Vector3 targetPos, int i, int c)
    {
        _cMission = currentMission.mAttack;
        _isIdle  = false;
        _target = target;
        _targetSS = targetSS;
        _index = i;
        _group = c;
        _movement.SetIsStop(false);

        _combat.TargetEnemy(target, _targetSS);
        
        _AImovement.destination = command_moveMath(_target.position);
        Talk("We're going after them");

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
                    _AImovement.destination = command_moveMath(_target.position);
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