using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameObject")]
    public class SO_GameObject : ScriptableObject
    {
        public GameObject GameObject;
    }
}