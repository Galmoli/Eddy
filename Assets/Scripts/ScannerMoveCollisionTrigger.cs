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
        var swordPos = _swordSphereCollider.transform.position;
        var colliderOrigin = swordPos + (_playerTransform.position - swordPos).normalized * 5;
        var direction = _playerTransform.position - colliderOrigin;
        
        Physics.Raycast(colliderOrigin, direction, out var hitInfo, 200f, LayerMask.GetMask("ScannerLayer"));
        
        _collider.transform.position = swordPos + GetEquatorialVector(hitInfo.point, swordPos, _swordSphereCollider.radius);
        _collider.transform.forward = -Vector3.ProjectOnPlane(_collider.transform.position - swordPos, Vector3.up);
    }

    private void SetAppearColliderPos()
    {
        if (!_boxCollider.bounds.Contains(_playerTransform.position + (_boxCollider.transform.position - _playerTransform.position).normalized * 0.5f)) return;
        
        _collider.gameObject.SetActive(true);
        var swordPos = _swordSphereCollider.transform.position;
        var direction = swordPos - _playerTransform.position;

        Physics.Raycast(_playerTransform.position, direction, out var hitInfo, 200f, LayerMask.GetMask("ScannerLayer"));
        Debug.DrawRay(_playerTransform.position, direction, Color.yellow);

        _collider.transform.position = swordPos + GetEquatorialVector(hitInfo.point, swordPos, _swordSphereCollider.radius);
        _collider.transform.forward = -Vector3.ProjectOnPlane(_collider.transform.position - _swordSphereCollider.transform.position, Vector3.up);
    }

    private Vector3 GetEquatorialVector(Vector3 hitPoint, Vector3 sphereCenter, float sphereRadius)
    {
        var angledVector = hitPoint - sphereCenter;
        var result = Vector3.ProjectOnPlane(angledVector, Vector3.up).normalized * sphereRadius;
        Debug.DrawRay(_swordSphereCollider.transform.position, result, Color.red);
        return result;
    }
}
