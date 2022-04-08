using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSettings
{
    public static class LoadSettingsFile
    {
        public static SO_Settings SOSettings;
        public static SO_Settings DefaultSettings;

        public static void InitiateSettings()
        {
            SettingsFile data = SaveAndLoadSettings.LoadSettings();

            // File exists
            if (data != null)
            {
                // Video Settings
                SOSettings.resolutionValue = data.resolution;
                SOSettings.windowModeValue = data.windowMode;
                SOSettings.brightnessValue = data.brightness;
                SOSettings.renderDistanceValue = data.renderDistance;
                SOSettings.vSyncValue = data.vSync;
                SOSettings.antiAliasingValue = data.antiAliasing;
                SOSettings.shadowQualityValue = data.shadowQuality;
                SOSettings.softShadowsValue = data.softShadows;
                SOSettings.textureQualityValue = data.textureQuality;
                //SOSettings.qualityPresetValue = data.qualityPreset;
                SOSettings.anisotropicTexturesValue = data.anisotropicTextures;
                SOSettings.softParticlesValue = data.softParticles;
                SOSettings.realtimeReflectionProbesValue = data.realtimeReflectionProbes;
                SOSettings.billboardsFacingCameraPositionsValue = data.billboardsFacingCameraPositions;
                SOSettings.skinWeightsValue = data.skinWeights;
                SOSettings.lODBiasValue = data.lODBias;
                SOSettings.particleRaycastBudgetValue = data.particleRaycastBudget;
            }
            // If the file does not exist. Find the default settings and assign them
            else 
            {
                // Default Video Settings
                SOSettings.resolutionValue = DefaultSettings.resolutionValue;
                SOSettings.windowModeValue = DefaultSettings.windowModeValue;
                SOSettings.brightnessValue = DefaultSettings.brightnessValue;
                SOSettings.renderDistanceValue = DefaultSettings.renderDistanceValue;
                SOSettings.vSyncValue = DefaultSettings.vSyncValue;
                SOSettings.antiAliasingValue = DefaultSettings.antiAliasingValue;
                SOSettings.shadowQualityValue = DefaultSettings.shadowQualityValue;
                SOSettings.softShadowsValue = DefaultSettings.softShadowsValue;
                SOSettings.textureQualityValue = DefaultSettings.textureQualityValue;
                //SOSettings.qualityPresetValue = DefaultSettings.qualityPresetValue;
                SOSettings.anisotropicTexturesValue = DefaultSettings.anisotropicTexturesValue;
                SOSettings.softParticlesValue = DefaultSettings.softParticlesValue;
                SOSettings.realtimeReflectionProbesValue = DefaultSettings.realtimeReflectionProbesValue;
                SOSettings.billboardsFacingCameraPositionsValue = DefaultSettings.billboardsFacingCameraPositionsValue;
                SOSettings.skinWeightsValue = DefaultSettings.skinWeightsValue;
                SOSettings.lODBiasValue = DefaultSettings.lODBiasValue;
                SOSettings.particleRaycastBudgetValue = DefaultSettings.particleRaycastBudgetValue;
                SaveAndLoadSettings.SaveSettings(SOSettings);
            }
        }
    }
}