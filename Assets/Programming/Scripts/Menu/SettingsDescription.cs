using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _settingsDescription;
    [TextArea(3, 5)] public string Description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _settingsDescription.text = Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _settingsDescription.text = "";
    }
}
