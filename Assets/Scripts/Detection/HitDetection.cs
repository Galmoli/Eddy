using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public struct HitStruct
    {
        public bool colliding;
        public GameObject hitObject;
    }
    
    [TagSelector]
    public string[] TagsToCollision = new string[] { };
    [HideInInspector] public HitStruct hit;
    public HitStruct Check()
    {
        return hit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagsToCollision.Contains(other.tag))
        {
            hit.colliding = true;
            hit.hitObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagsToCollision.Contains(other.tag))
        {
            hit.colliding = false;
            hit.hitObject = null;
        }
    }
}
