using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringsController))]

public class Seek : Steerings
{
    Vector3 desiredVelocity;

    private SteeringsController steeringOutput;

    void Start()
    {
        steeringOutput = GetComponent<SteeringsController>();
    }

    public override void Update()
    {
        desiredVelocity = (steeringOutput.target.position - transform.position).normalized * steeringOutput.maxSpeed;

        steeringForce = desiredVelocity - steeringOutput.rb.velocity;
    }
}
