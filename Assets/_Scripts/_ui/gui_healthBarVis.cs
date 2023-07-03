using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gui_healthBarVis : OptimizedBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Image _foregroundImage; 
    [SerializeField] private float _updateSpeedSeconds = 0.5f; 
    [SerializeField] private float _posOffset;
    
    private unit_subsystem _health;

    private void OnEnable()
    {
        _cam = Helpers.Camera;
    }

    public void SetHealth(unit_subsystem health)
    {
        this._health = health;
        health.OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = _foregroundImage.fillAmount;
        float elapsed = 0f;

        while(elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            _foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed);
            yield return null;
        }
        _foregroundImage.fillAmount = pct;
        Debug.Log(pct);
    }

    private void LateUpdate()
    {
        CachedTransform.position = _cam.WorldToScreenPoint(_health.CachedTransform.position + Vector3.up * _posOffset);
    }

    private void OnDestroy()
    {
        _health.OnHealthPctChanged -= HandleHealthChanged;
    }
}
