using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuLogic : MonoBehaviour
{
    private enum PauseMenuOption
    {
        Resume,
        Options,
        Exit
    }

    private InputActions _input;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.MenuBack.started += ctx => UIManager.Instance.HidePauseMenu();
        _input.PlayerControls.MenuAccept.started += ctx => AcceptOption();
        _input.PlayerControls.MenuNavigationUp.started += ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started += ctx => ItemDown();
    }

    private PauseMenuOption _option;

    private void OnEnable()
    {
        _input.Enable();
        _option = PauseMenuOption.Resume;
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void ItemUp()
    {
        switch (_option)
        {
            case PauseMenuOption.Resume:
                break;
            case PauseMenuOption.Options:
                _option = PauseMenuOption.Resume;
                break;
            case PauseMenuOption.Exit:
                _option = PauseMenuOption.Options;
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case PauseMenuOption.Resume:
                _option = PauseMenuOption.Options;
                break;
            case PauseMenuOption.Options:
                _option = PauseMenuOption.Exit;
                break;
            case PauseMenuOption.Exit:
                break;
        }
    }

    private void AcceptOption()
    {
        switch (_option)
        {
            case PauseMenuOption.Resume:
                UIManager.Instance.HidePauseMenu();
                break;
            case PauseMenuOption.Options:
                UIManager.Instance.ShowConfigMenu();
                break;
            case PauseMenuOption.Exit:
                UIManager.Instance.MainMenu();
                break;
        }
    }
}
