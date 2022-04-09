using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBattery : Battery
{
    [SerializeField] private Light _light;
    [SerializeField] private float _maxLightIntensity;

    protected override void Update()
    {
        base.Update();
        _light.intensity = _maxLightIntensity * _currentCharge / _standardChargingCapacity;
    }
}
