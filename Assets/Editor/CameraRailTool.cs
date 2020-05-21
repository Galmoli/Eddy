using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CameraRailTool : EditorWindow
{
    [MenuItem("Window/CameraRailTool")]
    static void OpenWindow()
    {
        CameraRailTool window = (CameraRailTool)GetWindow(typeof(CameraRailTool));
        window.minSize = new Vector2(300, 300);
        window.Show();
    }

    public GameObject camera;
    public GameObject rail;
    public int node;
    private List<Transform> nodes = new List<Transform>();

    private void OnGUI()
    {
        if (EditorApplication.isPlaying) return;

        DrawLayouts();

        EditorGUILayout.BeginHorizontal();
        rail = (GameObject)EditorGUILayout.ObjectField(rail, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        camera = (GameObject)EditorGUILayout.ObjectField(camera, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        node = EditorGUILayout.IntField("Node to Select:", node);

        if (GUILayout.Button("Init Camera Rail", GetAutoConfigStyle()))
        {
            InitCameraRail();
        }

        if (GUILayout.Button("Rename Rail", GetAutoConfigStyle()))
        {
            RenameRail();
        }

        if (GUILayout.Button("Add Node", GetAutoConfigStyle()))
        {
            AddNode();
        }

        if (GUILayout.Button("Parent Camera", GetAutoConfigStyle()))
        {
            ParentCamera();
        }

        if (GUILayout.Button("Free Camera", GetAutoConfigStyle()))
        {
            FreeCamera();
        }

    }

    public void FreeCamera()
    {
        camera.transform.parent = null;
    }

    public void ParentCamera()
    {
        camera.transform.position = rail.transform.GetChild(node).GetChild(0).transform.position;
        camera.transform.rotation = rail.transform.GetChild(node).GetChild(0).transform.rotation;
        camera.transform.parent = rail.transform.GetChild(node).GetChild(0).transform;
    }

    public void AddNode()
    {
        Transform selectedNode = rail.transform.GetChild(node);
        Transform newNode = Instantiate(selectedNode.gameObject, rail.transform).transform;
        newNode.SetSiblingIndex(node + 1);
        nodes.Insert(node + 1, newNode);
        RenameRail();

    }

    public void RenameRail()
    {
        for (int i= 0; i < nodes.Count; i++)
        {
            nodes[i].name = "Node (" + i + ")";
        }
    }

    public void InitCameraRail()
    {
        nodes.Clear();

        for (int i = 0; i < rail.transform.childCount; i++)
        {
            nodes.Add(rail.transform.GetChild(i));
        }
    }

    private GUIStyle GetAutoConfigStyle()
    {
        var autoConfigStyle = new GUIStyle(GUI.skin.button);
        autoConfigStyle.fixedWidth = Screen.width;
        autoConfigStyle.fixedHeight = 35;

        GUI.backgroundColor = Color.green;
        return autoConfigStyle;
    }

    private void DrawLayouts()
    {
        var textStyle = new GUIStyle(GUI.skin.label);
        textStyle.fontSize = 18;
        textStyle.fontStyle = FontStyle.Bold;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
