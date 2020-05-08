using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringsController : MonoBehaviour
{
    public Transform target;

    public float maxSteeringForce;
    public float maxSpeed;
    public float rotationSpeed;

    [HideInInspector] public Rigidbody rb;

    private List<Steerings> steeringsList = new List<Steerings>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Steerings[] steeringsArray = GetComponents<Steerings>();

        for (int i = 0; i < steeringsArray.Length; i++)
        {
            steeringsList.Add(steeringsArray[i]);
        }
    }

    void Update()
    {
        Vector3 steeringForceAverage = Vector3.zero;
        float priorityScale = 1;

        for (int i = 0; i < steeringsList.Count; i++)
        {
            if (steeringsList.Count > 1)
            {
                priorityScale = steeringsList[i].combinationScale;
            }

            steeringForceAverage += steeringsList[i].steeringForce * priorityScale;
        }

        steeringForceAverage.y = 0;
        steeringForceAverage = Vector3.ClampMagnitude(steeringForceAverage, maxSteeringForce);

        rb.velocity += steeringForceAverage;


        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime * rotationSpeed);
        }
    }
}
