using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_visCollision : OptimizedBehaviour
{

    [Header("Plugins")]
    [SerializeField] public LayerSet _layerSet;
    [SerializeField] public TagSet _tagSet;
    [SerializeField] public int _sizeTag;
    [SerializeField] private Collider _unitCol;


    [Header("Rise/Return Movement")]
    
    [SerializeField] private LayerMask _moveCheckLayers;
    [SerializeField] private Vector3 _startingPos;
    [SerializeField] private Vector3 _risePos;
    [SerializeField] private float _checkDistance;

    [SerializeField] private float _riseHeight;
    [SerializeField] private float _riseTimer;
    [SerializeField] private float _riseTime;
    [SerializeField] private bool _atRise;

    [SerializeField] private float _returnHieght;
    [SerializeField] private float _returnTimer;
    [SerializeField] private float _returnTime;
    [SerializeField] private bool _atReturn;
    [SerializeField] private bool _colliding;

    private void Awake()
    {
        _unitCol = GetComponent<Collider>();

    }

    private void Start()
    {
        _layerSet = Helpers.LayerSet;

        _startingPos = CachedTransform.position;
        _risePos = CachedTransform.up * _riseHeight;
        _returnHieght = CachedTransform.position.y;
        _atReturn = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(CachedTransform.position, _checkDistance);
    }
    private void Update()
    {
        UpdateTimers();
        UpdateCheckMovement();
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(CachedTransform.position, _checkDistance, _moveCheckLayers);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != _unitCol)
            {
                
                if (_sizeTag < colliders[i].GetComponent<unit_visCollision>()._sizeTag)
                {
                    Debug.Log(colliders[i]);
                    _colliding = true;
                    return;
                }
                else
                    return;
            }
            else
            {
                _colliding = false;
                return;
            }
        }
    }

    #region Update Functions
    private void UpdateTimers()
    {
        if (_atRise)
            _returnTimer = 0f;

        if (_atReturn)
            _riseTimer = 0f;

        _riseTimer += Time.deltaTime;
        _returnTimer += Time.deltaTime;
    }
    
    private void UpdateCheckMovement()
    {

        if (CachedTransform.position.y < _riseHeight)
        {
            _atRise = false;
        }
        if (CachedTransform.position.y > _returnHieght)
        {
            _atReturn = false;
        }
        if (CachedTransform.position.y >= _riseHeight)
        {
            _atRise = true;
            _atReturn = false;
        }
        if (CachedTransform.position.y <= _returnHieght)
        {
            _atReturn = true;
            _atRise = false;
        }
        if (_colliding)
        {
            if (!_atRise)
            {
                _risePos = new Vector3(CachedTransform.position.x, _riseHeight, CachedTransform.position.z);
                CachedTransform.position = Vector3.Lerp(CachedTransform.position, _risePos, _riseTimer / _riseTime);
                _atReturn = false;
            }
        }
        if (!_colliding)
        {
            if (!_atReturn)
            {
                _startingPos = new Vector3(CachedTransform.position.x, _returnHieght, CachedTransform.position.z);
                CachedTransform.position = Vector3.Lerp(CachedTransform.position, _startingPos, _returnTimer / _returnTime);
                _atRise = false;
                _atReturn = false;
            }
        }
    }
    #endregion

}
