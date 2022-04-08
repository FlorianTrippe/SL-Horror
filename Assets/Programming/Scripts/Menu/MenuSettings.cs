using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using GameSettings;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private SO_Settings _sOSettings;

    [Header("Settings Buttons")] 
    [SerializeField] private Button _gameButton;
    [SerializeField] private Button _videoButton;
    [SerializeField] private Button _interfaceButton;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _closeButton;

    [Header("Menu Game Objects")]
    [SerializeField] private GameObject _menuGame;
    [SerializeField] private GameObject _menuVideo;
    [SerializeField] private GameObject _menuInterface;
    [SerializeField] private GameObject _menuAudio;

    [Header("Menus to set active again after closing")] 
    [SerializeField] private List<GameObject> _menusToEnable;

    private GameObject _currentMenu;

    private void OpenMenu(GameObject menu)
    {
        _currentMenu.SetActive(false);
        menu.SetActive(true);
        _currentMenu = menu;
    }

    private void CloseMenu()
    {
        if (_menusToEnable.Count > 0)
        {
            foreach (GameObject item in _menusToEnable)
            {
                item.SetActive(true);
            }
        }

        SaveAndLoadSettings.SaveSettings(_sOSettings);
        gameObject.SetActive(false);
    }

    private void AssingButtonEvents()
    {
        _gameButton.onClick.AddListener(delegate { OpenMenu(_menuGame); });
        _videoButton.onClick.AddListener(delegate { OpenMenu(_menuVideo); });
        _interfaceButton.onClick.AddListener(delegate { OpenMenu(_menuInterface); });
        _audioButton.onClick.AddListener(delegate { OpenMenu(_menuAudio); });
        _closeButton.onClick.AddListener(delegate { CloseMenu(); });
    }

    private void Start()
    {
        _currentMenu = _menuGame.activeSelf ? _menuGame :
            _menuVideo.activeSelf ? _menuVideo :
            _menuInterface.activeSelf ? _menuInterface :
            _menuAudio.activeSelf ? _menuAudio : null;

        if (_currentMenu == _menuGame)
            _gameButton.Select();
        
        AssingButtonEvents();
    }
}