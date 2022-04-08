using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSettings
{
    public class InitializeScriptableObjectGO : MonoBehaviour
    {
        public SO_GameObject SOGameObject;
        public GameObject AssignWithThisGO;

        private void Awake()
        {
            SOGameObject.GameObject = AssignWithThisGO;
        }
    }
}