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
        var loading = SceneManager.LoadSceneAsync(SceneToLoad(), LoadSceneMode.Additive);
        yield return loading;
        if (!firstScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx + 1));
            MoveObjectsToActiveScene();
        }
    }

    public void DestroyPreviousScene()
    {
        if (_currentSceneIdx >= 2)
        {
            SceneManager.UnloadSceneAsync(_currentSceneIdx - 1);
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
