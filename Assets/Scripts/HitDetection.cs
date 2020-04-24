using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [TagSelector]
    public string[] TagsToCollision = new string[] { };
    
    private bool _colliding;
    public bool Check()
    {
        return _colliding;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagsToCollision.Contains(other.tag)) _colliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagsToCollision.Contains(other.tag)) _colliding = false;
    }
}
