using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehaviour : MonoBehaviour
{
    public GameObject targetCol;
    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetCol.layer == LayerMask.NameToLayer("inScanner"))
        {
            rigidbody.AddForce(Vector3.up * 50, ForceMode.Force);
        }
    }
}
