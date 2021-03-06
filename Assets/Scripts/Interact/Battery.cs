using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : Interactable
{
    [SerializeField] protected float _standardChargingCapacity;
    [SerializeField] protected float _chargeDangerZone;
    [SerializeField] protected float _chargeDeadZone;
    [SerializeField] protected float _charge;
    [SerializeField] protected float _chargeDrainBoostMax;
    [SerializeField] protected float _ChargeDrain;
    [SerializeField] protected float _minDistanceEnemyDischargeBoost;
    [SerializeField] protected GameObject _enemy;

    [SerializeField] protected GameObject obj1;
    [SerializeField] protected Slider _slider;
    [SerializeField] protected Image _fill;

    [SerializeField] protected float _currentCharge;

    private Renderer _renderer;

    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        _slider.maxValue = _chargeDeadZone;
    }
    protected virtual void Update()
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

            _slider.value = _currentCharge;

            if (_currentCharge <= _standardChargingCapacity)
                _fill.color = Color.green;
            else if (_currentCharge <= _chargeDangerZone)
                _fill.color = Color.yellow;
            else if (_currentCharge <= _chargeDeadZone)
                _fill.color = Color.red;
        }
    }

    public virtual void Charge()
    {
        Debug.Log("Charge");
        _currentCharge += _charge;
        if (_currentCharge >= _chargeDangerZone)
        {
            
        }
        if (_currentCharge >= _chargeDeadZone)
            BurnOut();
    }

    protected virtual void BurnOut()
    {
        //Lampe putt
        Destroy(this);
    }

    public override void Interact(GameObject player)
    {
        player.GetComponent<CableScripts>().ChargingOtherBattery(this.gameObject);
        DoStuff();
    }

    public virtual void StopCharge()
    {
        obj1.SetActive(false);
    }

    protected virtual void DoStuff()
    {
        obj1.SetActive(true);
    }
}
