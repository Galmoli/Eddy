using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class CameraNode : MonoBehaviour
{
    [HideInInspector] public CameraController cameraController;
    [HideInInspector] public List<CameraNode> connectedNodes = new List<CameraNode>();
    
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }
}
