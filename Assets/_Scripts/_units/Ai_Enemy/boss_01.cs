using DG.Tweening;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class boss_01 : OptimizedBehaviour
{
    [Header("Plugin")]
    [SerializeField] private TagSet _tagSet;
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private unit_settings _unit;

    [SerializeField] private OptimizedBehaviour _target;
    [SerializeField] private unit_Manager _unitM;
    [SerializeField] private RichAI _aiMovement;

    [Header("Settings")]
    [SerializeField] private float _activeTime;
    [SerializeField] private float _activeTimer;
    [SerializeField] private bool _isActive;
    [SerializeField] private int _numPositions;

    [Header("Teleportation")]
    [SerializeField] private bool _isInTeleport;
    [SerializeField] private float _teleportTime;
    [SerializeField] private float _teleportTimer;

    [SerializeField] private float _teleportRng;

    [SerializeField] private Vector3 _teleportAttackPos;
    [SerializeField] private Vector3 _teleportSafePos;

    [Header("Combat")]
    [SerializeField] private Vector3 _attackCheckOffset;
    [SerializeField] private Vector3 _attackCheckSize;

    [SerializeField] private Vector3 _movementAdjustment;

    [SerializeField] private Vector2 _attackDirection;

    [SerializeField] private float _dmg;
    [SerializeField] private float _atkTime;
    [SerializeField] private float _atkTimer;

    [Header("Animation")]
    [SerializeField] private OptimizedBehaviour _visualOb;
    [SerializeField] private Vector3 _attackAnim;
    [SerializeField] private float _attackAnimTime;

    [Header("VFX")]
    [SerializeField] private List<VisualEffect> _weaponVFX = new List<VisualEffect>();
    [SerializeField] private VisualEffect _teleportIn;
    [SerializeField] private VisualEffect _teleportOut;

    private void Awake()
    {
        _activeTimer = _activeTime;
        _teleportTimer = _teleportTime;
        _unitM = GetComponent<unit_Manager>();
        _aiMovement = GetComponent<RichAI>();

        _tagSet = Helpers.TagSet;
        _layerSet = Helpers.LayerSet;
    }

    private void Update()
    {
        UpdateTimers();
        UpdateAttackCheck();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackCheckOffset, _attackCheckSize * 2);
    }

    private void UpdateAttackCheck()
    {
        if (_isActive)
        {
            _atkTime -= Time.deltaTime;
            if(_atkTimer <= 0)
            {
                Collider[] colHit = Physics.OverlapBox(CachedTransform.position + _attackCheckOffset, _attackCheckSize, Quaternion.identity, _layerMask);
                if (colHit.Length > 0)
                {

                    _visualOb.CachedTransform.DOLocalRotate(_attackAnim, _attackAnimTime, RotateMode.Fast);
                    for (int i = 0; i < colHit.Length; i++)
                    {
                        ITargetable Itarget = colHit[i].transform.GetComponent<ITargetable>();
                        bool[] actives = Itarget.GetActive();
                        float[] hps = Itarget.GetHP();
                        int firingTarget = UnityEngine.Random.Range(0, 6);


                        if (actives[firingTarget] != true)
                        {
                            for (int f = 0; f < 6; f++)
                            {
                                firingTarget = f;
                                if (actives[firingTarget] == true)
                                {
                                    break;
                                }
                            }
                        }
                        for (int w = 0; w < _weaponVFX.Count; w++)
                        {
                            _weaponVFX[w].Play();
                        }
                        Itarget.TakeDamage(firingTarget, -_dmg);
                    }
                    _atkTimer = _atkTime;
                }
            }
        }
    }


    private void CalculatePositions()
    {
        if (_target == null)
        {
            return;
        }

        float rand = UnityEngine.Random.Range(0, _numPositions);

        float angleIncrement = 360.0f / _numPositions;

        for (int i = 0; i < _numPositions; i++)
        {
            float angle = i * angleIncrement;
            Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * _teleportRng;
            Vector3 position = _target.CachedTransform.position + offset;

            if(i == rand)
            {
                _teleportAttackPos = position;
                return;
            }
        }
    }

    private void TeleportIn()
    {
        CalculatePositions();
        if (!_isInTeleport && !_isActive)
        {
            _aiMovement.Teleport(_teleportAttackPos, true);
            SetMove();
        }
    }

    private void SetMove()
    {
        if (_isActive)
        {
            _aiMovement.destination = CachedTransform.localPosition + _movementAdjustment;
        }
    }


    private void TeleportOut()
    {
        _visualOb.CachedTransform.DOLocalRotate(Vector3.zero, _attackAnimTime, RotateMode.Fast);
        Helpers.GetWait(_attackAnimTime);
        _aiMovement.Teleport(_teleportSafePos, true);
        _isActive = false;
        _activeTimer = _activeTime;
    }

    private void UpdateTimers()
    {
        if (_isActive)
        {
            _activeTimer -= Time.deltaTime;
            if(_activeTimer <= 0)
            {
                _isActive= false;
                _isInTeleport = true;
                _teleportTimer = _teleportTime;
                TeleportOut();
            }
        }

        if(_isInTeleport)
        {
            _teleportTimer -= Time.deltaTime;
            if(_teleportTimer <= 0)
            {
                TeleportIn();
                _isActive= true;
                _isInTeleport= false;
                _activeTimer = _activeTime;
            }
        }
    }
}
