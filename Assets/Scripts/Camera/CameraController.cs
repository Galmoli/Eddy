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
    public float movementSpeed = 2f;
    //public float slowingRadius;

    [Header("Rotation")]
    public float rotationSpeed;
    public bool lookAtPlayer;

    float distance = 5f;
    float maxSpeed = 15f;
    float acceleration = 2f;
    float currentSpeed = 0f;

    void Update()
    {
        if (rail != null)
        {
            Vector3 pos = rail.ProjectPosition(target.position);

            float desiredSpeed = Vector3.Distance(transform.position, pos) * maxSpeed / distance;





            if (currentSpeed < desiredSpeed) currentSpeed += acceleration * Time.deltaTime;
            else if (currentSpeed > desiredSpeed) currentSpeed = desiredSpeed;

            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
            else if (currentSpeed < 0) currentSpeed = 0f;

            //currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, acceleration * Time.deltaTime);

            //transform.position = Vector3.Lerp(transform.position, pos, movementSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, pos, currentSpeed * Time.deltaTime);

            /*Vector3 desiredVelocity;
            float distanceFromPosition = (pos - transform.position).magnitude;
            float stoppingFactor;

            stoppingFactor = Mathf.Clamp(distanceFromPosition / slowingRadius, 0.0f, 1.0f);

            desiredVelocity = (pos - transform.position).normalized * maxMovementSpeed * stoppingFactor;

            rb.velocity += desiredVelocity - rb.velocity;*/

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

    public void SetPositionImmediately()
    {
        transform.position = rail.ProjectPosition(target.position, true);
        transform.rotation = Quaternion.Euler(rail.ProjectRotation(target.position, transform.position));
    }
}
