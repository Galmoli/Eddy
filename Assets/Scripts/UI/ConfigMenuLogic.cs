using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMenuLogic : MonoBehaviour
{
    private enum ConfigMenuOptions
    {
        Volume,
        Back
    }
    
    private InputActions _input;
    private ConfigMenuOptions _option;

    [SerializeField] private GameObject volume;
    [SerializeField] private GameObject backImage;
    
    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.MenuBack.started += ctx => Back();
        _input.PlayerControls.MenuNavigationUp.started += ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started += ctx => ItemDown();
        _input.PlayerControls.MenuAccept.started += ctx => AcceptItem();
    }

    private void OnEnable()
    {
        _input.Enable();
        _option = ConfigMenuOptions.Volume;
        volume.transform.localScale = new Vector3(1.1f,1.1f, 1);
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void ItemUp()
    {
        switch (_option)
        {
            case ConfigMenuOptions.Volume:
                break;
            case ConfigMenuOptions.Back:
                _option = ConfigMenuOptions.Volume;
                backImage.transform.localScale = new Vector3(1,1,1);
                volume.transform.localScale = new Vector3(1.1f,1.1f, 1);
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case ConfigMenuOptions.Volume:
                volume.transform.localScale = new Vector3(1, 1, 1);
                backImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
                _option = ConfigMenuOptions.Back;
                break;
            case ConfigMenuOptions.Back:
                break;
        }
    }

    private void AcceptItem()
    {
        switch (_option)
        {
            case ConfigMenuOptions.Volume:
                break;
            case ConfigMenuOptions.Back:
                Back();
                break;
        }
    }

    private void Back()
    {
        UIManager.Instance.HideConfigMenu();
    }
}
