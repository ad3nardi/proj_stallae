using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_glow : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _curBright;
    [SerializeField] private float _brightAdjust;
    [SerializeField] private float _flickerSpeedScaler;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _curBright = _light.intensity;
    }

    private void Update()
    {
        UpdateLightIntesity();
    }
    
    private void UpdateLightIntesity()
    {
        _light.intensity = Mathf.PingPong(Time.time * _flickerSpeedScaler, _brightAdjust);
        _light.intensity += _curBright;

    }
}
