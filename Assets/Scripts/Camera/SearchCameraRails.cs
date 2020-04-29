using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SearchCameraRails : MonoBehaviour
{
    
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SearchCameraRail;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SearchCameraRail;
    }

    private void SearchCameraRail(Scene current, Scene next)
    {
        var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        CameraController _cameraController = GetComponent<CameraController>();
        
        foreach (var o in gameObjects)
        {
            if (o.name == "CameraRails")
            {
                _cameraController.rail = o.GetComponent<CameraRail>();
            }
        }
    }
}
