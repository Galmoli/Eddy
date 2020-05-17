using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }
    [SerializeField] private float timeToShowMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject configMenu;
    [SerializeField] private GameObject mainMenu;
    [HideInInspector] public bool paused;

    private InputActions _input;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();

        _input.PlayerControls.Pause.started += ctx => ShowPauseMenu();
    }

    public void Play()
    {
        SceneManager.LoadScene("Prototype_Level");
    }

    public void ExitGame()
    {
        print("ExitGame");
    }

    private void ShowPauseMenu()
    {
        if (!pauseMenu.activeSelf && !configMenu.activeSelf && SceneManager.GetActiveScene().name != "MainMenu")
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }

    public void ShowConfigMenu()
    {
        if(pauseMenu.activeSelf) pauseMenu.SetActive(false);
        if(SceneManager.GetActiveScene().name == "MainMenu") mainMenu.SetActive(false);
        if (!configMenu.activeSelf)
        {
            configMenu.SetActive(true);
        }
    }

    public void HideConfigMenu()
    {
        if(configMenu.activeSelf) configMenu.SetActive(false);
        if(paused) pauseMenu.SetActive(true);
        if(SceneManager.GetActiveScene().name == "MainMenu") mainMenu.SetActive(true);
    }
    
    public IEnumerator ShowDeathMenu()
    {
        var currentTime = 0f;
        while (currentTime < timeToShowMenu)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        deathMenu.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
