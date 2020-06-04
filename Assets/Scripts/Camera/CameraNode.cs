using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class CameraNode : MonoBehaviour
{
    [HideInInspector] public CameraController cameraController;
    [HideInInspector] public List<CameraNode> connectedNodes;
    
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();

        foreach (CameraNode connection in connectedNodes)
        {
            if (!connection.connectedNodes.Contains(this))
                connection.connectedNodes.Add(this);
        }
    }
}
