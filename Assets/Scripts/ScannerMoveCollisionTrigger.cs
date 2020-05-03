using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerMoveCollisionTrigger : MonoBehaviour
{
    [SerializeField] SphereCollider _swordSphereCollider;
    [SerializeField] private GameObject _collider;
    
    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = transform.root;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hide") || other.gameObject.layer == LayerMask.NameToLayer("Appear"))
        {
            SetColliderPos();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _collider.gameObject.SetActive(false);
    }

    private void SetColliderPos()
    {
        _collider.gameObject.SetActive(true);
        RaycastHit hitInfo;
        Physics.Raycast(_playerTransform.position + _playerTransform.forward.normalized * 5, -_playerTransform.forward.normalized, out hitInfo, 200f, LayerMask.GetMask("ScannerLayer"));
        _collider.transform.position = hitInfo.point;
        _collider.transform.forward = _collider.transform.position - _swordSphereCollider.transform.position;
    }
}
