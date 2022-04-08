using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button _settingsButton;

    [Header("PopUp")] 
    [SerializeField] private GameObject _popUp;
    [SerializeField] private TextMeshProUGUI _popUpText;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _declineButton;

    [Header("Menus")] 
    [SerializeField] private GameObject _settingsMenu;

    // Handle exit game pop up
    private void HandleExitGame(string text)
    {
        _popUp.SetActive(true);
        _popUpText.text = text;
        _acceptButton.onClick.RemoveAllListeners();
        _declineButton.onClick.RemoveAllListeners();

        _acceptButton.onClick.AddListener(delegate { Application.Quit(); });
        _declineButton.onClick.AddListener(delegate { _popUp.SetActive(false); });
    }

    // Handle all pop ups
    //private void PopUp(string action)
    //{
    //    switch (action)
    //    {
    //        case "Exit":
    //            HandleExitGame();
    //    }
    //}

    // Open Settings
    private void Settings()
    {
        _settingsMenu.SetActive(true);
    }

    // Resume Game
    private void Resume()
    {
        gameObject.SetActive(false);
    }

    // Assign Button Events
    private void AssignButtonEvents()
    {
        _settingsButton.onClick.AddListener(delegate { Settings(); });
    }

    private void Start()
    {
        AssignButtonEvents();
    }

}