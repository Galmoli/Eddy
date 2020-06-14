using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfigMenuLogic : MonoBehaviour
{
    private enum ConfigMenuOptions
    {
        VolumeMusic,
        VolumeSFX,
        Back
    }
    
    private InputActions _input;
    private ConfigMenuOptions _option;

    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    
    [SerializeField] private TextMeshProUGUI volumeMusic;
    [SerializeField] private TextMeshProUGUI volumeSFX;
    [SerializeField] private TextMeshProUGUI backImage;
    public Animator volumeMusicBgAnim;
    public Animator volumeSFXBgAnim;
    public Animator backBgAnim;

    private Vector2 sliderVector;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.MenuBack.started += ctx => Back();
        _input.PlayerControls.MenuNavigationUp.started += ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started += ctx => ItemDown();
        _input.PlayerControls.MenuAccept.started += ctx => AcceptItem();
        _input.PlayerControls.MenuSlider.performed += callbackContext => sliderVector = callbackContext.ReadValue<Vector2>();
    }

    private void Start()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("musicVolume", 1);
        sliderSFX.value = PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    private void OnDestroy()
    {
        _input.PlayerControls.MenuBack.started -= ctx => Back();
        _input.PlayerControls.MenuNavigationUp.started -= ctx => ItemUp();
        _input.PlayerControls.MenuNavigationDown.started -= ctx => ItemDown();
        _input.PlayerControls.MenuAccept.started -= ctx => AcceptItem();
        _input.PlayerControls.MenuSlider.performed -= callbackContext => sliderVector = callbackContext.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _option = ConfigMenuOptions.VolumeMusic;

        volumeMusic.transform.localScale = new Vector3(1.1f,1.1f, 1);
        volumeMusic.color = Color.black;
        volumeMusicBgAnim.SetTrigger("enable");
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        switch (_option)
        {
            case ConfigMenuOptions.VolumeMusic:
                if (Mathf.Abs(sliderVector.x) > 0.25f)
                {
                    sliderMusic.value += sliderVector.x * 0.02f;
                    AudioManager.Instance.SetMusicVolume(sliderMusic.value);
                    PlayerPrefs.SetFloat("musicVolume", sliderMusic.value);
                }
                break;
            case ConfigMenuOptions.VolumeSFX:
                if (Mathf.Abs(sliderVector.x) > 0.25f)
                {
                    sliderSFX.value += sliderVector.x * 0.02f;
                    AudioManager.Instance.SetSFXVolume(sliderSFX.value);
                    PlayerPrefs.SetFloat("sfxVolume", sliderSFX.value);
                }
                break;
        }
    }

    private void ItemUp()
    {
        switch (_option)
        {
            case ConfigMenuOptions.VolumeMusic:
                break;
            case ConfigMenuOptions.VolumeSFX:
                _option = ConfigMenuOptions.VolumeMusic;
                volumeSFX.transform.localScale = new Vector3(1,1, 1);
                volumeSFX.color = Color.white;
                volumeSFXBgAnim.SetTrigger("disable");
                volumeMusic.transform.localScale = new Vector3(1.1f,1.1f, 1);
                volumeMusic.color = Color.black;
                volumeMusicBgAnim.SetTrigger("enable");
                break;
            case ConfigMenuOptions.Back:
                _option = ConfigMenuOptions.VolumeSFX;
                backImage.transform.localScale = new Vector3(1,1,1);
                backImage.color = Color.white;
                backBgAnim.SetTrigger("disable");
                volumeSFX.transform.localScale = new Vector3(1.1f,1.1f, 1);
                volumeSFX.color = Color.black;
                volumeSFXBgAnim.SetTrigger("enable");
                break;
        }
    }

    private void ItemDown()
    {
        switch (_option)
        {
            case ConfigMenuOptions.VolumeMusic:
                _option = ConfigMenuOptions.VolumeSFX;
                volumeMusic.transform.localScale = new Vector3(1, 1, 1);
                volumeMusic.color = Color.white;
                volumeMusicBgAnim.SetTrigger("disable");
                volumeSFX.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                volumeSFX.color = Color.black;
                volumeSFXBgAnim.SetTrigger("enable");
                break;
            case ConfigMenuOptions.VolumeSFX:
                _option = ConfigMenuOptions.Back;
                volumeSFX.transform.localScale = new Vector3(1, 1, 1);
                volumeSFX.color = Color.white;
                volumeSFXBgAnim.SetTrigger("disable");
                backImage.transform.localScale = new Vector3(1.1f,1.1f, 1);
                backImage.color = Color.black;
                backBgAnim.SetTrigger("enable");
                break;
            case ConfigMenuOptions.Back:
                break;
        }
    }

    private void AcceptItem()
    {
        switch (_option)
        {
            case ConfigMenuOptions.Back:
                backImage.color = Color.white;
                Back();
                break;
        }
    }

    private void Back()
    {
        volumeSFX.color = Color.white;
        backImage.color = Color.white;
        UIManager.Instance.HideConfigMenu();
    }
}
