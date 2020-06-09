using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableRagdoll : MonoBehaviour
{
    Transform followObject;

    // Start is called before the first frame update
    void Start()
    {
        followObject = transform.parent;
        transform.parent = transform.parent.parent;       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followObject.position;
        transform.rotation = followObject.rotation;
    }
}
