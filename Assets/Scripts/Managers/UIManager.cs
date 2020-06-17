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
    
    public static Action OnHeal = delegate {};

    public Camera mainCamera;
    [SerializeField] private float timeToShowMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject configMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject scannerWarning;
    [SerializeField] private LifeUILogic lifeUILogic;
    [SerializeField] private Animator fadeAnimator;
    [HideInInspector] public bool paused;
    [HideInInspector] public bool popUpEnabled;

    private InputActions _input;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();

        _input.PlayerControls.Pause.started += ctx => ShowPauseMenu();
    }

    private void OnDestroy()
    {
        _input.PlayerControls.Pause.started -= ctx => ShowPauseMenu();
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Play()
    {
        FadeIn();
        StartCoroutine(LoadFirstDialogueScene());     
    }

    IEnumerator LoadFirstDialogueScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("FirstDialogueScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void ShowPauseMenu()
    {
        if (!pauseMenu.activeSelf && !configMenu.activeSelf && SceneManager.GetActiveScene().name != "MainMenu")
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            paused = true;
            AudioManager.Instance.PauseAllEvents();
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            paused = false;
            AudioManager.Instance.ResumeAllEvents();
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

    public void Hit(int damage)
    {
        lifeUILogic.Hit(damage);
    }

    public void Heal()
    {
        lifeUILogic.Heal();
    }

    public void RestoreHealth()
    {
        lifeUILogic.RestoreHealth();
    }
    
    public IEnumerator ShowDeathMenu()
    {
        yield return new WaitForSeconds(timeToShowMenu);
        AudioManager.Instance.StopAllEvents(false);
        deathMenu.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowScannerWarning()
    {
        if (!scannerWarning.activeSelf)
        {
            scannerWarning.SetActive(true);
            StartCoroutine(Co_HideScannerWarning());
        }
    }

    private void HideScannerWarning()
    {
        if (scannerWarning.activeSelf)
        {
            scannerWarning.SetActive(false);
        }
    }
    
    private IEnumerator Co_HideScannerWarning()
    {
        yield return new WaitForSeconds(1.5f);
        HideScannerWarning();
    }

    public void DeathFade()
    {
        StartCoroutine(Co_DeathFade());
    }

    private IEnumerator Co_DeathFade()
    {
        yield return new WaitForSeconds(0.9f);
        FadeIn();
    }

    public void FadeIn()
    {
        fadeAnimator.SetTrigger("In");
    }
    public void FadeOut()
    {
        fadeAnimator.SetTrigger("Out");
    }
}
