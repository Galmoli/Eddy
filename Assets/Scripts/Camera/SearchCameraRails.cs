using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SearchCameraRails : MonoBehaviour
{
    private void Start()
    {
        if(gameObject.scene.name != "BootScene") SearchCameraRail();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SearchCameraRailEvent;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SearchCameraRailEvent;
    }

    private void SearchCameraRailEvent(Scene current, Scene next)
    {
        SearchCameraRail();
    }

    private void SearchCameraRail()
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
