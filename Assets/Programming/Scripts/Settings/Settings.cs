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
    public class Settings : MonoBehaviour
    {
        public UniversalRenderPipelineAsset Urp;
        public VolumeProfile PostProcessVolume;

        [Header("Scriptable Objects")]
        public SO_Settings SOSettings;
        public SO_Settings DefaultSettings;
        public SO_GameObject CameraGO;

        [Header("Pop Up")] 
        [SerializeField] private GameObject _resPopUp;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _revertButton;
        [SerializeField] private int _maxPopUpTimer = 10;
        [SerializeField] private TextMeshProUGUI _popUpText;

        [Header("Dropdowns")]
        public TMP_Dropdown ResolutionDD;
        public TMP_Dropdown WindowModeDD;
        public TMP_Dropdown AntiAliasingDD;
        public TMP_Dropdown ShadowQualityDD;
        public TMP_Dropdown TextureQualityDD;
        //public TMP_Dropdown QualityPresetDD;
        public TMP_Dropdown AnisotropicTexturesDD;
        public TMP_Dropdown SkinWeightsDD;
        public TMP_Dropdown LODBiasDD;
        public TMP_Dropdown ParticleRaycastBudgetDD;

        [Header("Sliders")]
        public SliderSettings BrightnessSlider;
        public SliderSettings RenderDistanceSlider;

        [Header("Toggles")] 
        public Toggle VSyncToggle;
        public Toggle SoftShadowsToggle;
        public Toggle SoftParticlesToggle;
        public Toggle RealtimeReflectionProbesToggle;
        public Toggle BillboardsFacingCameraPositionsToggle;

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
                ResolutionDD.options.Add(new TMP_Dropdown.OptionData(ResolutionToString(resolutions[i])));
                _supportedResolutions.Add(resolutions[i]);
            }

            ResolutionDD.value = SOSettings.resolutionValue;
            ResolutionDD.RefreshShownValue();
        }

        // Set window mode
        private void WindowModeOptions(int index)
        {
            switch (index)
            {
                case 0:
                    WindowModeDD.value = 0;
                    _windowMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    WindowModeDD.value = 1;
                    _windowMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    WindowModeDD.value = 2;
                    _windowMode = FullScreenMode.FullScreenWindow;
                    break;
            }
            
            Screen.SetResolution(Screen.width, Screen.height, _windowMode);
            WindowModeDD.RefreshShownValue();
        }

        // Handle Pop Up
        private void PopUpHandler(int index, TMP_Dropdown dropdown)
        {
            if (!_resPopUp.activeSelf)
            {
                _resPopUp.SetActive(true);
                _acceptButton.onClick.RemoveAllListeners();
                _revertButton.onClick.RemoveAllListeners();

                if (dropdown == ResolutionDD)
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
            if (dropdown == ResolutionDD)
            {
                _previousResolutionIndex = newIndex;
                SOSettings.resolutionValue = newIndex;
            }
            else
            {
                _previousWindowModeIndex = newIndex;
                SOSettings.windowModeValue = newIndex;
            }
            
            SaveAndLoadSettings.SaveSettings(SOSettings);
            ClosePopUp();
        }

        // Revert to previous resolution or window mode
        private void Revert(TMP_Dropdown dropdown, int index)
        {
            if (dropdown == ResolutionDD)
                SetResolution(index);
            else
                SetWindowMode(index);

            ClosePopUp();
        }

        // Applies the new resolution and saves it to the file / SO
        private void SetResolution(int index)
        {
            Screen.SetResolution(_supportedResolutions[index].width, _supportedResolutions[index].height, _windowMode);
            ResolutionDD.value = index;
            ResolutionDD.RefreshShownValue();
            SaveAndLoadSettings.SaveSettings(SOSettings);
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
                    if (dropdown == ResolutionDD)
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
                    SOSettings.brightnessValue = BrightnessSlider.ConvertVirtualToActualValue(
                        BrightnessSlider.Slider.minValue, BrightnessSlider.Slider.maxValue,
                        BrightnessSlider.MinSliderValue, BrightnessSlider.MaxSliderValue, value);
                    break;
                case _sliderRenderDistance:
                    SOSettings.renderDistanceValue = RenderDistanceSlider.ConvertVirtualToActualValue(
                        RenderDistanceSlider.Slider.minValue, RenderDistanceSlider.Slider.maxValue,
                        RenderDistanceSlider.MinSliderValue, RenderDistanceSlider.MaxSliderValue, value);
                    break;
            }

            SaveAndLoadSettings.SaveSettings(SOSettings);
        }

        // Whenever the user changes the slider, it updates immediately
        private void SliderChanged(SliderSettings sliderSettings)
        {
            switch (sliderSettings.Slider.name)
            {
                case _sliderBrightness:
                    if (PostProcessVolume.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
                        colorAdjustments.postExposure.Override(sliderSettings.SliderVirtualToActualConversion(BrightnessSlider, sliderSettings.Slider.value));
                    break;
                case _sliderRenderDistance:
                    CameraGO.GameObject.GetComponent<Camera>().farClipPlane = sliderSettings.SliderVirtualToActualConversion(RenderDistanceSlider,
                                RenderDistanceSlider.Slider.value);
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

            // Set Toggle value and set SOSettings value
            VSyncToggle.SetIsOnWithoutNotify(boxChecked);
            SOSettings.vSyncValue = boxChecked;
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
                        Urp.msaaSampleCount = 1;
                        break;
                    case 2: // MSAA 2X
                        Urp.msaaSampleCount = 2;
                        break;
                    case 3: // MSAA 4X
                        Urp.msaaSampleCount = 4;
                        break;
                    case 4: // MSAA 8X
                        Urp.msaaSampleCount = 8;
                        break;
                    case 5: // SMAA LOW / MSAA: Disabled
                        Urp.msaaSampleCount = 1;
                        CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.Low;
                        break;
                    case 6: // SMAA MEDIUM / MSAA: Disabled
                        Urp.msaaSampleCount = 1;
                        CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.Medium;
                        break;
                    case 7: // SMAA HIGH / MSAA: Disabled
                        Urp.msaaSampleCount = 1;
                        CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.High;
                        break;
                }
            }

            // Camera Control
            switch (index)
            {
                case 1: 
                    CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                    break;
                case 5: 
                    CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    break;
                case 6:
                    CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    break;
                case 7:
                    CameraGO.GameObject.GetComponent<Camera>().GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    break;
            }

            // Set Dropdown value and set SOSettings value
            AntiAliasingDD.SetValueWithoutNotify(index);
            SOSettings.antiAliasingValue = index;
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

            // Set Dropdown value and set SOSettings value
            ShadowQualityDD.SetValueWithoutNotify(index);
            SOSettings.shadowQualityValue = index;
        }

        // Get and set the properties
        private void SetShadows(ShadowmaskMode mask, bool mainLightCastShadows, ShadowResolution res, LightRenderingMode additionalLights, int perObjectLights, bool additionalLightsCastShadows, ShadowResolution additionalLightsRes, LightCookieResolution additionalLightsCookieRes, LightCookieFormat additionalLightsCookieFormat, bool probeBlending, bool boxProjection, float shadowDistance, int shadowCascades)
        {
            QualitySettings.shadowmaskMode = mask;
            UnityBullshit.MainLightCastShadows = mainLightCastShadows;
            UnityBullshit.MainLightShadowResolution = res;
            UnityBullshit.AdditionalLightsRenderingMode = additionalLights;
            Urp.maxAdditionalLightsCount = perObjectLights;
            UnityBullshit.AdditionalLightCastShadows = additionalLightsCastShadows;
            UnityBullshit.AdditionalLightShadowResolution = additionalLightsRes;
            UnityBullshit.AdditionalLightsCookieResolution = additionalLightsCookieRes;
            UnityBullshit.AdditionalLightsCookieFormat = additionalLightsCookieFormat;
            UnityBullshit.ReflectionProbeBlending = probeBlending;
            UnityBullshit.ReflectionProbeBoxProjection= boxProjection;
            Urp.shadowDistance = shadowDistance;
            Urp.shadowCascadeCount = shadowCascades;
        }

        // Set Soft Shadows
        public void SoftShadows(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                UnityBullshit.SoftShadowsEnabled = boxChecked;
            }

            // Set Toggle value and set SOSettings value
            SoftShadowsToggle.SetIsOnWithoutNotify(boxChecked);
            SOSettings.softShadowsValue = boxChecked;
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

            // Set Dropdown value and set SOSettings value
            TextureQualityDD.SetValueWithoutNotify(index);
            SOSettings.textureQualityValue = index;
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

            AnisotropicTexturesDD.SetValueWithoutNotify(index);
            SOSettings.anisotropicTexturesValue = index;
        }
        public void SoftParticles(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                Urp.supportsCameraDepthTexture = boxChecked;
            }

            // Set Toggle value and set SOSettings value
            SoftParticlesToggle.SetIsOnWithoutNotify(boxChecked);
            SOSettings.softParticlesValue = boxChecked;
        }

        public void RealtimeReflectionProbes(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                QualitySettings.realtimeReflectionProbes = boxChecked;
            }

            // Set Toggle value and set SOSettings value
            RealtimeReflectionProbesToggle.SetIsOnWithoutNotify(boxChecked);
            SOSettings.realtimeReflectionProbesValue = boxChecked;
        }

        public void BillboardsFacingCameraPositions(bool boxChecked, bool customPreset)
        {
            if (!customPreset)
            {
                //SetToCustom();

                QualitySettings.billboardsFaceCameraPosition = boxChecked;
            }

            // Set Toggle value and set SOSettings value
            BillboardsFacingCameraPositionsToggle.SetIsOnWithoutNotify(boxChecked);
            SOSettings.billboardsFacingCameraPositionsValue = boxChecked;
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

            SkinWeightsDD.SetValueWithoutNotify(index);
            SOSettings.skinWeightsValue = index;
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

            LODBiasDD.SetValueWithoutNotify(index);
            SOSettings.lODBiasValue = index;
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

            ParticleRaycastBudgetDD.SetValueWithoutNotify(index);
            SOSettings.particleRaycastBudgetValue = index;
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
        //    SOSettings.qualityPresetValue = index;
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
        //            //RenderDistance(RenderDistanceSlider.Slider.value, false);
        //            VerticalSync(VSyncToggle.isOn, false);
        //            AntiAliasing(AntiAliasingDD.value, false);
        //            ShadowQuality(ShadowQualityDD.value, false);
        //            SoftShadows(SoftShadowsToggle.isOn, false);
        //            TextureQuality(TextureQualityDD.value, false);
        //            AnisotropicTextures(AnisotropicTexturesDD.value, false);
        //            SoftParticles(SoftParticlesToggle.isOn, false);
        //            RealtimeReflectionProbes(RealtimeReflectionProbesToggle.isOn, false);
        //            BillboardsFacingCameraPositions(BillboardsFacingCameraPositionsToggle.isOn, false);
        //            SkinWeights(SkinWeightsDD.value, false);
        //            LODBias(LODBiasDD.value, false);
        //            ParticleRaycastBudget(ParticleRaycastBudgetDD.value, false);
        //            break;
        //    }
        //}

        //private void SetToCustom()
        //{
        //    if (QualityPresetDD.value != 5)
        //    {
        //        QualityPresetDD.value = 5;
        //        SOSettings.qualityPresetValue = QualityPresetDD.value;
        //    }
        //}

        #endregion

        #region Start Operations

        // Operations before loading file and SO
        private void Initialize()
        {
            AddResolution();
            WindowModeOptions(SOSettings.windowModeValue);

            // Values
            _resPopUp.SetActive(false);
            _previousResolutionIndex = ResolutionDD.value;
            _previousWindowModeIndex = WindowModeDD.value;
        }

        // Add button functions
        private void ButtonEvents()
        {
            WindowModeDD.onValueChanged.AddListener(delegate { PopUpHandler(WindowModeDD.value, WindowModeDD); });
            ResolutionDD.onValueChanged.AddListener(delegate { PopUpHandler(ResolutionDD.value, ResolutionDD); });
            BrightnessSlider.Slider.onValueChanged.AddListener(delegate { SliderChanged(BrightnessSlider); });
            RenderDistanceSlider.Slider.onValueChanged.AddListener(delegate { SliderChanged(RenderDistanceSlider); });
            VSyncToggle.onValueChanged.AddListener(delegate { VerticalSync(VSyncToggle.isOn, false); });
            AntiAliasingDD.onValueChanged.AddListener(delegate { AntiAliasing(AntiAliasingDD.value, false); });
            ShadowQualityDD.onValueChanged.AddListener(delegate { ShadowQuality(ShadowQualityDD.value, false); });
            SoftShadowsToggle.onValueChanged.AddListener(delegate { SoftShadows(SoftShadowsToggle.isOn, false); });
            TextureQualityDD.onValueChanged.AddListener(delegate { TextureQuality(TextureQualityDD.value, false); });
            //QualityPresetDD.onValueChanged.AddListener(delegate { QualityPreset(QualityPresetDD.value); });
            AnisotropicTexturesDD.onValueChanged.AddListener(delegate { AnisotropicTextures(AnisotropicTexturesDD.value, false); });
            SoftParticlesToggle.onValueChanged.AddListener(delegate { SoftParticles(SoftParticlesToggle.isOn, false); });
            RealtimeReflectionProbesToggle.onValueChanged.AddListener(delegate { RealtimeReflectionProbes(RealtimeReflectionProbesToggle.isOn, false); });
            BillboardsFacingCameraPositionsToggle.onValueChanged.AddListener(delegate { BillboardsFacingCameraPositions(BillboardsFacingCameraPositionsToggle.isOn, false); });
            SkinWeightsDD.onValueChanged.AddListener(delegate { SkinWeights(SkinWeightsDD.value, false); });
            LODBiasDD.onValueChanged.AddListener(delegate { LODBias(LODBiasDD.value, false); });
            ParticleRaycastBudgetDD.onValueChanged.AddListener(delegate { ParticleRaycastBudget(ParticleRaycastBudgetDD.value, false); });
        }

        private void SetValues()
        {
            BrightnessSlider.Slider.value =
                BrightnessSlider.SliderActualToVirtualConversion(BrightnessSlider, SOSettings.brightnessValue);
            RenderDistanceSlider.Slider.value =
                RenderDistanceSlider.SliderActualToVirtualConversion(RenderDistanceSlider,
                    SOSettings.renderDistanceValue);
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