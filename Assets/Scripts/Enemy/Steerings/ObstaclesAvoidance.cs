using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringsController))]

public class ObstaclesAvoidance : Steerings
{
    [Header("Steering Parameters")]  
    public LayerMask detectionLayers;
    public float boundingSphereRadius;
    public float obstacleMaxDistance;

    //private float m_SteeringForceConservation = 0.9f;

    //private float m_SteeringForceConservationDuration = 1;

    public float angle;

    // Steering force conservation timer
    //private float m_SteeringForceConservationTimer = 0;

    // Old valid steering force
    //private Vector3 m_OldValidSteeringForce = Vector3.zero;

    Vector3 desiredVelocity;

    private SteeringsController steeringOutput;

    void Start()
    {
        steeringOutput = GetComponent<SteeringsController>();
    }

    public override void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        Vector3 avoidanceForce = Vector3.zero;

        if (Physics.SphereCast(ray, boundingSphereRadius, out hitInfo, obstacleMaxDistance, detectionLayers))
        {
            if (Vector3.Angle(hitInfo.normal, transform.up) > angle)
            {
                avoidanceForce = Vector3.Reflect(steeringOutput.rb.velocity, hitInfo.normal);

                if (Vector3.Dot(avoidanceForce, steeringOutput.rb.velocity) < -0.9f)
                {
                    avoidanceForce = transform.right;
                }
            }
        }

        if (avoidanceForce != Vector3.zero)
        {
            desiredVelocity = avoidanceForce.normalized * steeringOutput.maxSpeed;

            steeringForce = desiredVelocity - steeringOutput.rb.velocity;
            //m_OldValidSteeringForce = steeringForce;
            //m_SteeringForceConservationTimer = 0;
        }
        else
        {
            steeringForce = Vector3.zero;
        }
    }
}
