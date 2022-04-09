using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Settings")]
    public class SO_Settings : ScriptableObject
    {
        [Header("Video Settings")]
        [Attribute_ReadOnly] public int resolutionValue;
        [Attribute_ReadOnly] public int windowModeValue;
        public float brightnessValue;
        public float renderDistanceValue;
        public bool vSyncValue;
        public int antiAliasingValue;
        public int shadowQualityValue;
        public bool softShadowsValue;
        public int textureQualityValue;
        //public int qualityPresetValue;
        public int anisotropicTexturesValue;
        public bool softParticlesValue;
        public bool realtimeReflectionProbesValue;
        public bool billboardsFacingCameraPositionsValue;
        public int skinWeightsValue;
        public int lODBiasValue;
        public int particleRaycastBudgetValue;

        [Header("Game Settings")] 
        public float mouseSensivityValue;
        public bool mouseXInvertValue;
        public bool mouseYInvertValue;
    }
}