using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathMenuLogic : MonoBehaviour
{
    private enum DeathMenuOptions
    {
        TryAgain,
        MainMenu
    }

    private InputActions _input;
    private DeathMenuOptions _option;

    [SerializeField] private TextMeshProUGUI tryAgainImage;
    [SerializeField] private TextMeshProUGUI mainMenuImage;
    private PlayerMovementController _controller;
    public Animator tryAgainBgAnim;
    public Animator mainMenuBgAnim;

    [Header("Sounds")]
    [FMODUnity.EventRef] public string buttonNavigationSoundPath;
    [FMODUnity.EventRef] public string buttonClickSoundPath;

    private void Awake()
    {
        _input = new InputActions();
        _input.PlayerControls.MenuAccept.started += ctx => AcceptItem();
        _input.PlayerControls.MenuNavigationUp.started += ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started += ctx => ItemDown();
        _controller = FindObjectOfType<PlayerMovementController>();
    }
    
    private void OnEnable()
    {
        _input.Enable();
        _option = DeathMenuOptions.TryAgain;
        tryAgainImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        tryAgainImage.color = Color.black;
        tryAgainBgAnim.SetTrigger("enable");
        if(!_controller.scannerSword.HoldingSword() && _controller.scannerSword.SwordUnlocked()) _controller.scannerSword.SwordRecovered();
        _controller.scannerSword.LockSword();
        GameManager.Instance.Respawn();
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void ItemUp()
    {
        switch (_option)
        {
            case DeathMenuOptions.TryAgain:
                break;
            case DeathMenuOptions.MainMenu:
                ButtonNavigationSound();
                mainMenuImage.transform.localScale = new Vector3(1, 1, 1);
                mainMenuImage.color = Color.white;
                mainMenuBgAnim.SetTrigger("disable");
                tryAgainImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                tryAgainImage.color = Color.black;
                tryAgainBgAnim.SetTrigger("enable");
                _option = DeathMenuOptions.TryAgain;
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case DeathMenuOptions.TryAgain:
                ButtonNavigationSound();
                tryAgainImage.transform.localScale = new Vector3(1, 1, 1);
                tryAgainImage.color = Color.white;
                tryAgainBgAnim.SetTrigger("disable");
                mainMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                mainMenuImage.color = Color.black;
                mainMenuBgAnim.SetTrigger("enable");
                _option = DeathMenuOptions.MainMenu;
                break;
            case DeathMenuOptions.MainMenu:
                break;
        }
    }

    private void AcceptItem()
    {
        ButtonClickSound();
        switch (_option)
        {
            case DeathMenuOptions.TryAgain:
                Respawn();
                break;
            case DeathMenuOptions.MainMenu:
                MainMenu();
                break;
        }
    }

    public void Respawn() //Currently used by Button OnClick Event
    {
        UIManager.Instance.FadeOut();
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void MainMenu() //Currently used by Button OnClick Event
    {
        UIManager.Instance.MainMenu();
    }

    private void ButtonNavigationSound()
    {
        if (AudioManager.Instance.ValidEvent(buttonNavigationSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(buttonNavigationSoundPath, transform);
        }
    }

    private void ButtonClickSound()
    {
        if (AudioManager.Instance.ValidEvent(buttonClickSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(buttonClickSoundPath, transform);
        }
    }
}
