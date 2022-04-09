using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

namespace GameSettings
{
    public class VideoSettings : MonoBehaviour
    {
        [SerializeField] private UniversalRenderPipelineAsset _urp;
        [SerializeField] private VolumeProfile _postProcessVolume;

        [Header("Scriptable Objects")]
        [SerializeField] private SO_Settings _soSettings;
        [SerializeField] private SO_Settings _defaultSettings;
        [SerializeField] private SO_GameObject _soCamera;

        [Header("Pop Up")] 
        [SerializeField] private GameObject _resPopUp;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _revertButton;
        [SerializeField] private int _maxPopUpTimer = 10;
        [SerializeField] private TextMeshProUGUI _popUpText;

        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown _resolutionDD;
        [SerializeField] private TMP_Dropdown _windowModeDD;
        [SerializeField] private TMP_Dropdown _antiAliasingDD;
        [SerializeField] private TMP_Dropdown _shadowQualityDD;
        [SerializeField] private TMP_Dropdown _textureQualityDD;
        //public TMP_Dropdown QualityPresetDD;
        [SerializeField] private TMP_Dropdown _anisotropicTexturesDD;
        [SerializeField] private TMP_Dropdown _skinWeightsDD;
        [SerializeField] private TMP_Dropdown _lodBiasDD;
        [SerializeField] private TMP_Dropdown _particleRaycastBudgetDD;

        [Header("Sliders")]
        [SerializeField] private SliderSettings _brightnessSlider;
        [SerializeField] private SliderSettings _renderDistanceSlider;

        [Header("Toggles")] 
        [SerializeField] private Toggle _vSyncToggle;
        [SerializeField] private Toggle _softShadowsToggle;
        [SerializeField] private Toggle _softParticlesToggle;
        [SerializeField] private Toggle _realtimeReflectionProbesToggle;
        [SerializeField] private Toggle _billboardsFacingCameraPositionsToggle;

        private List<Resolution> _supportedResolutions;
        private FullScreenMode _windowMode;
        private int _previousWindowModeIndex;
        private int _previousResolutionIndex;

        private const string _sliderRenderDistance = "SL_RenderDistance";
        private const string _sliderBrightness = "SL_Brightness";
        
        #region Resolution and Display // Adds _supportedResolutions to dropdown based on the users supported _supportedResolutions

        // Adds all supported resolutions at the current screen refresh rate and skip every resolution below 1280x720
        private void AddResolution()
        {
            _supportedResolutions = new List<Resolution>();
            Resolution[] resolutions = Screen.resolutions;
            Array.Reverse(resolutions);
            for (int i = 0; i < resolutions.Length; i++)
            {
                _resolutionDD.options.Add(new TMP_Dropdown.OptionData(ResolutionToString(resolutions[i])));
                _supportedResolutions.Add(resolutions[i]);
            }

            _resolutionDD.value = _soSettings.resolutionValue;
            _resolutionDD.RefreshShownValue();
        }

        // Set window mode
        private void WindowModeOptions(int index)
        {
            switch (index)
            {
                case 0:
                    _windowModeDD.value = 0;
                    _windowMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    _windowModeDD.value = 1;
                    _windowMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    _windowModeDD.value = 2;
                    _windowMode = FullScreenMode.FullScreenWindow;
                    break;
            }
            
            Screen.SetResolution(Screen.width, Screen.height, _windowMode);
            _windowModeDD.RefreshShownValue();
        }

        // Handle Pop Up
        private void PopUpHandler(int index, TMP_Dropdown dropdown)
        {
            if (!_resPopUp.activeSelf)
            {
                _resPopUp.SetActive(true);
                _acceptButton.onClick.RemoveAllListeners();
                _revertButton.onClick.RemoveAllListeners();

                if (dropdown == _resolutionDD)
                {
                    SetResolution(index);
                    _acceptButton.onClick.AddListener(delegate { Apply(dropdown, index); });
                    _revertButton.onClick.AddListener(delegate { Revert(dropdown, _previousResolutionIndex); });
                }
                else
                {
                    SetWindowMode(index);
                    _acceptButton.onClick.AddListener(delegate { Apply(dropdown, index); });
                    _revertButton.onClick.AddListener(delegate { Revert(dropdown, _previousWindowModeIndex); });
                }

                StartCoroutine("PopUpTimer", dropdown);
            }
        }

