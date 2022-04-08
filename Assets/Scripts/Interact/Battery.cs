using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : Interactable
{
    [SerializeField] private float _standardChargingCapacity;
    [SerializeField] private float _chargeDangerZone;
    [SerializeField] private float _chargeDeadZone;
    [SerializeField] private float _charge;
    [SerializeField] private float _chargeDrainBoostMax;
    [SerializeField] private float _ChargeDrain;
    [SerializeField] private float _minDistanceEnemyDischargeBoost;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private GameObject obj1;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;

    [SerializeField] private float _currentCharge;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _slider.maxValue = _chargeDeadZone;
    }
    private void Update()
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

    public void Charge()
    {
        Debug.Log("Charge");
        _currentCharge += _charge;
        if (_currentCharge >= _chargeDangerZone)
        {
            
        }
        if (_currentCharge >= _chargeDeadZone)
            BurnOut();
    }

    private void BurnOut()
    {
        //Lampe putt
        Destroy(this);
    }

    public override void Interact(GameObject player)
    {
        player.GetComponent<CableScripts>().ChargingOtherBattery(this.gameObject);
        DoStuff();
    }

    public void StopCharge()
    {
        obj1.SetActive(false);
    }

    private void DoStuff()
    {
        obj1.SetActive(true);
    }
}
