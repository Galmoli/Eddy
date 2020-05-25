using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook : MonoBehaviour
{
    public Transform LookAtObj;
    public Vector3 iniRot;

    void Update()
    {
        transform.LookAt(LookAtObj,Vector3.up);
        transform.Rotate(iniRot);
    }
}
