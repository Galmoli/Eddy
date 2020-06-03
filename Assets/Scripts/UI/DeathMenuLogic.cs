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
    public Animator tryAgainBgAnim;
    public Animator mainMenuBgAnim;

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
        _option = DeathMenuOptions.TryAgain;
        tryAgainImage.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        tryAgainImage.color = Color.black;
        tryAgainBgAnim.SetTrigger("enable");
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
        GameManager.Instance.Respawn();
        gameObject.SetActive(false);
    }
    
    public void MainMenu() //Currently used by Button OnClick Event
    {
        UIManager.Instance.MainMenu();
    }
}
