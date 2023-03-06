using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RichAI))]
[RequireComponent(typeof(unit_combat))]
public class unit_Manager : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] public unit_settings unit;
    [SerializeField] public LayerSet layerSet;
    [SerializeField] public GameObject _highlightGO;

    [Header("Unit Plugins")]
    [SerializeField] public unit_health _health;
    [SerializeField] public unit_movement _movement;
    [SerializeField] public unit_combat _combat;
    [SerializeField] public RichAI _AImovement;
    [SerializeField] public List<unit_subsytems> _subsytems = new List<unit_subsytems>();

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
        SetDefaults();
        UnitDeselected();
        mission_move(CachedTransform.position);
    }

    private void SetDefaults()
    {
        UnitDeselected();
        _movement.SetDefaults();
        _target = null;
    }
    //UNIT SELEcTION
    public void UnitSelected()
    {
        _isSelected = true;
        _highlightGO.SetActive(true);
    }
    public void UnitDeselected()
    {
        _isSelected = false;
        _highlightGO.SetActive(false);
        
    }

    //UNIT MISSIONS
    public void mission_none()
    {

    }
    public void mission_attack(unit_subsytems _AttackTargetSS)
    { 
        
    }
    //Unit will move to passed in Vec3 coordinate
    public void mission_move(Vector3 target)
    {
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