        // Apply and set new index
        private void Apply(TMP_Dropdown dropdown, int newIndex)
        {
            if (dropdown == _resolutionDD)
            {
                _previousResolutionIndex = newIndex;
                _soSettings.resolutionValue = newIndex;
            }
            else
            {
                _previousWindowModeIndex = newIndex;
                _soSettings.windowModeValue = newIndex;
            }
            
            SaveAndLoadSettings.SaveSettings(_soSettings);
            ClosePopUp();
        }

        // Revert to previous resolution or window mode
        private void Revert(TMP_Dropdown dropdown, int index)
        {
            if (dropdown == _resolutionDD)
                SetResolution(index);
            else
                SetWindowMode(index);

            ClosePopUp();
        }

        // Applies the new resolution and saves it to the file / SO
        private void SetResolution(int index)
        {
            Screen.SetResolution(_supportedResolutions[index].width, _supportedResolutions[index].height, _windowMode);
            _resolutionDD.value = index;
            _resolutionDD.RefreshShownValue();
            SaveAndLoadSettings.SaveSettings(_soSettings);
        }

        // Applies the new window mode and saves it to the file / SO
        private void SetWindowMode(int index)
        {
            WindowModeOptions(index);
            StartCoroutine("SetWindowModeAtEnd");
        }

        // Close Pop Up
        private void ClosePopUp()
        {
            _resPopUp.SetActive(false);
            StopCoroutine("PopUpTimer");
        }

        // Sets the resolution and window mode at next fixed Update
        private IEnumerator SetWindowModeAtEnd()
        {
            yield return new WaitForFixedUpdate();
            Screen.SetResolution(Screen.width, Screen.height, _windowMode);
        }

        // Pop Up Timer
        private IEnumerator PopUpTimer(TMP_Dropdown dropdown)
        {
            int currentTimer = _maxPopUpTimer;
            while (currentTimer >= 0)
            {
                _popUpText.text = $"Apply this resolution? Settings will be restored in {currentTimer} Seconds.";
                yield return new WaitForSecondsRealtime(1);
                currentTimer--;

                if (currentTimer < 0)
                {
                    if (dropdown == _resolutionDD)
                        Revert(dropdown, _previousResolutionIndex);
                    else
                        Revert(dropdown, _previousWindowModeIndex);
                }
            }
        }

        public void ReceiveSliderValue(Slider slider, float value)
        {
            switch (slider.name)
            {
                case _sliderBrightness:
                    _soSettings.brightnessValue = _brightnessSlider.ConvertVirtualToActualValue(
                        _brightnessSlider.Slider.minValue, _brightnessSlider.Slider.maxValue,
                        _brightnessSlider.MinSliderValue, _brightnessSlider.MaxSliderValue, value);
                    break;
                case _sliderRenderDistance:
                    _soSettings.renderDistanceValue = _renderDistanceSlider.ConvertVirtualToActualValue(
                        _renderDistanceSlider.Slider.minValue, _renderDistanceSlider.Slider.maxValue,
                        _renderDistanceSlider.MinSliderValue, _renderDistanceSlider.MaxSliderValue, value);
                    break;
            }
        }

        // Whenever the user changes the slider, it updates immediately
        private void SliderChanged(SliderSettings sliderSettings)
        {
            switch (sliderSettings.Slider.name)
            {
                case _sliderBrightness:
                    if (_postProcessVolume.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
                        colorAdjustments.postExposure.Override(sliderSettings.SliderVirtualToActualConversionInt(_brightnessSlider, sliderSettings.Slider.value));
                    break;
                case _sliderRenderDistance:
                    _soCamera.GameObject.GetComponent<Camera>().farClipPlane = sliderSettings.SliderVirtualToActualConversionInt(_renderDistanceSlider,
                                _renderDistanceSlider.Slider.value);
                    break;
            }
        }

        // Set V-Sync
        public void VerticalSync(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                QualitySettings.vSyncCount = boxChecked ? 1 : 0;
            }

            // Set Toggle value and set _soSettings value
            _vSyncToggle.SetIsOnWithoutNotify(boxChecked);
            _soSettings.vSyncValue = boxChecked;
        }

