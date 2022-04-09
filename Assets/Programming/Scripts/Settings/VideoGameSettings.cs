using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSettings;
using KinematicCharacterController.Examples;
using UnityEngine.UI;

public class VideoGameSettings : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] private SO_Settings _soSettings;

    [Header("Sliders")] 
    [SerializeField] private SliderSettings _mouseSensivitySlider;

    [Header("Toggles")] 
    [SerializeField] private Toggle _invertMouseXToggle;
    [SerializeField] private Toggle _invertMouseYToggle;

    // Private Variables
    private const string _sliderMouseSensivity = "SL_MouseSensivity";

    public void ReceiveSliderValue(Slider slider, float value)
    {
        switch (slider.name)
        {
            case _sliderMouseSensivity:
                _soSettings.mouseSensivityValue = _mouseSensivitySlider.ConvertVirtualToActualValue(
                    _mouseSensivitySlider.Slider.minValue, _mouseSensivitySlider.Slider.maxValue,
                    _mouseSensivitySlider.MinSliderValue, _mouseSensivitySlider.MaxSliderValue, value);
                break;
        }
    }

    // Whenever the user changes the slider, it updates immediately
    private void SliderChanged(SliderSettings sliderSettings)
    {
        switch (sliderSettings.Slider.name)
        {
            case _sliderMouseSensivity:
                sliderSettings.SliderVirtualToActualConversionWithoutRound(_mouseSensivitySlider, sliderSettings.Slider.value);
                break;
        }
    }

    public void MouseInvertX(bool boxChecked)
    {
        _invertMouseXToggle.SetIsOnWithoutNotify(boxChecked);
        _soSettings.mouseXInvertValue = boxChecked;
    }

    public void MouseInvertY(bool boxChecked)
    {
        _invertMouseYToggle.SetIsOnWithoutNotify(boxChecked);
        _soSettings.mouseYInvertValue = boxChecked;
    }

    private void Initialize()
    {
        SetValues();

        _invertMouseXToggle.SetIsOnWithoutNotify(_soSettings.mouseXInvertValue);
        _invertMouseYToggle.SetIsOnWithoutNotify(_soSettings.mouseYInvertValue);
    }

    private void SetValues()
    {
        _mouseSensivitySlider.Slider.value =
            _mouseSensivitySlider.SliderActualToVirtualConversionWithoutRound(_mouseSensivitySlider,
                _soSettings.mouseSensivityValue);
    }

    private void ButtonEvents()
    {
        _mouseSensivitySlider.Slider.onValueChanged.AddListener(delegate { SliderChanged(_mouseSensivitySlider); });
        _invertMouseXToggle.onValueChanged.AddListener(delegate { MouseInvertX(_invertMouseXToggle.isOn); });
        _invertMouseYToggle.onValueChanged.AddListener(delegate { MouseInvertY(_invertMouseYToggle.isOn); });
    }

    private void Start()
    {
        ButtonEvents();
        Initialize();
    }
}
