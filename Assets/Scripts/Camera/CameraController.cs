using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraRail rail;
    public Transform target;
    public float movementSpeed = 1f;
    public float rotationSpeed = 1f;

    [Header("Look At Player / Nodes Rotation")]
    public bool lookAtPlayer;

    void Update()
    {      
        if(rail != null)
        {
            transform.position = Vector3.Lerp(transform.position, rail.ProjectPosition(target.position), movementSpeed * Time.deltaTime); ;

            if (lookAtPlayer)
            {
                transform.LookAt(target.position);
            }
            else
            {
                Vector3 rot = rail.ProjectRotation(target.position, transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
