using System;
using System.Collections;
using System.Collections.Generic;
using GameSettings;
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
    [SerializeField] private SO_Settings _sOSettings;
    [SerializeField] private SO_Settings _defaultSettings;
    [SerializeField] private SO_GameObject _sOCamera;

    [Header("Settings Components")]
    [SerializeField] private Settings _videoMenu;

    private FullScreenMode _screenMode;
    
    private void Start()
    {
        SettingsFile data = SaveAndLoadSettings.LoadSettings();
        if (data == null)
            AssignSettings(null);
        else
            AssignSettings(data);

        VideoSettings();
    }

    #region Video Settings

    private void VideoSettings()
    {
        SetResolutionAndWindowMode(_sOSettings.resolutionValue, _sOSettings.windowModeValue);
        if (_postProcessVolume.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
            colorAdjustments.postExposure.Override(_sOSettings.brightnessValue);
        _sOCamera.GameObject.GetComponent<Camera>().farClipPlane = _sOSettings.renderDistanceValue;

        _videoMenu.VerticalSync(_sOSettings.vSyncValue, false);
        _videoMenu.AntiAliasing(_sOSettings.antiAliasingValue, false);
        _videoMenu.ShadowQuality(_sOSettings.shadowQualityValue, false);
        _videoMenu.SoftShadows(_sOSettings.softShadowsValue, false);
        _videoMenu.TextureQuality(_sOSettings.textureQualityValue, false);
        _videoMenu.AnisotropicTextures(_sOSettings.anisotropicTexturesValue, false);
        _videoMenu.SoftParticles(_sOSettings.softParticlesValue, false);
        _videoMenu.RealtimeReflectionProbes(_sOSettings.realtimeReflectionProbesValue, false);
        _videoMenu.BillboardsFacingCameraPositions(_sOSettings.billboardsFacingCameraPositionsValue, false);
        _videoMenu.SkinWeights(_sOSettings.skinWeightsValue, false);
        _videoMenu.LODBias(_sOSettings.lODBiasValue, false);
        _videoMenu.ParticleRaycastBudget(_sOSettings.particleRaycastBudgetValue, false);

        //// If a preset was chosen, preset with the given index will be accessed
        //if (_sOSettings.qualityPresetValue < 5)
        //    //_videoMenu.QualityPreset(_sOSettings.qualityPresetValue);
        //else
        //{
        //    //QualitySettings.SetQualityLevel(5);
        //    //RenderDistance(_sOSettings.renderDistanceValue, false);
        //    _videoMenu.VerticalSync(_sOSettings.vSyncValue, false);
        //    _videoMenu.AntiAliasing(_sOSettings.antiAliasingValue, false);
        //    _videoMenu.ShadowQuality(_sOSettings.shadowQualityValue, false);
        //    _videoMenu.SoftShadows(_sOSettings.softShadowsValue, false);
        //    _videoMenu.TextureQuality(_sOSettings.textureQualityValue, false);
        //    _videoMenu.AnisotropicTextures(_sOSettings.anisotropicTexturesValue, false);
        //    _videoMenu.SoftParticles(_sOSettings.softParticlesValue, false);
        //    _videoMenu.RealtimeReflectionProbes(_sOSettings.realtimeReflectionProbesValue, false);
        //    _videoMenu.BillboardsFacingCameraPositions(_sOSettings.billboardsFacingCameraPositionsValue, false);
        //    _videoMenu.SkinWeights(_sOSettings.skinWeightsValue, false);
        //    _videoMenu.LODBias(_sOSettings.lODBiasValue, false);
        //    _videoMenu.ParticleRaycastBudget(_sOSettings.particleRaycastBudgetValue, false);
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
            _sOSettings.resolutionValue = _defaultSettings.resolutionValue;
            _sOSettings.windowModeValue = _defaultSettings.windowModeValue;
            _sOSettings.brightnessValue = _defaultSettings.brightnessValue;
            _sOSettings.renderDistanceValue = _defaultSettings.renderDistanceValue;
            _sOSettings.vSyncValue = _defaultSettings.vSyncValue;
            _sOSettings.antiAliasingValue = _defaultSettings.antiAliasingValue;
            _sOSettings.shadowQualityValue = _defaultSettings.shadowQualityValue;
            _sOSettings.softShadowsValue = _defaultSettings.softShadowsValue;
            _sOSettings.textureQualityValue = _defaultSettings.textureQualityValue;
            //_sOSettings.qualityPresetValue = _defaultSettings.qualityPresetValue;
            _sOSettings.anisotropicTexturesValue = _defaultSettings.anisotropicTexturesValue;
            _sOSettings.softParticlesValue = _defaultSettings.softParticlesValue;
            _sOSettings.realtimeReflectionProbesValue = _defaultSettings.realtimeReflectionProbesValue;
            _sOSettings.billboardsFacingCameraPositionsValue = _defaultSettings.billboardsFacingCameraPositionsValue;
            _sOSettings.skinWeightsValue = _defaultSettings.skinWeightsValue;
            _sOSettings.lODBiasValue = _defaultSettings.lODBiasValue;
            _sOSettings.particleRaycastBudgetValue = _defaultSettings.particleRaycastBudgetValue;
        }
        // File exists
        else
        {
            // Video Settings
            _sOSettings.resolutionValue = file.resolution;
            _sOSettings.windowModeValue = file.windowMode;
            _sOSettings.brightnessValue = file.brightness;
            _sOSettings.renderDistanceValue = file.renderDistance;
            _sOSettings.vSyncValue = file.vSync;
            _sOSettings.antiAliasingValue = file.antiAliasing;
            _sOSettings.shadowQualityValue = file.shadowQuality;
            _sOSettings.softShadowsValue = file.softShadows;
            _sOSettings.textureQualityValue = file.textureQuality;
            //_sOSettings.qualityPresetValue = file.qualityPreset;
            _sOSettings.anisotropicTexturesValue = file.anisotropicTextures;
            _sOSettings.softParticlesValue = file.softParticles;
            _sOSettings.realtimeReflectionProbesValue = file.realtimeReflectionProbes;
            _sOSettings.billboardsFacingCameraPositionsValue = file.billboardsFacingCameraPositions;
            _sOSettings.skinWeightsValue = file.skinWeights;
            _sOSettings.lODBiasValue = file.lODBias;
            _sOSettings.particleRaycastBudgetValue = file.particleRaycastBudget;
        }
    }

    #endregion
}