        // Set Anti-Aliasing
        public void AntiAliasing(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                // Project Setting / URP Control
                switch (index)
                {
                    case 1: // FXAA / MSAA: Disabled
                        _urp.msaaSampleCount = 1;
                        break;
                    case 2: // MSAA 2X
                        _urp.msaaSampleCount = 2;
                        break;
                    case 3: // MSAA 4X
                        _urp.msaaSampleCount = 4;
                        break;
                    case 4: // MSAA 8X
                        _urp.msaaSampleCount = 8;
                        break;
                    case 5: // SMAA LOW / MSAA: Disabled
                        _urp.msaaSampleCount = 1;
                        _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.Low;
                        break;
                    case 6: // SMAA MEDIUM / MSAA: Disabled
                        _urp.msaaSampleCount = 1;
                        _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.Medium;
                        break;
                    case 7: // SMAA HIGH / MSAA: Disabled
                        _urp.msaaSampleCount = 1;
                        _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.High;
                        break;
                }
            }

            // Camera Control
            switch (index)
            {
                case 1: 
                    _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                    break;
                case 5: 
                    _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    break;
                case 6:
                    _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    break;
                case 7:
                    _soCamera.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    break;
            }

            // Set Dropdown value and set _soSettings value
            _antiAliasingDD.SetValueWithoutNotify(index);
            _soSettings.antiAliasingValue = index;
        }

        // Set Shadow Quality
        public void ShadowQuality(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                switch (index)
                {
                    case 0: // Shadow Quality ULTRA
                        SetShadows(ShadowmaskMode.DistanceShadowmask, true, ShadowResolution._4096, LightRenderingMode.PerPixel,
                            8, true, ShadowResolution._4096, LightCookieResolution._4096, LightCookieFormat.ColorHDR, true, true, 150, 4);
                        break;
                    case 1: // Shadow Quality HIGH
                        SetShadows(ShadowmaskMode.DistanceShadowmask, true, ShadowResolution._2048, LightRenderingMode.PerPixel,
                            4, true, ShadowResolution._2048, LightCookieResolution._2048, LightCookieFormat.ColorHigh, true, true, 70, 3);
                        break;
                    case 2: // Shadow Quality MEDIUM
                        SetShadows(ShadowmaskMode.DistanceShadowmask, true, ShadowResolution._1024, LightRenderingMode.PerPixel,
                            2, true, ShadowResolution._1024, LightCookieResolution._1024, LightCookieFormat.GrayscaleHigh, true, true, 40, 2);
                        break;
                    case 3: // Shadow Quality LOW
                        SetShadows(ShadowmaskMode.Shadowmask, true, ShadowResolution._512, LightRenderingMode.Disabled,
                            0, false, ShadowResolution._512, LightCookieResolution._512, LightCookieFormat.ColorLow, false, false, 20, 1);
                        break;
                    case 4: // Shadow Quality VERY LOW
                        SetShadows(ShadowmaskMode.Shadowmask, true, ShadowResolution._256, LightRenderingMode.Disabled,
                            0, false, ShadowResolution._256, LightCookieResolution._256, LightCookieFormat.GrayscaleLow, false, false, 15, 1);
                        break;
                    case 5: // Shadow Quality OFF
                        SetShadows(ShadowmaskMode.Shadowmask, false, ShadowResolution._256, LightRenderingMode.Disabled,
                            0, false, ShadowResolution._256, LightCookieResolution._256, LightCookieFormat.GrayscaleLow, false, false, 10, 1);
                        break;
                }
            }

