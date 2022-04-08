using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSettings
{
    [Serializable]
    public class SettingsFile
    {
        // Video Settings
        public int resolution;
        public int windowMode;
        public float brightness;
        public float renderDistance;
        public bool vSync;
        public int antiAliasing;
        public int shadowQuality;
        public bool softShadows;
        public int textureQuality;
        //public int qualityPreset;
        public int anisotropicTextures;
        public bool softParticles;
        public bool realtimeReflectionProbes;
        public bool billboardsFacingCameraPositions;
        public int skinWeights;
        public int lODBias;
        public int particleRaycastBudget;

        public SettingsFile(SO_Settings settings)
        {
            resolution = settings.resolutionValue;
            windowMode = settings.windowModeValue;
            brightness = settings.brightnessValue;
            renderDistance = settings.renderDistanceValue;
            vSync = settings.vSyncValue;
            antiAliasing = settings.antiAliasingValue;
            shadowQuality = settings.shadowQualityValue;
            softShadows = settings.softShadowsValue;
            textureQuality = settings.textureQualityValue;
            //qualityPreset = settings.qualityPresetValue;
            anisotropicTextures = settings.anisotropicTexturesValue;
            softParticles = settings.softParticlesValue;
            realtimeReflectionProbes = settings.realtimeReflectionProbesValue;
            billboardsFacingCameraPositions = settings.billboardsFacingCameraPositionsValue;
            skinWeights = settings.skinWeightsValue;
            lODBias = settings.lODBiasValue;
            particleRaycastBudget = settings.particleRaycastBudgetValue;
        }
    }
}