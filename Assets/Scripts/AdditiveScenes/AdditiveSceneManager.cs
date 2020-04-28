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
        print("Scene " + _currentSceneIdx + " Generated");
        if (firstScene)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    public IEnumerator LoadNextScene()
    {
        var loading = SceneManager.LoadSceneAsync(_currentSceneIdx + 1, LoadSceneMode.Additive);
        yield return loading;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIdx + 1));
    }

    public void DestroyPreviousScene()
    {
        if (_currentSceneIdx >= 2)
        {
            MoveObjectsToActiveScene();
            SceneManager.UnloadSceneAsync(_currentSceneIdx - 1);
        }
    }

    private void MoveObjectsToActiveScene()
    {
        SceneManager.MoveGameObjectToScene(GameObject.Find("Maintain"), SceneManager.GetActiveScene());
    }
}
