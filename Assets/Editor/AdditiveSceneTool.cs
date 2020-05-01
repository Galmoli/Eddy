using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class AdditiveSceneTool : EditorWindow
{
    private bool _toolActive;
    private GameObject _dummy;
    
    [MenuItem("Window/AdditiveSceneTool")]
    static void OpenWindow()
    {
        AdditiveSceneTool window = (AdditiveSceneTool) GetWindow(typeof(AdditiveSceneTool));
        window.minSize = new Vector2(300, 300);
        window.Show();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlay;
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying) return;
        GUILayout.Label("TOOL ACTIVE = " + _toolActive);
        if (GUILayout.Button("Auto-config", GetAutoConfigStyle()))
        {
            _toolActive = true;
            SpawnDummy();
        }
        
        if (GUILayout.Button("Reset", GetResetStyle()))
        {
            _toolActive = false;
            DestroyDummy();
        }
    }
    
    private void OnPlay(PlayModeStateChange _state)
    {
        if (_state == PlayModeStateChange.EnteredPlayMode && _toolActive)
        {
            SpawnMaintain();
            SpawnContiguousScenes();
            DestroyDummy();
        }

        if (_state == PlayModeStateChange.ExitingPlayMode && _toolActive)
        {
            SpawnDummy();
        }
    }

    private void SpawnMaintain()
    {
        if (!GameObject.Find("MaintainBetweenScenes") && !GameObject.Find("ContiguousScenes"))
        {
            var maintain = Instantiate(Resources.Load<GameObject>("MaintainBetweenScenes"), _dummy.transform.position, Quaternion.identity);
            maintain.name = "MaintainBetweenScenes";
        }
    }

    private void SpawnDummy()
    {
        _dummy = Instantiate(Resources.Load<GameObject>("PlayerDummy"));
        _dummy.name = "PlayerDummy";
        EditorGUIUtility.PingObject(_dummy);
    }

    private void DestroyDummy()
    {
        var d = GameObject.Find("PlayerDummy");
        if(d) DestroyImmediate(d);
    }

    private void SpawnContiguousScenes()
    {
        AdditiveSceneManager sm = FindObjectOfType<AdditiveSceneManager>();
        int currentSceneIdx = sm.gameObject.scene.buildIndex;
        if (currentSceneIdx <= sm._bootSceneIdx + 2) //Only load next Scene
        {
            SceneManager.LoadSceneAsync(currentSceneIdx + 1, LoadSceneMode.Additive);
        }
        else if (currentSceneIdx >= SceneManager.sceneCountInBuildSettings - 1) //Only load previous Scene
        {
            SceneManager.LoadSceneAsync(currentSceneIdx - 1, LoadSceneMode.Additive);
        }
        else //Load both scenes
        {
            SceneManager.LoadSceneAsync(currentSceneIdx + 1, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(currentSceneIdx - 1, LoadSceneMode.Additive);
        }
    }

    private GUIStyle GetAutoConfigStyle()
    {
        var autoConfigStyle = new GUIStyle(GUI.skin.button);
        autoConfigStyle.fixedWidth = 120;
        autoConfigStyle.fixedHeight = 30;
        
        GUI.backgroundColor = Color.green;
        return autoConfigStyle;
    }
    
    private GUIStyle GetResetStyle()
    {
        var resetStyle = new GUIStyle(GUI.skin.button);
        resetStyle.fixedWidth = 120;
        resetStyle.fixedHeight = 30;
        
        GUI.backgroundColor = Color.red;
        return resetStyle;
    }
}
