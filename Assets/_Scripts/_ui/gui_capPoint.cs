using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gui_capPoint : OptimizedBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Image _foregroundImage;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _updateSpeedSeconds = 0.5f;
    [SerializeField] private float _posOffset;

    public ms_capPoints _capPoint;

    private void OnEnable()
    {
        _cam = Helpers.Camera;
    }

    public void SetProgress(ms_capPoints capPoint)
    {
        this._capPoint = capPoint;
        _capPoint.OnProgressPctChanged += HandleProgressChanged;
    }

    private void HandleProgressChanged(float pct)
    {
        float pctShow = Mathf.Floor(pct *100);
        _text.text = pctShow+"%";
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = _foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            _foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed);
            yield return null;
        }
        _foregroundImage.fillAmount = pct;
    }

    private void LateUpdate()
    {
        CachedTransform.position = _cam.WorldToScreenPoint(_capPoint.CachedTransform.position + Vector3.up * _posOffset);
    }

    private void OnDestroy()
    {
        _capPoint.OnProgressPctChanged -= HandleProgressChanged;
    }
}
