using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordProgressiveColliders : MonoBehaviour
{
    [HideInInspector] public bool swordActive;
    public LayerMask _layerNormal;
    private List<Collider> affectedList = new List<Collider>();
    private List<GameObject> _playerGameObjects = new List<GameObject>();

    private void Awake()
    {
        var player = GameObject.Find("Player");
        var children = player.GetComponentsInChildren<Transform>();
        
        foreach (var c in children)
        {
            if(c.gameObject.layer == LayerMask.NameToLayer("Player")) _playerGameObjects.Add(c.gameObject);
        }
    }

    public void EnableSword()
    {
        swordActive = true;
        affectedList = Physics.OverlapSphere(transform.position, 4, _layerNormal).ToList();
        SetLayer(affectedList, LayerMask.NameToLayer("inScanner"));
        SetPlayerObjectsToLayer(LayerMask.NameToLayer("inScanner"));
    }

    public void DisableSword()
    {
        swordActive = false;
        SetLayer(affectedList, LayerMask.NameToLayer("Normal"));
        SetPlayerObjectsToLayer(LayerMask.NameToLayer("Player"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Normal"))
        {
            affectedList.Add(other);
            other.gameObject.layer = LayerMask.NameToLayer("inScanner");
        }
        else if(other.gameObject == _playerGameObjects[0])
        {
            SetPlayerObjectsToLayer(LayerMask.NameToLayer("inScanner"));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("inScanner"))
        {
            affectedList.Remove(other);
            if (other.gameObject == _playerGameObjects[0])
            {
                SetPlayerObjectsToLayer(LayerMask.NameToLayer("Player"));
            }
            else if(!_playerGameObjects.Contains(other.gameObject)) other.gameObject.layer = LayerMask.NameToLayer("Normal");
        }
    }

    private void SetPlayerObjectsToLayer(int layer)
    {
        foreach (var go in _playerGameObjects)
        {
            go.layer = layer;
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
