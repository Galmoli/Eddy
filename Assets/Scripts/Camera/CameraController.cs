using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraRail rail;
    public Transform target;
    public float speed = 1f;

    private Vector3 lastPos;

    void Update()
    {
        lastPos = Vector3.Lerp(lastPos, rail.ProjectPosition(target.position), speed * Time.deltaTime);
        transform.position = lastPos;

        transform.LookAt(target.position);
    }
}
