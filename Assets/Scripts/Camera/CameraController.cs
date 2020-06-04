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

    private bool closeEnough;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) SetPlayerPosition();
        
        if(rail != null)
        {
            Vector3 pos = rail.ProjectPosition(target.position);

            closeEnough = Vector3.Distance(target.position, pos) < 1;

            if (closeEnough)
            {
                transform.position = Vector3.Lerp(transform.position, pos, movementSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, pos, movementSpeed / 5 * Time.deltaTime);
            }

            if (lookAtPlayer)
            {
                transform.LookAt(target.position);
            }
            else
            {
                Vector3 rot = rail.ProjectRotation(target.position, transform.position);

                if (closeEnough)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), rotationSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), rotationSpeed / 5 * Time.deltaTime);
                }
            }
        }
    }

    public void SetPlayerPosition()
    {
        transform.position = rail.ProjectPosition(target.position, true);
        transform.rotation = Quaternion.Euler(rail.ProjectRotation(target.position, transform.position));
    }
}
