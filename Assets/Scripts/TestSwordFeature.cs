using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSwordFeature : MonoBehaviour
{
    [HideInInspector] public bool swordActive;
    public LayerMask _layerNormal;
    private List<Collider> affectedList = new List<Collider>();
    public void EnableSword()
    {
        swordActive = true;
        affectedList = Physics.OverlapSphere(transform.position, 4, _layerNormal).ToList();
        SetLayer(affectedList, 19);
        GameObject.Find("Player").layer = 19;
    }

    public void DisableSword()
    {
        swordActive = false;
        SetLayer(affectedList, 18);
        GameObject.Find("Player").layer = 11;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 18)
        {
            affectedList.Add(other);
            other.gameObject.layer = 19;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 19)
        {
            affectedList.Remove(other);
            other.gameObject.layer = 18;
        }
    }

    private void SetLayer(List<Collider> colliders, int layer)
    {
        foreach (var c in colliders)
        {
            c.gameObject.layer = layer;
        }
    }
}
