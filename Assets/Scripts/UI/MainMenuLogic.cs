using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuLogic : MonoBehaviour
{
    private enum MainMenuOptions
    {
        Play,
        Options,
        Exit
    }
    private InputActions _input;
    private MainMenuOptions _option;

    [SerializeField] private TextMeshProUGUI playImage;
    [SerializeField] private TextMeshProUGUI optionsImage;
    [SerializeField] private TextMeshProUGUI exitImage;
    public Animator playBgAnim;
    public Animator optionsBgAnim;
    public Animator exitBgAnim;

    private void Awake()
    {
        _input = new InputActions();
        _input.PlayerControls.MenuAccept.started += ctx => AcceptItem();
        _input.PlayerControls.MenuNavigationUp.started += ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started += ctx => ItemDown();
    }

    private void OnDestroy()
    {
        _input.PlayerControls.MenuAccept.started -= ctx => AcceptItem();
        _input.PlayerControls.MenuNavigationUp.started -= ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started -= ctx => ItemDown();
    }

    private void OnEnable()
    {
        _input.Enable();
        _option = MainMenuOptions.Play;

        playImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
        playImage.color = Color.black;
        playBgAnim.SetTrigger("enable");
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void ItemUp()
    {
        switch (_option)
        {
            case MainMenuOptions.Play:
                break;
            case MainMenuOptions.Options:
                _option = MainMenuOptions.Play;
                optionsImage.transform.localScale = new Vector3(1,1, 1);
                optionsImage.color = Color.white;
                optionsBgAnim.SetTrigger("disable");
                playImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
                playImage.color = Color.black;
                playBgAnim.SetTrigger("enable");
                break;
            case MainMenuOptions.Exit:
                _option = MainMenuOptions.Options;
                exitImage.transform.localScale = new Vector3(1,1, 1);
                exitImage.color = Color.white;
                exitBgAnim.SetTrigger("disable");
                optionsImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
                optionsImage.color = Color.black;
                optionsBgAnim.SetTrigger("enable");
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case MainMenuOptions.Play:
                _option = MainMenuOptions.Options;
                playImage.transform.localScale = new Vector3(1,1,1);
                playImage.color = Color.white;
                playBgAnim.SetTrigger("disable");
                optionsImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
                optionsImage.color = Color.black;
                optionsBgAnim.SetTrigger("enable");
                break;
            case MainMenuOptions.Options:
                _option = MainMenuOptions.Exit;
                optionsImage.transform.localScale = new Vector3(1,1, 1);
                optionsImage.color = Color.white;
                optionsBgAnim.SetTrigger("disable");
                exitImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
                exitImage.color = Color.black;
                exitBgAnim.SetTrigger("enable");
                break;
            case MainMenuOptions.Exit:
                break;
        }
    }

    private void AcceptItem()
    {
        switch (_option)
        {
            case MainMenuOptions.Play:
                UIManager.Instance.Play();
                break;
            case MainMenuOptions.Options:
                UIManager.Instance.ShowConfigMenu();
                break;
            case MainMenuOptions.Exit:
                UIManager.Instance.ExitGame();
                break;
        }
    }
}
