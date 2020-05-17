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
    [HideInInspector] public bool paused;

    private InputActions _input;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();

        _input.PlayerControls.Pause.started += ctx => ShowPauseMenu();
    }

    private void ShowPauseMenu()
    {
        if (!pauseMenu.activeSelf && !configMenu.activeSelf)
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
        if (!configMenu.activeSelf)
        {
            configMenu.SetActive(true);
        }
    }

    public void HideConfigMenu()
    {
        if(configMenu.activeSelf) configMenu.SetActive(false);
        if(paused) pauseMenu.SetActive(true);
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
