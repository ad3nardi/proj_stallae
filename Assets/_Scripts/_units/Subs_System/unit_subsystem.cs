using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class unit_subsystem : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private unit_subSystemManager _subsystemM;
    [SerializeField] public bool _activeSubsytem;
    [SerializeField] private bool _isDestroyed;
    [SerializeField] private bool _isDisabled;
    [SerializeField] private bool _isSubscribed;
    [SerializeField] private Image _img;
    [SerializeField] private GameObject _goExpolosion;
    [SerializeField] private VisualEffect _vfxExpolosion;

    [Header("Settings")]
    [SerializeField] public subsytemType _subsystem;
    [SerializeField] private float _maxHP;
    [SerializeField] private float _disableTimer;

    [SerializeField] public float _curHP { get; private set; }

    public static event Action<unit_subsystem> OnHealthAdded = delegate { };
    public event Action<float> OnHealthPctChanged = delegate { };
    public event Action<float> OnHealthChanged = delegate { };
    public event Action<bool> OnDestroyed = delegate { };
    public event Action<bool> OnDisabled = delegate { };

    //UNITY FUNCTIONS
    private void Awake()
    {
        _isSubscribed = false;
        _subsystemM = GetComponentInParent<unit_subSystemManager>();
        _goExpolosion = null;
    }
    private void Start()
    {
        if (_activeSubsytem && !_isSubscribed)
        {
            _isSubscribed = true;
            _subsystemM.SetMaxHealth += SetToMaxHP;
        }
        if (_activeSubsytem && _subsystemM._unitM._unit.vfx_subExplosion != null)
        {
            _goExpolosion = Instantiate(_subsystemM._unitM._unit.vfx_subExplosion.gameObject, CachedTransform);
            _vfxExpolosion = _goExpolosion.GetComponent<VisualEffect>();
            _goExpolosion.SetActive(false);
            _vfxExpolosion.Stop();
        }
        if (!_activeSubsytem)
        {
            _img.fillAmount= 0;
        }
    }
    private void OnEnable()
    {
        if(_activeSubsytem && !_isSubscribed)
        {
            _isSubscribed = true;
            _subsystemM.SetMaxHealth += SetToMaxHP;
        }
    }


    private void Update()
    {
        UpdateTimers(Time.deltaTime);
    }

    private void OnDisable()
    {
        if (_isSubscribed)
        {
            _subsystemM.SetMaxHealth -= SetToMaxHP;
        }
    }

    //HEALTH & DAMAGE FUNCTIONS
    public void SetToMaxHP(float maxHp, float subSystemCount)
    {
        _maxHP = maxHp / subSystemCount;
        _curHP = _maxHP;
        OnHealthAdded(this);
        _isDisabled = false;
        _isDestroyed = false;
        OnDestroyed(_isDestroyed);
        OnDisabled(_isDisabled);
    }

    public void ModifyHealth(float amount)
    {
        _curHP += amount;
        float currentHPpct = _curHP / _maxHP;
        _img.fillAmount = currentHPpct;
        OnHealthPctChanged(currentHPpct);
        OnHealthChanged(_curHP);
        if (_curHP >= 0)
        {
            OnDestroyed(false);
        }
        else
        {
            SystemDestroy();
        }
    }

    public void SystemDisable(bool isDisabled)
    {
        _isDisabled = isDisabled;
        OnDisabled(_isDisabled);
    }

    public void SystemDestroy()
    {
        _goExpolosion.SetActive(true);
        _vfxExpolosion.Play();
        _isDestroyed = true;
        OnDestroyed(_isDestroyed);
    }

    private void UpdateTimers(float time)
    {
        _disableTimer += time;

        if (!_isDisabled)
            return;
        else if (_isDisabled)
        {
            if(_disableTimer >= 0)
            {
                SystemDisable(false);
            }
        }
    }
}

public enum subsytemType
{
    hull,
    weapon1,
    weapon2,
    shield,
    engine,
    special
}
