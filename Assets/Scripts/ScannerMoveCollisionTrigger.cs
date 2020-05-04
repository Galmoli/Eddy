using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerMoveCollisionTrigger : MonoBehaviour
{
    [SerializeField] SphereCollider _swordSphereCollider;
    [SerializeField] private GameObject _collider;
    private BoxCollider _boxCollider;
    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = transform.root;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EdgeTrigger")) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Hide") || other.gameObject.layer == LayerMask.NameToLayer("Appear"))
        {
            _boxCollider = other.GetComponent<BoxCollider>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hide"))
        {
            SetHideColliderPos();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Appear"))
        {
            SetAppearColliderPos();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _collider.gameObject.SetActive(false);
    }

    private void SetHideColliderPos()
    {
        if (!_boxCollider.bounds.Contains(_playerTransform.position + _playerTransform.forward.normalized * 0.5f)) return;
        
        _collider.gameObject.SetActive(true);
        Physics.Raycast(_playerTransform.position + _playerTransform.forward.normalized * 5, -_playerTransform.forward.normalized, out var hitInfo, 200f, LayerMask.GetMask("ScannerLayer"));
        _collider.transform.position = hitInfo.point;
        _collider.transform.forward = _collider.transform.position - _swordSphereCollider.transform.position;
    }

    private void SetAppearColliderPos()
    {
        if (!_boxCollider.bounds.Contains(_playerTransform.position + (_boxCollider.transform.position - _playerTransform.position).normalized * 0.5f)) return;
        
        _collider.gameObject.SetActive(true);
        Physics.Raycast(_playerTransform.position, _swordSphereCollider.transform.position - _playerTransform.position, out var hitInfo, 200f, LayerMask.GetMask("ScannerLayer"));
        _collider.transform.position = hitInfo.point;
        _collider.transform.forward = _collider.transform.position - _swordSphereCollider.transform.position;
    }
}
