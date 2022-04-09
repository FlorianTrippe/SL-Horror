using System;
using System.Collections;
using System.Collections.Generic;
using GameSettings;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private VolumeProfile _postProcessVolume;

    [Header("SO_Settings")]
    [SerializeField] private SO_Settings _soSettings;
    [SerializeField] private SO_Settings _defaultSettings;
    [SerializeField] private SO_GameObject _soCamera;
    //[SerializeField] private SO_GameObject _soPlayer;

    [Header("Settings Components")] 
    [SerializeField] private VideoGameSettings _gameMenu;
    [SerializeField] private VideoSettings _videoMenu;
    
    private FullScreenMode _screenMode;
    private ExamplePlayer _player;
    
    private void Start()
    {
        //_player = _soPlayer.GameObject.GetComponent<ExamplePlayer>();

        SettingsFile data = SaveAndLoadSettings.LoadSettings();
        if (data == null)
            AssignSettings(null);
        else
            AssignSettings(data);

        VideoGameSettings();
        VideoSettings();
    }

    #region Game Settings

    private void VideoGameSettings()
    { 
        //_player.MouseSensivity = _soSettings.mouseSensivityValue;
        //_player.MouseXInvert = _soSettings.mouseXInvertValue;
        //_player.MouseYInvert = _soSettings.mouseYInvertValue;
    }

    #endregion

    #region Video Settings

    private void VideoSettings()
    {
        SetResolutionAndWindowMode(_soSettings.resolutionValue, _soSettings.windowModeValue);
        if (_postProcessVolume.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
            colorAdjustments.postExposure.Override(_soSettings.brightnessValue);
        _soCamera.GameObject.GetComponent<Camera>().farClipPlane = _soSettings.renderDistanceValue;

        _videoMenu.VerticalSync(_soSettings.vSyncValue, false);
        _videoMenu.AntiAliasing(_soSettings.antiAliasingValue, false);
        _videoMenu.ShadowQuality(_soSettings.shadowQualityValue, false);
        _videoMenu.SoftShadows(_soSettings.softShadowsValue, false);
        _videoMenu.TextureQuality(_soSettings.textureQualityValue, false);
        _videoMenu.AnisotropicTextures(_soSettings.anisotropicTexturesValue, false);
        _videoMenu.SoftParticles(_soSettings.softParticlesValue, false);
        _videoMenu.RealtimeReflectionProbes(_soSettings.realtimeReflectionProbesValue, false);
        _videoMenu.BillboardsFacingCameraPositions(_soSettings.billboardsFacingCameraPositionsValue, false);
        _videoMenu.SkinWeights(_soSettings.skinWeightsValue, false);
        _videoMenu.LODBias(_soSettings.lODBiasValue, false);
        _videoMenu.ParticleRaycastBudget(_soSettings.particleRaycastBudgetValue, false);

        //// If a preset was chosen, preset with the given index will be accessed
        //if (_soSettings.qualityPresetValue < 5)
        //    //_videoMenu.QualityPreset(_soSettings.qualityPresetValue);
        //else
        //{
        //    //QualitySettings.SetQualityLevel(5);
        //    //RenderDistance(_soSettings.renderDistanceValue, false);
        //    _videoMenu.VerticalSync(_soSettings.vSyncValue, false);
        //    _videoMenu.AntiAliasing(_soSettings.antiAliasingValue, false);
        //    _videoMenu.ShadowQuality(_soSettings.shadowQualityValue, false);
        //    _videoMenu.SoftShadows(_soSettings.softShadowsValue, false);
        //    _videoMenu.TextureQuality(_soSettings.textureQualityValue, false);
        //    _videoMenu.AnisotropicTextures(_soSettings.anisotropicTexturesValue, false);
        //    _videoMenu.SoftParticles(_soSettings.softParticlesValue, false);
        //    _videoMenu.RealtimeReflectionProbes(_soSettings.realtimeReflectionProbesValue, false);
        //    _videoMenu.BillboardsFacingCameraPositions(_soSettings.billboardsFacingCameraPositionsValue, false);
        //    _videoMenu.SkinWeights(_soSettings.skinWeightsValue, false);
        //    _videoMenu.LODBias(_soSettings.lODBiasValue, false);
        //    _videoMenu.ParticleRaycastBudget(_soSettings.particleRaycastBudgetValue, false);
        //}
    }

    private void SetResolutionAndWindowMode(int resIndex, int windowModeIndex)
    {
        Resolution[] resolutions = Screen.resolutions;
        Array.Reverse(resolutions);

        switch (windowModeIndex)
        {
            case 0:
                _screenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                _screenMode = FullScreenMode.Windowed;
                break;
            case 2:
                _screenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, _screenMode);
    }

    public void AssignSettings(SettingsFile file)
    {
        // If the file does not exist. Find the default settings and assign them
        if (file == null)
        {
            // Default Video Settings
            _soSettings.resolutionValue = _defaultSettings.resolutionValue;
            _soSettings.windowModeValue = _defaultSettings.windowModeValue;
            _soSettings.brightnessValue = _defaultSettings.brightnessValue;
            _soSettings.renderDistanceValue = _defaultSettings.renderDistanceValue;
            _soSettings.vSyncValue = _defaultSettings.vSyncValue;
            _soSettings.antiAliasingValue = _defaultSettings.antiAliasingValue;
            _soSettings.shadowQualityValue = _defaultSettings.shadowQualityValue;
            _soSettings.softShadowsValue = _defaultSettings.softShadowsValue;
            _soSettings.textureQualityValue = _defaultSettings.textureQualityValue;
            //_soSettings.qualityPresetValue = _defaultSettings.qualityPresetValue;
            _soSettings.anisotropicTexturesValue = _defaultSettings.anisotropicTexturesValue;
            _soSettings.softParticlesValue = _defaultSettings.softParticlesValue;
            _soSettings.realtimeReflectionProbesValue = _defaultSettings.realtimeReflectionProbesValue;
            _soSettings.billboardsFacingCameraPositionsValue = _defaultSettings.billboardsFacingCameraPositionsValue;
            _soSettings.skinWeightsValue = _defaultSettings.skinWeightsValue;
            _soSettings.lODBiasValue = _defaultSettings.lODBiasValue;
            _soSettings.particleRaycastBudgetValue = _defaultSettings.particleRaycastBudgetValue;

            // Default Game Settings
            _soSettings.mouseSensivityValue = _defaultSettings.mouseSensivityValue;
            _soSettings.mouseXInvertValue = _defaultSettings.mouseXInvertValue;
            _soSettings.mouseYInvertValue = _defaultSettings.mouseYInvertValue;
        }
        // File exists
        else
        {
            // Video Settings
            _soSettings.resolutionValue = file.resolution;
            _soSettings.windowModeValue = file.windowMode;
            _soSettings.brightnessValue = file.brightness;
            _soSettings.renderDistanceValue = file.renderDistance;
            _soSettings.vSyncValue = file.vSync;
            _soSettings.antiAliasingValue = file.antiAliasing;
            _soSettings.shadowQualityValue = file.shadowQuality;
            _soSettings.softShadowsValue = file.softShadows;
            _soSettings.textureQualityValue = file.textureQuality;
            //_soSettings.qualityPresetValue = file.qualityPreset;
            _soSettings.anisotropicTexturesValue = file.anisotropicTextures;
            _soSettings.softParticlesValue = file.softParticles;
            _soSettings.realtimeReflectionProbesValue = file.realtimeReflectionProbes;
            _soSettings.billboardsFacingCameraPositionsValue = file.billboardsFacingCameraPositions;
            _soSettings.skinWeightsValue = file.skinWeights;
            _soSettings.lODBiasValue = file.lODBias;
            _soSettings.particleRaycastBudgetValue = file.particleRaycastBudget;

            // Game Settings
            _soSettings.mouseSensivityValue = file.mouseSensivity;
            _soSettings.mouseXInvertValue = file.mouseXInvert;
            _soSettings.mouseYInvertValue = file.mouseYInvert;
        }
    }

    #endregion
}