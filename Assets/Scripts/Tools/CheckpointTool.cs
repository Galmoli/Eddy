using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTool : MonoBehaviour
{
    [Serializable]
    struct CPPos
    {
        public int scene;
        public Vector3 position;
    }

    [SerializeField] private CharacterController _cc;
    [SerializeField] private CPPos[] positions;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1)) GoTo(0); 
        if(Input.GetKeyDown(KeyCode.Keypad2)) GoTo(1); 
        if(Input.GetKeyDown(KeyCode.Keypad3)) GoTo(2); 
        if(Input.GetKeyDown(KeyCode.Keypad4)) GoTo(3); 
        if(Input.GetKeyDown(KeyCode.Keypad5)) GoTo(4); 
    }

    private void GoTo(int i)
    {
        _cc.enabled = false;
        transform.position = positions[i].position;
        _cc.enabled = true;
        GameManager.Instance.GoToScene(positions[i].scene);
    }
}
