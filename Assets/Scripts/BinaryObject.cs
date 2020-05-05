using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryObject : MonoBehaviour
{
    [SerializeField] private Collider collider1;
    [SerializeField] private Collider collider2;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Normal"))
        {
            collider1.enabled = true;
            collider2.enabled = false;
        }
        else
        {
            collider1.enabled = false;
            collider2.enabled = true;
        }
    }
}
