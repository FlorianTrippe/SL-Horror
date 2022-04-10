using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EChargeState
{
    Empty,
    Normal,
    Critical,
    Danger
}

public class ModularBattery : Battery
{
    public UnityEvent BurnOutEvent;
    public UnityEvent DoStuffEvent;
    public UnityEvent StopChargeEvent;
    public UnityEvent ChargeEmptyEvent;
    public UnityEvent<float, EChargeState, float> ChargeEvent;

    private EChargeState _chargeState;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_currentCharge > 0)
        {
            float distance = Vector3.Distance(_enemy.transform.position, transform.position);

            if (distance > _minDistanceEnemyDischargeBoost)
            {
                _currentCharge -= _ChargeDrain * Time.deltaTime;
            }
            else
            {
                _currentCharge -= (_ChargeDrain + (1 / distance * _chargeDrainBoostMax)) * Time.deltaTime;
            }
            ChargeEvent?.Invoke(_currentCharge, _chargeState, _chargeDeadZone);
        }
        else
        {
            ChargeEmptyEvent?.Invoke();
        }
    }
    public override void Charge()
    {
        Debug.Log("Charge");
        _currentCharge += _charge;
        if (_currentCharge<=0)
        {
            _chargeState = EChargeState.Empty;
        }
        else if (_currentCharge < _standardChargingCapacity)
        {
            _chargeState = EChargeState.Normal;
        }
        else if (_currentCharge <= _chargeDangerZone)
        {
            _chargeState = EChargeState.Critical;
        }
        else if (_currentCharge <= _chargeDangerZone)
        {
            _chargeState = EChargeState.Danger;
        }
        else if (_currentCharge > _chargeDeadZone)
        {
            BurnOut();
        }
    }

    protected override void BurnOut()
    {
        BurnOutEvent?.Invoke();
    }

    protected override void DoStuff()
    {
        DoStuffEvent?.Invoke();
    }

    public override void StopCharge()
    {
        StopChargeEvent?.Invoke();
    }
}
