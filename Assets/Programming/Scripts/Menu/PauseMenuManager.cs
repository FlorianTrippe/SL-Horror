using System;
using System.Collections;
using System.Collections.Generic;
using GameSettings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] private SO_Settings _sOSettings;

    [Header("Menus")] 
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private List<PopUpButton> _popUps;

    private void CloseMenu()
    {
        bool popUp = false;

        foreach (PopUpButton item in _popUps)
        {
            if (item.PopUp.activeSelf)
            {
                item.DeclineButton.onClick.Invoke();
                popUp = true;
            }
        }

        if (!popUp)
        {
            if (_settingsMenu.activeSelf)
            {
                //When the user exits the settings menu, save
                SaveAndLoadSettings.SaveSettings(_sOSettings);
                _settingsMenu.SetActive(false);
            }
            else 
                _pauseMenu.SetActive(false);
        }
    }

    public void OpenMenu()
    {
        if (_pauseMenu.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}

[Serializable]
public class PopUpButton
{
    public GameObject PopUp;
    public Button DeclineButton;
}