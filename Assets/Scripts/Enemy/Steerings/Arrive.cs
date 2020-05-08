using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringsController))]

public class Arrive : Steerings
{
    public float slowingDistance;

    Vector3 desiredVelocity;

    private SteeringsController steeringOutput;

    void Start()
    {
        steeringOutput = GetComponent<SteeringsController>();
    }

    public override void Update()
    {
        float TargetDistance = (steeringOutput.target.position - transform.position).magnitude;
        float stoppingFactor;

        if (slowingDistance > 0)
        {
            stoppingFactor = Mathf.Clamp(TargetDistance / slowingDistance, 0.0f, 1.0f);
        }
        else
        {
            stoppingFactor = Mathf.Clamp(TargetDistance, 0.0f, 1.0f);
        }

        desiredVelocity = (steeringOutput.target.position - transform.position).normalized * steeringOutput.maxSpeed * stoppingFactor;

        // Calculate steering force
        steeringForce = desiredVelocity - steeringOutput.rb.velocity;
    }
}
