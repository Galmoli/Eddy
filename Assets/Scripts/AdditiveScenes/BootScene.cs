using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScene : MonoBehaviour
{
    [SerializeField] private int _initialSceneIdx;
    private int _currentSceneIdx;
    private void Start()
    {
        _currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
        
        LoadFirstScene();
        LoadScene(_initialSceneIdx -1);
        LoadScene(_initialSceneIdx +1);
    }

    private void LoadFirstScene()
    {
        StartCoroutine(Co_LoadFirstScene());
    }

    private IEnumerator Co_LoadFirstScene()
    {
        var loading = SceneManager.LoadSceneAsync(_initialSceneIdx, LoadSceneMode.Additive);
        yield return loading;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_initialSceneIdx));
        SceneManager.MoveGameObjectToScene(GameObject.Find("MaintainBetweenScenes"), SceneManager.GetActiveScene());
    }
    
    private void LoadScene(int idx)
    {
        SceneManager.LoadSceneAsync(idx, LoadSceneMode.Additive);
    }
}
