using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class wpn_missile : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private unit_subsytems _target;
    [SerializeField] private objectPooling _parentPool;
    [SerializeField] private VisualEffect _explosion;

    [Header("Settings")]
    [SerializeField] private float _hitSize;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("Variations")]
    [SerializeField] private bool _isElectrical;

    [Header("Prediction")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    [SerializeField] private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("Deviation")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    [Header("Target Velocity")]
    [SerializeField] private Vector3 _targetSpeed;
    [SerializeField] private Vector3 _targetLastPos;

    //UNITY FUNCTIONS
    private void Awake()
    {
        _target = null;
    }
    private void Start()
    {
        _parentPool = GetComponentInParent<objectPooling>();
        if (_explosion == null)
            _explosion = GetComponent<VisualEffect>();
    }
    private void OnEnable()
    {
        _targetLastPos = _target.CachedTransform.position;
    }
    private void OnDisable()
    {
        _target = null;
    }

    private void Update()
    {
        UpdateTargetVelocity();        
    }
    private void FixedUpdate()
    {
        UpdateCollision();
        CachedTransform.Translate(CachedTransform.forward * _speed);

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));

        FUpdateMovement(leadTimePercentage);
        FUpdateAddDeviation(leadTimePercentage);
        FUpdateRotateRocket();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(CachedTransform.position, _standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
    }

    //UPDATE FUNCTIONS
    private void UpdateTargetVelocity()
    {
        if (_targetLastPos != _target.CachedTransform.position)
        {
            _targetSpeed = _target.CachedTransform.position - _targetLastPos;
            _targetSpeed /= Time.deltaTime;
            _targetLastPos = _target.CachedTransform.position;
        }
    }
    private void UpdateCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(CachedTransform.position, _hitSize, _targetLayer);
        if (hitColliders.Length > 0)
        {
            DamageTarget();
        }
        else
            return;
    }

    //FIXED UPDATE FUNCTIONS
    private void FUpdateMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
        //var targetVelocity = _target.CachedTransform.position - 
        _standardPrediction = _target.CachedTransform.position + _targetSpeed.normalized * predictionTime;
    }
    private void FUpdateAddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }
    private void FUpdateRotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        CachedTransform.rotation = (Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }
    
    //WEAPON FUNCTIONS
    public void DesignateTarget(unit_subsytems unitSS)
    {
        _target = unitSS;
    }
    private void DamageTarget()
    {
        _explosion.Play();
        if (_isElectrical)
        {
            _target.SystemDisable();
        }
        _target.ModifyHealth(-_damage);
        _parentPool.ReturnObject(this);
    }
}
