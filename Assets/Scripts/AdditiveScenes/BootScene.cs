using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScene : MonoBehaviour
{
    private int _currentSceneIdx;
    private void Start()
    {
        _currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
        
        LoadFirstScene();
        LoadSecondScene();
    }

    private void LoadFirstScene()
    {
        StartCoroutine(Co_LoadFirstScene());
    }

    private IEnumerator Co_LoadFirstScene()
    {
        var loading = SceneManager.LoadSceneAsync(_currentSceneIdx + 1, LoadSceneMode.Additive);
        yield return loading;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx + 1));
        SceneManager.MoveGameObjectToScene(GameObject.Find("MaintainBetweenScenes"), SceneManager.GetActiveScene());
    }
    
    private void LoadSecondScene()
    {
        SceneManager.LoadSceneAsync(_currentSceneIdx + 2, LoadSceneMode.Additive);
    }
}
