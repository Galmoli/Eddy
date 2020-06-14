using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuLogic : MonoBehaviour
{
    private enum PauseMenuOption
    {
        Resume,
        Options,
        Exit
    }

    private InputActions _input;

    [SerializeField] private TextMeshProUGUI resumeImage;
    [SerializeField] private TextMeshProUGUI optionsImage;
    [SerializeField] private TextMeshProUGUI exitImage;
    public Animator resumeBgAnim;
    public Animator optionsBgAnim;
    public Animator exitBgAnim;

    [Header("Sounds")]
    [FMODUnity.EventRef] public string buttonNavigationSoundPath;
    [FMODUnity.EventRef] public string buttonClickSoundPath;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.MenuBack.started += ctx => HidePauseMenu();
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
        resumeImage.color = Color.black;
        resumeBgAnim.SetTrigger("enable");
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
                ButtonNavigationSound();
                optionsImage.transform.localScale = new Vector3(1, 1, 1);
                optionsImage.color = Color.white;
                optionsBgAnim.SetTrigger("disable");
                resumeImage.transform.localScale = new Vector3(1.1f,1.1f,1);
                resumeImage.color = Color.black;
                resumeBgAnim.SetTrigger("enable");
                _option = PauseMenuOption.Resume;
                break;
            case PauseMenuOption.Exit:
                ButtonNavigationSound();
                exitImage.transform.localScale = new Vector3(1,1,1);
                exitImage.color = Color.white;
                exitBgAnim.SetTrigger("disable");
                optionsImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                optionsImage.color = Color.black;
                optionsBgAnim.SetTrigger("enable");
                _option = PauseMenuOption.Options;
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case PauseMenuOption.Resume:
                ButtonNavigationSound();
                resumeImage.transform.localScale = new Vector3(1,1,1);
                resumeImage.color = Color.white;
                resumeBgAnim.SetTrigger("disable");
                optionsImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                optionsImage.color = Color.black;
                optionsBgAnim.SetTrigger("enable");
                _option = PauseMenuOption.Options;
                break;
            case PauseMenuOption.Options:
                ButtonNavigationSound();
                optionsImage.transform.localScale = new Vector3(1,1,1);
                optionsImage.color = Color.white;
                optionsBgAnim.SetTrigger("disable");
                exitImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                exitImage.color = Color.black;
                exitBgAnim.SetTrigger("enable");
                _option = PauseMenuOption.Exit;
                break;
            case PauseMenuOption.Exit:
                break;
        }
    }

    private void AcceptOption()
    {
        ButtonClickSound();
        switch (_option)
        {
            case PauseMenuOption.Resume:
                UIManager.Instance.HidePauseMenu();
                break;
            case PauseMenuOption.Options:
                optionsImage.color = Color.white;
                UIManager.Instance.ShowConfigMenu();
                break;
            case PauseMenuOption.Exit:
                UIManager.Instance.MainMenu();
                break;
        }
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

    private void HidePauseMenu()
    {
        optionsImage.color = Color.white;
        exitImage.color = Color.white;
        UIManager.Instance.HidePauseMenu();
    }
}
