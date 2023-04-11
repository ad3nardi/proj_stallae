using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(RichAI))]
[RequireComponent(typeof(unit_combat))]
[RequireComponent(typeof(unit_movement))]
public class unit_Manager : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public unit_settings unit;
    [SerializeField] public OptimizedBehaviour _highlightGO;

    [Header("Unit Plugins")]
    [Range (0 , 1)]
    [SerializeField] private float _idleMultiplier;
    [SerializeField] private unit_movement _movement;
    [SerializeField] private unit_combat _combat;
    [SerializeField] private RichAI _AImovement;
    [SerializeField] public List<unit_subsytems> _subsytems = new List<unit_subsytems>();

    [Header("Unit Sub-Sysetms")]
    [SerializeField] private float _hitPoints;
    [SerializeField] private float _shieldHitPoints;
    [SerializeField] private bool _hasShields;
    [SerializeField] private bool _isShielded;

    [Header("Unit Status")]
    [SerializeField] private currentMission _cMission;
    [SerializeField] public Vector3 _targetPosition;
    [SerializeField] public unit_Manager _target;
    [SerializeField] public bool _isIdle;
    [SerializeField] public int _targetUnitSS;
    [SerializeField] public bool _isSelected;


    //UNITY FUNCTIONS
    private void Awake()
    {
        //Cache
        _movement = GetComponent<unit_movement>();
        _AImovement = GetComponent<RichAI>();
        _combat = GetComponent<unit_combat>();
    }
    private void Start()
    {
        UnitDeselected();
        _target = null;

        //Set Unit to Idle on its spawn Position
        _movement.SetDefaults();
        mission_none();
        _isShielded = _hasShields;
    }
    private void LateUpdate()
    {
        if (_isIdle)
            IdleMove();

        switch (_cMission)
        {
            case currentMission.mNone:
                IdleMove();
                break;
            case currentMission.mAttack:
                _movement.StopAtAttackRangeMax(_target); 
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
    
    //UNIT SELECTION
    public void UnitSelected()
    {
        _isSelected = true;
        _highlightGO.CachedGameObject.SetActive(true);
    }
    public void UnitDeselected()
    {
        _isSelected = false;
        _highlightGO.CachedGameObject.SetActive(false);
    }
    
    //UNIT SUB-SYSTEM FUNCTIONS
    private void IdleMove()
    {
        CachedTransform.position += CachedTransform.forward * _idleMultiplier * Time.deltaTime;
    }
    public void ChangeHitPoints(float hp)
    {
        _hitPoints += hp;
    }
    public void TakeDamage(int i, float dmg)
    {
        Debug.Log("Taking Damage");
        _subsytems[i].ModifyHealth(dmg);
    }

    //UNIT MISSIONS
    public void mission_none()
    {
        _cMission = currentMission.mNone;
        _isIdle = true;
    }
    public void mission_move(Vector3 target)
    {
        _cMission = currentMission.mMove;
        _isIdle = false;
        _movement.SetIsStop(false);
        _AImovement.destination = target;

        /*
        float angle = 60; // angular step
        int countOnCircle = (int)(360 / angle); // max number in one round
        int count = meshAgents.Count; // number of agents
        float step = 1; // circle number
        int i = 1; // agent serial number
        float randomizeAngle = Random.Range(0, angle);

        while (count > 1)
        {
            var vec = Vector3.forward;
            vec = Quaternion.Euler(0, angle * (countOnCircle - 1) + randomizeAngle, 0) * vec;

            meshAgents[i].SetDestination(myAgent.destination + vec * (myAgent.radius + meshAgents[i].radius + 0.5f) * step);
            countOnCircle--;
            count--;
            i++;

            if (countOnCircle == 0)
            {
                if (step != 3 && step != 4 && step < 6 || step == 10) { angle /= 2f; }
                countOnCircle = (int)(360 / angle);
                step++;
                randomizeAngle = Random.Range(0, angle);

            }

        }
        */
    }
    public void mission_attack(unit_Manager attackTarget, int attackTargetSS, Vector3 targetPos)
    {
        _cMission = currentMission.mAttack;
        _isIdle  = false;
        _target = attackTarget;
        _targetUnitSS = attackTargetSS;
        _AImovement.destination = targetPos;
        _combat.TargetEnemy(_target, _targetUnitSS);
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