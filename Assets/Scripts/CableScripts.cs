using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using UnityEngine;
using DG.Tweening;
using KinematicCharacterController.Examples;
using UnityEngine.UI;

public class CableScripts : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private GameObject _bonfireKey;
    [SerializeField] private float _bonfireDropSoundFallOffDistance;
    [SerializeField] private GameObject _charger;
    [SerializeField] private float _chargeSoundFallOffDistance;
    [SerializeField] private GameObject _flashLight;
    [SerializeField] private GameObject _geiger;
    [SerializeField] private float _geigerSoundFallOffDistance;
    [SerializeField] private GameObject _filter;
    [SerializeField] private float _filterChangeSoundFallOffDistance;
    [SerializeField] private GameObject _itemSpawnPoint;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private ExamplePlayer _player;
    [SerializeField] private Light _flashLightLight;
    [SerializeField] private Slider _chargeSlider;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private float _chargingSpeed;
    [SerializeField] private float _chargingDrain;
    [SerializeField] private float _maxCharge;
    [SerializeField] private GameObject Camera;

    private bool _hasBonfireKey;
    private bool _hasCharger;
    private bool _hasFlashLight;
    private bool _hasGeiger;
    private bool _chargingOwnBattery = true;
    private bool _geigerEquipped;

    private int _filterCount = 0;

    public float ChargingState;

    private ItemType _equippedItem;
    private ItemType _lastEquippedItem;
    private GameObject _lastBonfire;
    private GameObject _otherBattery;

    // Start is called before the first frame update
    void Start()
    {
        ChargingState = _maxCharge;
        _chargeSlider.maxValue = _maxCharge;
    }

    private void Update()
    {
        if (_geigerEquipped)
        {
            float distance = Vector3.Distance(transform.position, _enemy.transform.position);
            //trigger Sound / move indicator
        }

        ChargingState -= _chargingDrain * Time.deltaTime;

        _chargeSlider.value = ChargingState;
    }

    public void BonFireRespawn(GameObject bonFire)
    {
        _lastBonfire = bonFire;
    }
    
    private void UpdateItem()
    {
        _geigerEquipped = false;
        _flashLightLight.enabled = false;

        switch (_equippedItem)
        {
            case ItemType.None:
                break;
            case ItemType.BonfireKey:
                break;
            case ItemType.Charger:
                break;
            case ItemType.FlashLight:
                _flashLightLight.enabled = true;
                break;
            case ItemType.Geiger:
                _geigerEquipped = true;
                break;
            case ItemType.Filter:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PlaceBonFireKey()
    {

    }

    public void ChargingOtherBattery(GameObject battery)
    {
        _player.ReadyToCharge = true;
        _chargingOwnBattery = false;
        _otherBattery = battery;
        EquipCharger();
    }

    public void Charge()
    {
        NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _chargeSoundFallOffDistance);
        if (_chargingOwnBattery)
        {
            ChargingState += _chargingSpeed;
            if (ChargingState >= _maxCharge)
                ChargingState = _maxCharge;
        }
        else
        {
            Battery battery = _otherBattery.GetComponent<Battery>();
            battery.Charge();
        }
    }

    public void EquipLastItem()
    {
        _player.ReadyToCharge = false;
        _otherBattery = null;
        _equippedItem = _lastEquippedItem;
        UpdateItem();
    }
    public void ChangeFilter()
    {
        NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _filterChangeSoundFallOffDistance);
        if (_filterCount > 0)
        {
            DropKey();
            if (_equippedItem != ItemType.BonfireKey && _equippedItem != ItemType.Charger && _equippedItem != ItemType.Filter)
                _lastEquippedItem = _equippedItem;
            _equippedItem = ItemType.Filter;
            GameObject obj = Instantiate(_filter, _itemSpawnPoint.transform); //TODO: hide / unhide Object
            UpdateItem();
        }
        else
        {
            //You are fucked
        }
    }
    public void EquipFlashLight()
    {
        if (_hasFlashLight)
        {
            DropKey();
            if (_equippedItem != ItemType.BonfireKey && _equippedItem != ItemType.Charger && _equippedItem != ItemType.Filter)
                _lastEquippedItem = _equippedItem;
            _equippedItem = ItemType.FlashLight;
            //TODO: hide / unhide Object
            UpdateItem();
        }
    }
    public void EquipGeiger()
    {
        if (_hasGeiger)
        {
            DropKey();
            if (_equippedItem != ItemType.BonfireKey && _equippedItem != ItemType.Charger && _equippedItem != ItemType.Filter)
                _lastEquippedItem = _equippedItem;
            _equippedItem = ItemType.Geiger;
            //TODO: hide / unhide Object
            UpdateItem();
        }
    }
    public void EquipCharger()
    {
        if (_hasCharger)
        {
            DropKey();
            if (_equippedItem != ItemType.BonfireKey && _equippedItem != ItemType.Charger && _equippedItem != ItemType.Filter)
                _lastEquippedItem = _equippedItem;
            _equippedItem = ItemType.Charger;
            //TODO: hide / unhide Object
            UpdateItem();
        }
    }
    public void DropKey()
    {
        if (_hasBonfireKey)
        {
            NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _bonfireDropSoundFallOffDistance);
            Destroy(_itemSpawnPoint.transform.GetChild(_itemSpawnPoint.transform.childCount - 1).gameObject);
            _hasBonfireKey = false;
            Instantiate(_bonfireKey, transform.position, Quaternion.identity);
            _equippedItem = _lastEquippedItem;
            UpdateItem();
        }
    }
    public void PickUpBonFireKey()
    {
        if (_hasBonfireKey)
            return;

        _hasBonfireKey = true;
        GameObject obj = Instantiate(_bonfireKey, _itemSpawnPoint.transform);//TODO: hide / unhide Object
        obj.GetComponent<Collider>().enabled = false;
        if (_equippedItem != ItemType.BonfireKey && _equippedItem != ItemType.Charger && _equippedItem != ItemType.Filter)
            _lastEquippedItem = _equippedItem;
        _equippedItem = ItemType.BonfireKey;
        UpdateItem();
    }
    public void FillInventory(ItemType type)
    {
        switch (type)
        {
            case ItemType.BonfireKey:
                PickUpBonFireKey();
                break;
            case ItemType.Charger:
                _hasCharger = true;
                break;
            case ItemType.FlashLight:
                _hasFlashLight = true;
                break;
            case ItemType.Geiger:
                _hasGeiger = true;
                break;
            case ItemType.Filter:
                _filterCount++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public void Interact()
    {
        Debug.DrawRay(Camera.transform.position,Camera.transform.forward * 1.5f,Color.green);
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, 1.5f, _interactableLayerMask))
        {
            Interactable interact = hit.transform.gameObject.GetComponent<Interactable>();
            interact.Interact(this.gameObject);
        }
    }
    public void HoldInteract()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f, _interactableLayerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out BonFireDing fire))
            {
                fire.Hold();
            }
        }
    }
}
