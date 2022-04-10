using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [Header("Menus")] 
    [SerializeField] private GameObject _settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        AssignButtonEvents();
    }

    private void Settings()
    {
        _settingsMenu.SetActive(true);
    }

    // Resume Game
    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    // Main Menu
    private void StartGame()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("6dungeon");
    }

    // Assign Button Events
    private void AssignButtonEvents()
    {
        _startGameButton.onClick.AddListener(delegate { StartGame(); });
        _settingsButton.onClick.AddListener(delegate { Settings(); });
        _quitButton.onClick.AddListener(delegate { Quit(); });
    }
}
