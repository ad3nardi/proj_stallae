using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    [SerializeField] private currentMission _cMission;
    [Range (0 , 1)]
    [SerializeField] private float _idleMultiplier;
    [SerializeField] private unit_movement _movement;
    [SerializeField] private unit_combat _combat;
    [SerializeField] private RichAI _AImovement;
    [SerializeField] public List<unit_subsytems> _subsytems = new List<unit_subsytems>();

    [Header("Unit Sub-Sysetms")]
    [SerializeField] private ss_logic_shield _shield;
    [SerializeField] private bool _hasShields;
    [SerializeField] private bool _isShielded;

    [Header("Unit Status")]
    [SerializeField] public bool _isSelected;
    [SerializeField] public Transform _target;
    [SerializeField] public unit_subsytems _targetUnitSS;
    [SerializeField] public Vector3 _targetPosition;
    
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
        if(_shield == null)
        {
            _hasShields = false;
        }
        _isShielded = _hasShields;
    }
    private void LateUpdate()
    {
        switch (_cMission)
        {
            case currentMission.mNone:
                mission_none();
                break;
            case currentMission.mAttack:
                break;
            case currentMission.mMove:
                if(_AImovement.reachedDestination)
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
    public void SubsSystemDestoryed(unit_subsytems unitSS)
    {
        if(unitSS._subsystem == subsytemType.shield)
        {

        }
        else if(unitSS._subsystem == subsytemType.weapon)
        {

        }
        if (unitSS._subsystem == subsytemType.engine)
        {

        }
        else if (unitSS._subsystem == subsytemType.hull)
        {

        }
        else
            return;
    }

    //UNIT MISSIONS
    public void mission_none()
    {
        _cMission = currentMission.mNone;
        CachedTransform.position += CachedTransform.forward * _idleMultiplier * Time.deltaTime;
        //_AImovement.destination = CachedTransform.position + Vector3.forward * _idleMultiplier;
    }
    public void mission_attack(Transform _AttackTarget, unit_subsytems _AttackTargetSS)
    {
        _cMission = currentMission.mAttack;
        _target = _AttackTarget;
        _targetUnitSS = _AttackTargetSS;
    }
    public void mission_move(Vector3 target)
    {
        _cMission = currentMission.mMove;
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