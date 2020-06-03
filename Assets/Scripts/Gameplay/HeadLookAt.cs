using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    public Transform lookAtObj;
    public Vector3 rotOffset;

    void Start()
    {
        if (lookAtObj == null) lookAtObj = GameObject.FindObjectOfType<PlayerController>().transform;
    }
    void Update()
    {
        transform.LookAt(lookAtObj);
        transform.Rotate(rotOffset);
    }
}
