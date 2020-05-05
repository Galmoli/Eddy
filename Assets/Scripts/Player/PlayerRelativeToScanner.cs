using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRelativeToScanner : MonoBehaviour
{
    [SerializeField] private Transform rightCheck;
    [SerializeField] private Transform leftCheck;
    [SerializeField] private SphereCollider _swordSphereCollider;
    
    private List<GameObject> _playerGameObjects = new List<GameObject>();
    private void Awake()
    {
        var children = transform.root.GetComponentsInChildren<Transform>();
        
        foreach (var c in children)
        {
            if(c.gameObject.layer == LayerMask.NameToLayer("Player")) _playerGameObjects.Add(c.gameObject);
        }
    }

    private void Update()
    {
        
        if (Vector3.Distance(_swordSphereCollider.transform.position, rightCheck.position) <= _swordSphereCollider.radius && 
            Vector3.Distance(_swordSphereCollider.transform.position, leftCheck.position) <= _swordSphereCollider.radius && 
            transform.root.gameObject.layer != LayerMask.NameToLayer("inScanner"))
        {
            SetPlayerObjectsToLayer(LayerMask.NameToLayer("inScanner"));
        }
        else if (Vector3.Distance(_swordSphereCollider.transform.position, rightCheck.position) > _swordSphereCollider.radius && 
                 Vector3.Distance(_swordSphereCollider.transform.position, leftCheck.position) > _swordSphereCollider.radius &&
                 transform.root.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            SetPlayerObjectsToPlayerLayer();
        }
    }

    private void SetPlayerObjectsToLayer(int layer)
    {
        foreach (var go in _playerGameObjects)
        {
            go.layer = layer;
        }
    }
    
    private void SetPlayerObjectsToPlayerLayer()
    {
        SetPlayerObjectsToLayer(LayerMask.NameToLayer("Player"));
    }
    
}
