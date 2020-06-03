using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    [SerializeField] private bool firstScene;
    [SerializeField] private bool secondScene;
    private int _currentSceneIdx;
    [HideInInspector] public int _bootSceneIdx = 2;

    private void Start()
    {
        _currentSceneIdx = gameObject.scene.buildIndex;
    }
    
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }
    
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    public IEnumerator LoadNextScene()
    {
        if (_currentSceneIdx >= SceneManager.sceneCountInBuildSettings - 1) //Is Last scene
        {
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
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx - 1));
            MoveObjectsToActiveScene();
            DestroyNextScene();
            yield break;
        }

        if (!secondScene)
        {
            var loading = SceneManager.LoadSceneAsync(_currentSceneIdx - 2, LoadSceneMode.Additive);
            yield return loading;
        }
        
        if (!firstScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx - 1));
            MoveObjectsToActiveScene();
            DestroyNextScene();
        }
    }

    public IEnumerator LoadScene(int idx)
    {
        if(idx == gameObject.scene.buildIndex) yield break;
        
        if (!IsSceneInstanced(idx))
        {
            var loading = SceneManager.LoadSceneAsync(idx, LoadSceneMode.Additive);
            yield return loading;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(idx));
        
        if (idx - 1 > _bootSceneIdx && !IsSceneInstanced(idx - 1))
        {
            var loadPrevious = SceneManager.LoadSceneAsync(idx - 1, LoadSceneMode.Additive);
            yield return loadPrevious;
        }

        if (idx + 1 < SceneManager.sceneCountInBuildSettings && !IsSceneInstanced(idx + 1))
        {
            var loadNext = SceneManager.LoadSceneAsync(idx + 1, LoadSceneMode.Additive);
            yield return loadNext;
        }
        
        MoveObjectsToActiveScene();

        if (_currentSceneIdx - 1 != idx - 1 &&
            _currentSceneIdx - 1 != idx &&
            _currentSceneIdx - 1 != idx + 1 &&
            IsSceneInstanced(_currentSceneIdx - 1))
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx - 1);
        }
        
        if (_currentSceneIdx + 1 != idx - 1 &&
            _currentSceneIdx + 1 != idx &&
            _currentSceneIdx + 1 != idx + 1 &&
            IsSceneInstanced(_currentSceneIdx + 1))
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx + 1);
        }
        
        if (_currentSceneIdx != idx - 1 &&
            _currentSceneIdx != idx &&
            _currentSceneIdx != idx + 1 &&
            IsSceneInstanced(_currentSceneIdx + 1))
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx);
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

    private void ActiveSceneChanged(Scene p, Scene n)
    {
        if (n.buildIndex == gameObject.scene.buildIndex)
        {
            GameManager.Instance.asm = this;
        }
    }

    private bool IsSceneInstanced(int idx)
    {
        return SceneManager.GetSceneByBuildIndex(idx).isLoaded;
    }
}
