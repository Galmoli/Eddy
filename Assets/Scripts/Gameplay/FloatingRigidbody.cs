using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class FloatingRigidbody : MonoBehaviour
{
    public float minTorque, maxTorque;
    public float minForce, maxforce;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        rb.AddTorque(Random.Range(minTorque, maxTorque) * Random.insideUnitSphere);
        rb.AddForce(Random.Range(minForce, maxforce) * Random.insideUnitSphere, ForceMode.Impulse);
    }


}
