using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _settingsDescription;
    [TextArea(3, 5)] public string Description;

    public void SetText(string description)
    {
        _settingsDescription.text = description;
    }
}
