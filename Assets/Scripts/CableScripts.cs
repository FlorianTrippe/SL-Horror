using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using UnityEngine;
using DG.Tweening;
using KinematicCharacterController.Examples;
using UnityEngine.UI;

public class CableScripts : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private GameObject _bonfireKey;
    [SerializeField] private GameObject _itemSpawnPoint;
    [SerializeField] private GameObject[] _geigerTargetArray;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private ExamplePlayer _player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject _zeigerVater;
    [SerializeField] private Vector3 _maxRotation;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _deathCanvas;
    [SerializeField] private GameObject _youDiedSFX;
    [SerializeField] private Animator _youDiedAnimator;

    [Header("Geiger Counter")]
    [SerializeField] private float _geigerChargeDrain;
    [SerializeField] private float _geigerSoundTime;
    [SerializeField] private float _geigerMinDistance;
    [SerializeField] private GameObject _geigerOnOff;
    [SerializeField] private Light _geigerLight;

    [Header("Flash Light")]
    [SerializeField] private float _flashLightChargeDrain;
    [SerializeField] private float _maxFlashLightLightIntensity;
    [SerializeField] private Light _flashLightLight;

    [Header("Filter")]
    [SerializeField] private float _maxFilterCharge;
    [SerializeField] private float _filterDrain;
    [SerializeField] private float _timerWithoutFilter;
    [SerializeField] private Slider _filterSlider;
    [SerializeField] private GameObject _vignette;

    [Header("Noise")]
    [SerializeField] private float _bonfireDropSoundFallOffDistance;
    [SerializeField] private float _chargeSoundFallOffDistance;
    [SerializeField] private float _geigerSoundFallOffDistance;
    [SerializeField] private float _filterChangeSoundFallOffDistance;

    [Header("Charging")]
    [SerializeField] private float _chargingSpeed;
    [SerializeField] private float _chargingDrain;
    [SerializeField] private float _maxCharge;
    [SerializeField] private Slider _chargeSlider;

    private bool _hasBonfireKey;
    private bool _hasCharger;
    private bool _hasFlashLight;
    private bool _hasGeiger;
    public bool HasGasMask;
    private bool _geigerEquipped;

    private int _filterCount;

    [HideInInspector] public float ChargingState;
    [HideInInspector] public float FilterState;

    private ItemType _equippedItem;
    private ItemType _lastEquippedItem;
    private GameObject _lastBonfire;
    [HideInInspector] public GameObject OtherBattery;

    [HideInInspector] public bool CanCharge = true;
    [HideInInspector] public bool ChargingOwnBattery = true;

    private bool _itemOn;
    private float _time;
    private float _geigerTime;

    // Start is called before the first frame update
    void Start()
    {
        ChargingState = _maxCharge;
        FilterState = _maxFilterCharge;
        _chargeSlider.maxValue = _maxCharge;
        _filterSlider.maxValue = _maxFilterCharge;
        _chargeSlider.enabled = false;
        _filterSlider.enabled = false;
    }

    private void Update()
    {
        if (Vector3.Distance(_enemy.transform.position, transform.position) <= 2)
        {
            Debug.Log("Gefunden");
            StartCoroutine("WaitHalfSecond");
        }

        float totalChargeDrain = _chargingDrain;
        if (_geigerEquipped && _itemOn)
        {
            _geigerOnOff.SetActive(true);
            totalChargeDrain += _geigerChargeDrain;
            float distance = Vector3.Distance(transform.position, _geigerTargetArray[0].transform.position);

            distance = _geigerTargetArray.Select(obj => Vector3.Distance(transform.position, obj.transform.position)).Prepend(distance).Min();

            if (_geigerMinDistance >= distance)
            {
                float time = distance / _geigerMinDistance * _geigerSoundTime;

                if (_geigerTime < time)
                {
                    _geigerTime += Time.deltaTime;
                    _geigerLight.enabled = false;
                }
                else
                {
                    _geigerTime = 0;
                    _geigerLight.enabled = true;
                    SFXManager.Instance.PlayClip(SFXManager.Instance.SFXList[0]);
                    //TODO: Play Sound
                }
            }
            //trigger Sound / move indicator
        }
        else if(_geigerEquipped && !_itemOn)
        {
            _geigerOnOff.SetActive(false);
        }

        if (_equippedItem == ItemType.FlashLight && !_itemOn)
        {
            _flashLightLight.enabled = false;
        }
        else if (_equippedItem == ItemType.FlashLight && _itemOn)
        {
            _flashLightLight.enabled = true;
            _flashLightLight.intensity = ChargingState / _maxCharge * _maxFlashLightLightIntensity;
            totalChargeDrain += _flashLightChargeDrain;
        }

        if (FilterState>0)
            FilterState -= _filterDrain * Time.deltaTime;
        else
        {
            if (_time < _timerWithoutFilter)
            {
                _time += Time.deltaTime;
            }
            else
            {
                //TODO: Game Over
            }
        }

        if(ChargingState>0)
            ChargingState -= totalChargeDrain * Time.deltaTime;

        ChargeUpdate();
        _chargeSlider.value = ChargingState;
        _filterSlider.value = FilterState;
    }

    public void BonFireRespawn(GameObject bonFire)
    {
        _lastBonfire = bonFire;
    }
    private void UpdateItem()
    {
        _geigerEquipped = false;
        _flashLightLight.enabled = false;
        _itemOn = true;

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
    public void TurnOnOFF()
    {
        if (_equippedItem is ItemType.FlashLight or ItemType.Geiger)
            _itemOn = !_itemOn;
    }
    public void PlaceBonFireKey()
    {
        _anim.SetBool("KeyPickUp", false);
        _anim.SetBool("KeyPutAway", true);
        NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _bonfireDropSoundFallOffDistance);
        _hasBonfireKey = false;
        _equippedItem = _lastEquippedItem;
        UpdateItem();
    }
    private void ChargeUpdate()
    {
        if(ChargingState <= _maxCharge && ChargingState != 0)
        {
            float merkmal = _maxRotation.y / 100;
            float prozent = ChargingState / _maxCharge;
            _zeigerVater.transform.rotation = new Quaternion(0, _maxRotation.y/prozent, 0,0);
        }
    }
    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log(other.gameObject.name);

    //    if(other.gameObject.name == "Enemy")
    //    {
    //        StartCoroutine("WaitHalfSecond");
    //        //TODO Leben abziehen etc
    //    }
    //}

    private IEnumerator WaitHalfSecond()
    {
        yield return new WaitForSeconds(0.5f);
        _deathCanvas.SetActive(true);
        _youDiedSFX.SetActive(true);
        _youDiedAnimator.SetTrigger("Died");
        yield return new WaitForSeconds(7f);
        Respawn();
    }

    public void Respawn()
    {
        if (_lastBonfire != null)
        {
            _deathCanvas.SetActive(false);
            _youDiedSFX.SetActive(false);
            gameObject.transform.parent.transform.position = _lastBonfire.transform.GetChild(0).transform.position;
        }
    }
    public void ChargingOtherBattery(GameObject battery)
    {
        _player.ReadyToCharge = true;
        ChargingOwnBattery = false;
        OtherBattery = battery;
        EquipCharger();
    }
    public void Charge()
    {
        if (CanCharge)
        {
            NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _chargeSoundFallOffDistance);
            if (ChargingOwnBattery)
            {
                ChargingState += _chargingSpeed;
                if (ChargingState >= _maxCharge)
                    ChargingState = _maxCharge;
            }
            else
            {
                Battery battery = OtherBattery.GetComponent<Battery>();
                battery.Charge();
            }
        }
    }
    public void EquipLastItem()
    {
        _flashLightLight.enabled = false;
        _geigerLight.enabled = false;
        _anim.SetBool("ChargerAway", true);
        _anim.SetBool("ChargerOut", false);
        _player.ReadyToCharge = false;
        OtherBattery = null;
        _equippedItem = _lastEquippedItem;
        switch (_equippedItem)
        {
            case ItemType.FlashLight:
                EquipFlashLight();
                break;
            case ItemType.Geiger:
                EquipGeiger();
                break;
            default:
                break;
        }
        UpdateItem();
    }
    public void ChangeFilter()
    {
        if (HasGasMask)
        {
            NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _filterChangeSoundFallOffDistance);
            if (_filterCount > 0)
            {
                _anim.SetBool("Reload", true);
                DropKey();
                if (_equippedItem != ItemType.BonfireKey && _equippedItem != ItemType.Charger && _equippedItem != ItemType.Filter)
                    _lastEquippedItem = _equippedItem;
                _equippedItem = ItemType.Filter;
                FilterState = _maxFilterCharge;
                _time = 0f;
                //TODO: hide / unhide Object
                UpdateItem();
            }
            else
            {
                //You are fucked
            }
        }
    }
    public void PlayFootstepSound()
    {
        SFXManager.Instance.PlayClip(SFXManager.Instance.Steps[0]);
    }
    public void EquipFlashLight()
    {
        if (_hasFlashLight)
        {
            _anim.SetBool("LampAway", !_anim.GetBool("LampAway"));
            _anim.SetBool("LampOut", !_anim.GetBool("LampOut"));
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
            _anim.SetBool("GeigerIn", !_anim.GetBool("GeigerIn"));
            _anim.SetBool("GeigerOut", !_anim.GetBool("GeigerOut"));
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
            _anim.SetBool("GeigerOut", false);
            _anim.SetBool("LampOut", false);

            _anim.SetBool("GeigerIn", true);
            _anim.SetBool("LampAway", true);

            _anim.SetBool("ChargerAway", !_anim.GetBool("ChargerAway"));
            _anim.SetBool("ChargerOut", !_anim.GetBool("ChargerOut"));
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
            _anim.SetBool("KeyPickUp", false);
            _anim.SetBool("KeyPutAway", true);
            NoiseManager.NoiseManagerReference.SetNoise(transform.position, false, _bonfireDropSoundFallOffDistance);
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

        _anim.SetBool("ChargerOut", false);
        _anim.SetBool("GeigerOut", false);
        _anim.SetBool("LampOut", false);

        _anim.SetBool("ChargerAway", true);
        _anim.SetBool("GeigerIn", true);
        _anim.SetBool("LampAway", true);

        _anim.SetBool("KeyPickUp", true);
        _anim.SetBool("KeyPutAway", false);

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
                _chargeSlider.enabled = true;
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
            case ItemType.GasMask:
                _filterSlider.enabled = true;
                _vignette.SetActive(true);
                HasGasMask = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public void Interact()
    {
        Debug.DrawRay(Camera.transform.position,Camera.transform.forward * 2f,Color.green);
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, 2f, _interactableLayerMask))
        {
            Interactable interact = hit.transform.gameObject.GetComponent<Interactable>();
            interact.Interact(this.gameObject);
        }
    }
    public void dings()
    {

    }
    public void HoldInteract()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.blue);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, _interactableLayerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out BonFireDing fire))
            {
                fire.Hold();
            }
        }
    }
}
