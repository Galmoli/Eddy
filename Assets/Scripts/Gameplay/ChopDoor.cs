using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDoor : MonoBehaviour
{
    public float force;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(force * Vector3.down, ForceMode.Force);
    }
}
