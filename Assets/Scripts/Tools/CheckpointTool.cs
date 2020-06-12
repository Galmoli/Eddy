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
    
    private CharacterController _cc;
    [SerializeField] private CPPos[] positions;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1)) StartCoroutine(GoTo(0)); 
        if(Input.GetKeyDown(KeyCode.Keypad2)) StartCoroutine(GoTo(1)); 
        if(Input.GetKeyDown(KeyCode.Keypad3)) StartCoroutine(GoTo(2)); 
        if(Input.GetKeyDown(KeyCode.Keypad4)) StartCoroutine(GoTo(3)); 
        if(Input.GetKeyDown(KeyCode.Keypad5)) StartCoroutine(GoTo(4)); 
    }

    private IEnumerator GoTo(int i)
    {
        _cc.enabled = false;
        transform.position = positions[i].position;
        GameManager.Instance.GoToScene(positions[i].scene);
        FindObjectOfType<CameraController>().SetPositionImmediately();
        
        yield return new WaitForSeconds(0.5f);
        _cc.enabled = true;
    }
}
