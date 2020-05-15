using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [TagSelector] public string[] TagsToCollision = new string[] { };
    [HideInInspector] public GameObject hitObject;

    private BoxCollider _trigger;
    public Action OnHit = delegate { };

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (TagsToCollision.Contains(other.tag))
        {
            hitObject = other.gameObject;
            OnHit();
        }
    }

    public void EnableTrigger()
    {
        _trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        _trigger.enabled = false;
    }
}
