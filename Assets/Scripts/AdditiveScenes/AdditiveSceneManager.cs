using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    [SerializeField] private bool firstScene;
    private int _currentSceneIdx;
    void Start()
    {
        _currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
        if (firstScene)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    public IEnumerator LoadNextScene()
    {
        if (_currentSceneIdx >= SceneManager.sceneCountInBuildSettings - 2)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx + 1));
            MoveObjectsToActiveScene();
            DestroyPreviousScene();
            yield break;
        } 
        
        var loading = SceneManager.LoadSceneAsync(SceneToLoad(), LoadSceneMode.Additive);
        yield return loading;
        
        if (!firstScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx + 1));
            MoveObjectsToActiveScene();
            DestroyPreviousScene();
        }
    }

    public IEnumerator LoadPreviousScene()
    {
        if (firstScene) yield break;
        
        var loading = SceneManager.LoadSceneAsync(_currentSceneIdx - 1, LoadSceneMode.Additive);
        yield return loading;
        if (!firstScene)
        {
            _currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx - 1));
            MoveObjectsToActiveScene();
            DestroyNextScene();
        }
    }

    private void DestroyPreviousScene()
    {
        if (_currentSceneIdx >= 2)
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx - 1);
        }
    }

    private void DestroyNextScene()
    {
        if (_currentSceneIdx + 2 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx + 2);
        }
    }

    private void MoveObjectsToActiveScene()
    {
        SceneManager.MoveGameObjectToScene(GameObject.Find("Maintain"), SceneManager.GetActiveScene());
    }

    private int SceneToLoad()
    {
        if (firstScene) return _currentSceneIdx + 1;
        return _currentSceneIdx + 2;
    }
}
