using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    [SerializeField] private bool firstScene;
    private int _currentSceneIdx;
    private int _bootSceneIdx = 0;

    private void Start()
    {
        _currentSceneIdx = gameObject.scene.buildIndex;
    }

    public IEnumerator LoadNextScene()
    {
        if (_currentSceneIdx >= SceneManager.sceneCountInBuildSettings - 1) //Is Last scene
        {
            print("LastScene: "+ _currentSceneIdx);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx));
            MoveObjectsToActiveScene();
            DestroyPreviousScene();
            yield break;
        } 
        
        var loading = SceneManager.LoadSceneAsync(_currentSceneIdx + 1, LoadSceneMode.Additive);
        yield return loading;
        
        if (!firstScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx));
            MoveObjectsToActiveScene();
            DestroyPreviousScene();
        }
    }

    public IEnumerator LoadPreviousScene()
    {
        if(firstScene) yield break;
        
        if (_currentSceneIdx <= _bootSceneIdx + 2) //Is First Scene
        {
            print("First Scene " + _currentSceneIdx);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx - 1));
            MoveObjectsToActiveScene();
            DestroyNextScene();
            yield break;
        }
        
        var loading = SceneManager.LoadSceneAsync(_currentSceneIdx - 2, LoadSceneMode.Additive);
        yield return loading;
        if (!firstScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx - 1));
            MoveObjectsToActiveScene();
            DestroyNextScene();
        }
    }

    private void DestroyPreviousScene()
    {
        if (_currentSceneIdx > _bootSceneIdx + 2)
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx - 2);
        }
    }

    private void DestroyNextScene()
    {
        if (_currentSceneIdx + 1 <= SceneManager.sceneCountInBuildSettings -1)
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx + 1);
        }
    }

    private void MoveObjectsToActiveScene()
    {
        SceneManager.MoveGameObjectToScene(GameObject.Find("MaintainBetweenScenes"), SceneManager.GetActiveScene());
    }
}
