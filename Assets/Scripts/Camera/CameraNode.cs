using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNode : MonoBehaviour
{
    public CameraController cameraController;
    public CameraNode[] connectedNodes;
    
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        
    }
}
