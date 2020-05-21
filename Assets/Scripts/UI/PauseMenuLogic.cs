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

    [SerializeField] private GameObject resumeImage;
    [SerializeField] private GameObject optionsImage;
    [SerializeField] private GameObject exitImage;

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
        resumeImage.transform.localScale = new Vector3(1.1f,1.1f,1);
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
                optionsImage.transform.localScale = new Vector3(1, 1, 1);
                resumeImage.transform.localScale = new Vector3(1.1f,1.1f,1);
                _option = PauseMenuOption.Resume;
                break;
            case PauseMenuOption.Exit:
                exitImage.transform.localScale = new Vector3(1,1,1);
                optionsImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                _option = PauseMenuOption.Options;
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case PauseMenuOption.Resume:
                resumeImage.transform.localScale = new Vector3(1,1,1);
                optionsImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                _option = PauseMenuOption.Options;
                break;
            case PauseMenuOption.Options:
                optionsImage.transform.localScale = new Vector3(1,1,1);
                exitImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
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
