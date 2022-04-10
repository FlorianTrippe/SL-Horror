using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugScript : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _material;
    [SerializeField] private Color _noChargeColor;
    [SerializeField] private Color _normalChargeColor;
    [SerializeField] private Color _criticalChargeColor;
    [SerializeField] private Color _dangerChargeColor;
    [SerializeField] private float _emissionStrength;

    public void ChargeUpdate(float f, EChargeState state, float maxF)
    {
        switch (state)
        {
            case EChargeState.Empty:
                _renderer.material.SetColor("EmissionColor", _noChargeColor);
                break;
            case EChargeState.Normal:
                _renderer.material.SetColor("EmissionColor", _normalChargeColor);
                break;
            case EChargeState.Critical:
                _renderer.material.SetColor("EmissionColor", _criticalChargeColor);
                break;
            case EChargeState.Danger:
                _renderer.material.SetColor("EmissionColor", _dangerChargeColor);
                break;
        }

        float temp = _emissionStrength * f / maxF;
        _renderer.material.SetFloat("Emissionstrngstdt", temp);
    }

    public void ChargeEmpty()
    {

    }

    public void Activate()
    {

    }
}
