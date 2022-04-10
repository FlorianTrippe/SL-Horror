using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Vector3 _elevatorUp;
    [SerializeField] private Vector3 _elevatorDown;
    [SerializeField] private GameObject _elevatorCabin;
    [SerializeField] private bool _elevatorStartUp;
    [SerializeField] private float _elevatorSpeed;

    private bool _ready = true;
    private bool _elevatorActive;

    void Start()
    {
        if (_elevatorStartUp)
            _elevatorCabin.transform.position = _elevatorUp;
        else
            _elevatorCabin.transform.position = _elevatorDown;
    }

    public void ActivateElevator()
    {
        _elevatorActive = !_elevatorActive;
    }

    public void RideElevator()
    {
        if (!_ready || !_elevatorActive) return;

        switch (_elevatorStartUp)
        {
            case false:
                _elevatorCabin.transform.DOMove(_elevatorUp, _elevatorSpeed);
                break;
            default:
                _elevatorCabin.transform.DOMove(_elevatorDown, _elevatorSpeed);
                break;
        }

        _elevatorStartUp = !_elevatorStartUp;

    }
}