            // Set Dropdown value and set _soSettings value
            _shadowQualityDD.SetValueWithoutNotify(index);
            _soSettings.shadowQualityValue = index;
        }

        // Get and set the properties
        private void SetShadows(ShadowmaskMode mask, bool mainLightCastShadows, ShadowResolution res, LightRenderingMode additionalLights, int perObjectLights, bool additionalLightsCastShadows, ShadowResolution additionalLightsRes, LightCookieResolution additionalLightsCookieRes, LightCookieFormat additionalLightsCookieFormat, bool probeBlending, bool boxProjection, float shadowDistance, int shadowCascades)
        {
            QualitySettings.shadowmaskMode = mask;
            UnityBullshit.MainLightCastShadows = mainLightCastShadows;
            UnityBullshit.MainLightShadowResolution = res;
            UnityBullshit.AdditionalLightsRenderingMode = additionalLights;
            _urp.maxAdditionalLightsCount = perObjectLights;
            UnityBullshit.AdditionalLightCastShadows = additionalLightsCastShadows;
            UnityBullshit.AdditionalLightShadowResolution = additionalLightsRes;
            UnityBullshit.AdditionalLightsCookieResolution = additionalLightsCookieRes;
            UnityBullshit.AdditionalLightsCookieFormat = additionalLightsCookieFormat;
            UnityBullshit.ReflectionProbeBlending = probeBlending;
            UnityBullshit.ReflectionProbeBoxProjection= boxProjection;
            _urp.shadowDistance = shadowDistance;
            _urp.shadowCascadeCount = shadowCascades;
        }

        // Set Soft Shadows
        public void SoftShadows(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                UnityBullshit.SoftShadowsEnabled = boxChecked;
            }

            // Set Toggle value and set _soSettings value
            _softShadowsToggle.SetIsOnWithoutNotify(boxChecked);
            _soSettings.softShadowsValue = boxChecked;
        }

        public void TextureQuality(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                switch (index)
                {
                    case 0: // Texture Quality Fullres / ULTRA
                        QualitySettings.masterTextureLimit = 0;
                        break;
                    case 1: // Texture Quality 1/2 Resolution / HIGH
                        QualitySettings.masterTextureLimit = 1;
                        break;
                    case 2: // Texture Quality 1/4 Resolution / MEDIUM
                        QualitySettings.masterTextureLimit = 2;
                        break;
                    case 3: // Texture Quality 1/8 Resolution / LOW
                        QualitySettings.masterTextureLimit = 3;
                        break;
                }
            }

            // Set Dropdown value and set _soSettings value
            _textureQualityDD.SetValueWithoutNotify(index);
            _soSettings.textureQualityValue = index;
        }
        
        public void AnisotropicTextures(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                switch (index)
                {
                    case 0:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                        break;
                    case 1:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                        break;
                    case 2:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                        break;
                }
            }

            _anisotropicTexturesDD.SetValueWithoutNotify(index);
            _soSettings.anisotropicTexturesValue = index;
        }
        public void SoftParticles(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                _urp.supportsCameraDepthTexture = boxChecked;
            }

            // Set Toggle value and set _soSettings value
            _softParticlesToggle.SetIsOnWithoutNotify(boxChecked);
            _soSettings.softParticlesValue = boxChecked;
        }

        public void RealtimeReflectionProbes(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                QualitySettings.realtimeReflectionProbes = boxChecked;
            }

            // Set Toggle value and set _soSettings value
            _realtimeReflectionProbesToggle.SetIsOnWithoutNotify(boxChecked);
            _soSettings.realtimeReflectionProbesValue = boxChecked;
        }

        public void BillboardsFacingCameraPositions(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                QualitySettings.billboardsFaceCameraPosition = boxChecked;
            }

            // Set Toggle value and set _soSettings value
            _billboardsFacingCameraPositionsToggle.SetIsOnWithoutNotify(boxChecked);
            _soSettings.billboardsFacingCameraPositionsValue = boxChecked;
        }

        public void SkinWeights(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                switch (index)
                {
                    case 0:
                        QualitySettings.skinWeights = UnityEngine.SkinWeights.OneBone;
                        break;
                    case 1:
                        QualitySettings.skinWeights = UnityEngine.SkinWeights.TwoBones;
                        break;
                    case 2:
                        QualitySettings.skinWeights = UnityEngine.SkinWeights.FourBones;
                        break;
                    case 3:
                        QualitySettings.skinWeights = UnityEngine.SkinWeights.Unlimited;
                        break;
                }
            }

            _skinWeightsDD.SetValueWithoutNotify(index);
            _soSettings.skinWeightsValue = index;
        }

        public void LODBias(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                switch (index)
                {
                    case 0:
                        QualitySettings.lodBias = 0.3f;
                        break;
                    case 1:
                        QualitySettings.lodBias = 0.7f;
                        break;
                    case 2:
                        QualitySettings.lodBias = 1f;
                        break;
                    case 3:
                        QualitySettings.lodBias = 1.5f;
                        break;
                    case 4:
                        QualitySettings.lodBias = 2f;
                        break;
                }
            }

            _lodBiasDD.SetValueWithoutNotify(index);
            _soSettings.lODBiasValue = index;
        }

        public void ParticleRaycastBudget(int index, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                switch (index)
                {
                    case 0:
                        QualitySettings.particleRaycastBudget = 128;
                        break;
                    case 1:
                        QualitySettings.particleRaycastBudget = 256;
                        break;
                    case 2:
                        QualitySettings.particleRaycastBudget = 512;
                        break;
                    case 3:
                        QualitySettings.particleRaycastBudget = 1024;
                        break;
                    case 4:
                        QualitySettings.particleRaycastBudget = 2048;
                        break;
                    case 5:
                        QualitySettings.particleRaycastBudget = 4096;
                        break;
                }
            }

            _particleRaycastBudgetDD.SetValueWithoutNotify(index);
            _soSettings.particleRaycastBudgetValue = index;
        }

        //public void QualityPreset(int index)
        //{
        //    switch (index)
        //    {
        //        case 0: // ULTRA
        //            QualitySettings.SetQualityLevel(4);
        //            break;
        //        case 1: // HIGH
        //            QualitySettings.SetQualityLevel(3);
        //            break;
        //        case 2: // MEDIUM
        //            QualitySettings.SetQualityLevel(2);
        //            break;
        //        case 3: // LOW
        //            QualitySettings.SetQualityLevel(1);
        //            break;
        //        case 4: // VERY LOW
        //            QualitySettings.SetQualityLevel(0);
        //            break;
        //        case 5: // CUSTOM
        //            QualitySettings.SetQualityLevel(5);
        //            break;
        //    }

        //    UpdateDropdowns(index);
        //    QualityPresetDD.SetValueWithoutNotify(index);
        //    _soSettings.qualityPresetValue = index;
        //}

        //private void UpdateDropdowns(int index)
        //{
        //    switch (index)
        //    {
        //        case 0: // ULTRA
        //            //RenderDistance(10f, true);
        //            VerticalSync(true, true);
        //            AntiAliasing(7, true);
        //            ShadowQuality(0, true);
        //            SoftShadows(true, true);
        //            TextureQuality(0, true);
        //            AnisotropicTextures(0, true);
        //            SoftParticles(true, true);
        //            RealtimeReflectionProbes(true, true);
        //            BillboardsFacingCameraPositions(true, true);
        //            SkinWeights(3, true);
        //            LODBias(4, true);
        //            ParticleRaycastBudget(5, true);
        //            break;
        //        case 1: // HIGH
        //            //RenderDistance(8f, true);
        //            VerticalSync(true, true);
        //            AntiAliasing(4, true);
        //            ShadowQuality(1, true);
        //            SoftShadows(true, true);
        //            TextureQuality(1, true);
        //            AnisotropicTextures(0, true);
        //            SoftParticles(true, true);
        //            RealtimeReflectionProbes(true, true);
        //            BillboardsFacingCameraPositions(true, true);
        //            SkinWeights(2, true);
        //            LODBias(3, true);
        //            ParticleRaycastBudget(4, true);
        //            break;
        //        case 2: // MEDIUM
        //            //RenderDistance(6f, true);
        //            VerticalSync(true, true);
        //            AntiAliasing(2, true);
        //            ShadowQuality(2, true);
        //            SoftShadows(false, true);
        //            TextureQuality(2, true);
        //            AnisotropicTextures(1, true);
        //            SoftParticles(true, true);
        //            RealtimeReflectionProbes(false, true);
        //            BillboardsFacingCameraPositions(true, true);
        //            SkinWeights(1, true);
        //            LODBias(2, true);
        //            ParticleRaycastBudget(3, true);
        //            break;
        //        case 3: // LOW
        //            //RenderDistance(4f, true);
        //            VerticalSync(false, true);
        //            AntiAliasing(1, true);
        //            ShadowQuality(3, true);
        //            SoftShadows(false, true);
        //            TextureQuality(3, true);
        //            AnisotropicTextures(1, true);
        //            SoftParticles(false, true);
        //            RealtimeReflectionProbes(false, true);
        //            BillboardsFacingCameraPositions(false, true);
        //            SkinWeights(1, true);
        //            LODBias(1, true);
        //            ParticleRaycastBudget(2, true);
        //            break;
        //        case 4: // VERY LOW
        //            //RenderDistance(2f, true);
        //            VerticalSync(false, true);
        //            AntiAliasing(0, true);
        //            ShadowQuality(4, true);
        //            SoftShadows(false, true);
        //            TextureQuality(3, true);
        //            AnisotropicTextures(2, true);
        //            SoftParticles(false, true);
        //            RealtimeReflectionProbes(false, true);
        //            BillboardsFacingCameraPositions(false, true);
        //            SkinWeights(0, true);
        //            LODBias(0, true);
        //            ParticleRaycastBudget(1, true);
        //            break;
        //        case 5: // CUSTOM
        //            //RenderDistance(_renderDistanceSlider.Slider.value, false);
        //            VerticalSync(_vSyncToggle.isOn, false);
        //            AntiAliasing(_antiAliasingDD.value, false);
        //            ShadowQuality(_shadowQualityDD.value, false);
        //            SoftShadows(_softShadowsToggle.isOn, false);
        //            TextureQuality(_textureQualityDD.value, false);
        //            AnisotropicTextures(_anisotropicTexturesDD.value, false);
        //            SoftParticles(_softParticlesToggle.isOn, false);
        //            RealtimeReflectionProbes(_realtimeReflectionProbesToggle.isOn, false);
        //            BillboardsFacingCameraPositions(_billboardsFacingCameraPositionsToggle.isOn, false);
        //            SkinWeights(_skinWeightsDD.value, false);
        //            LODBias(_lodBiasDD.value, false);
        //            ParticleRaycastBudget(_particleRaycastBudgetDD.value, false);
        //            break;
        //    }
        //}

        //private void SetToCustom()
        //{
        //    if (QualityPresetDD.value != 5)
        //    {
        //        QualityPresetDD.value = 5;
        //        _soSettings.qualityPresetValue = QualityPresetDD.value;
        //    }
        //}

        #endregion

        #region Start Operations

        // Operations before loading file and SO
        private void Initialize()
        {
            AddResolution();
            WindowModeOptions(_soSettings.windowModeValue);

            // Values
            _resPopUp.SetActive(false);
            _previousResolutionIndex = _resolutionDD.value;
            _previousWindowModeIndex = _windowModeDD.value;
        }

        // Add button functions
        private void ButtonEvents()
        {
            _windowModeDD.onValueChanged.AddListener(delegate { PopUpHandler(_windowModeDD.value, _windowModeDD); });
            _resolutionDD.onValueChanged.AddListener(delegate { PopUpHandler(_resolutionDD.value, _resolutionDD); });
            _brightnessSlider.Slider.onValueChanged.AddListener(delegate { SliderChanged(_brightnessSlider); });
            _renderDistanceSlider.Slider.onValueChanged.AddListener(delegate { SliderChanged(_renderDistanceSlider); });
            _vSyncToggle.onValueChanged.AddListener(delegate { VerticalSync(_vSyncToggle.isOn, false); });
            _antiAliasingDD.onValueChanged.AddListener(delegate { AntiAliasing(_antiAliasingDD.value, false); });
            _shadowQualityDD.onValueChanged.AddListener(delegate { ShadowQuality(_shadowQualityDD.value, false); });
            _softShadowsToggle.onValueChanged.AddListener(delegate { SoftShadows(_softShadowsToggle.isOn, false); });
            _textureQualityDD.onValueChanged.AddListener(delegate { TextureQuality(_textureQualityDD.value, false); });
            //QualityPresetDD.onValueChanged.AddListener(delegate { QualityPreset(QualityPresetDD.value); });
            _anisotropicTexturesDD.onValueChanged.AddListener(delegate { AnisotropicTextures(_anisotropicTexturesDD.value, false); });
            _softParticlesToggle.onValueChanged.AddListener(delegate { SoftParticles(_softParticlesToggle.isOn, false); });
            _realtimeReflectionProbesToggle.onValueChanged.AddListener(delegate { RealtimeReflectionProbes(_realtimeReflectionProbesToggle.isOn, false); });
            _billboardsFacingCameraPositionsToggle.onValueChanged.AddListener(delegate { BillboardsFacingCameraPositions(_billboardsFacingCameraPositionsToggle.isOn, false); });
            _skinWeightsDD.onValueChanged.AddListener(delegate { SkinWeights(_skinWeightsDD.value, false); });
            _lodBiasDD.onValueChanged.AddListener(delegate { LODBias(_lodBiasDD.value, false); });
            _particleRaycastBudgetDD.onValueChanged.AddListener(delegate { ParticleRaycastBudget(_particleRaycastBudgetDD.value, false); });
        }

        private void SetValues()
        {
            _brightnessSlider.Slider.value =
                _brightnessSlider.SliderActualToVirtualConversionInt(_brightnessSlider, _soSettings.brightnessValue);
            _renderDistanceSlider.Slider.value =
                _renderDistanceSlider.SliderActualToVirtualConversionInt(_renderDistanceSlider,
                    _soSettings.renderDistanceValue);
        }

        #endregion

        // Start is called before the first frame update

        void Start()
        {
            Initialize();
            ButtonEvents();
            SetValues();
        }

        #region Converter

        // Converts resolution to string
        private string ResolutionToString(Resolution res)
        {
            return res.width + " x " + res.height + " @ " + res.refreshRate + " Hz";
        }

        #endregion
    }
}