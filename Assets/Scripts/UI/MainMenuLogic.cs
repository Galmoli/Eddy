using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        _input = new InputActions();
        _input.PlayerControls.MenuAccept.started += ctx => AcceptItem();
        _input.PlayerControls.MenuNavigationUp.started += ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started += ctx => ItemDown();
    }

    private void OnEnable()
    {
        _input.Enable();
        _option = MainMenuOptions.Play;
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
                break;
            case MainMenuOptions.Exit:
                _option = MainMenuOptions.Options;
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case MainMenuOptions.Play:
                _option = MainMenuOptions.Options;
                break;
            case MainMenuOptions.Options:
                _option = MainMenuOptions.Exit;
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
