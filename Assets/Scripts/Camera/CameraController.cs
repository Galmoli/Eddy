using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Basics")]
    public CameraRail rail;
    public Transform target;

    [Header("Movement")]
    public float maxMovementSpeed = 15f;
    public float maxDistance = 5f;
    public float movementSpeedIncrement = 8f;

    private float currentMovementSpeed = 0f;

    [Header("Rotation")]
    public bool lookAtPlayer = false;
    public float cameraRotationSpeed = 2f;

    /*public float maxRotationSpeed = 15f;
    public float maxAngle = 5f;
    public float rotationSpeedIncrement = 5f;
    private float currentRotationSpeed = 0f;*/

    void Update()
    {
        if (rail != null)
        {
            Vector3 pos = rail.ProjectPosition(target.position);

            float desiredMovementSpeed = Vector3.Distance(transform.position, pos) * maxMovementSpeed / maxDistance;

            if (currentMovementSpeed < desiredMovementSpeed) currentMovementSpeed += movementSpeedIncrement * Time.deltaTime;
            else if (currentMovementSpeed > desiredMovementSpeed) currentMovementSpeed = desiredMovementSpeed;

            if (currentMovementSpeed > maxMovementSpeed) currentMovementSpeed = maxMovementSpeed;
            else if (currentMovementSpeed < 0) currentMovementSpeed = 0f;

            transform.position = Vector3.MoveTowards(transform.position, pos, currentMovementSpeed * Time.deltaTime);

            if (lookAtPlayer)
            {
                transform.LookAt(target.position);
            }
            else
            {
                Vector3 rot = rail.ProjectRotation(target.position, transform.position);

                /*float desiredRotationSpeed = Mathf.Max(Mathf.Abs(rot.x - transform.eulerAngles.x), Mathf.Abs(rot.y - transform.eulerAngles.y), Mathf.Abs(rot.z - transform.eulerAngles.z)) * maxRotationSpeed / maxAngle;

                if (currentRotationSpeed < desiredRotationSpeed) currentRotationSpeed += movementSpeedIncrement * Time.deltaTime;
                else if (currentRotationSpeed > desiredRotationSpeed) currentRotationSpeed = desiredRotationSpeed;

                Debug.Log(desiredRotationSpeed);

                if (currentRotationSpeed > maxRotationSpeed) currentRotationSpeed = maxRotationSpeed;
                else if (currentRotationSpeed < 0) currentRotationSpeed = 0f;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rot), currentRotationSpeed * Time.deltaTime);*/
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), cameraRotationSpeed * Time.deltaTime);
            }
        }
    }

    public void SetPositionImmediately()
    {
        transform.position = rail.ProjectPosition(target.position, true);
        transform.rotation = Quaternion.Euler(rail.ProjectRotation(target.position, transform.position));
    }
}