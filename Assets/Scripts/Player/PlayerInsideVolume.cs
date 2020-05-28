using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInsideVolume : MonoBehaviour
{
    [SerializeField] private float radiusToCheck = 0.5f;
    [SerializeField] private float maxAxisX = 0.5f;
    [SerializeField] private float maxAxisZ = 0.5f;
    private PlayerMovementController _controller;
    private PlayerSwordScanner _sword;
    private bool _disableLater;
    private void Awake()
    {
        _controller = GetComponent<PlayerMovementController>();
        _sword = FindObjectOfType<PlayerSwordScanner>();
    }

    private void Update()
    {
        if (!_disableLater) return;

        bool stillInCollider = false;
        Collider[] colliders = Physics.OverlapSphere(_controller.transform.position, radiusToCheck);
        foreach (var c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Hide"))
            {
                if (c.bounds.size.x >= maxAxisX && c.bounds.size.x >= maxAxisZ)
                {
                    stillInCollider = true;
                }
            }
        }

        if (!stillInCollider)
        {
            _disableLater = false;
            _sword.ScannerOff();
        }
    }

    public bool CanActivateScanner()
    {
        Collider[] colliders = Physics.OverlapSphere(_controller.transform.position, radiusToCheck);
        foreach (var c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Appear"))
            {
                if (c.bounds.size.x >= maxAxisX && c.bounds.size.x >= maxAxisZ)
                {
                    UIManager.Instance.ShowScannerWarning();
                    return false;
                }

                //Move Player
                var moveDir = _controller.transform.position - c.transform.position;
                moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
                _controller.characterController.Move(moveDir * 0.7f);
                return true;
            }
        }

        return true;
    }

    public bool CanDisableScanner()
    {
        Collider[] colliders = Physics.OverlapSphere(_controller.transform.position, radiusToCheck);
        foreach (var c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Hide"))
            {
                if (c.bounds.size.x >= maxAxisX && c.bounds.size.x >= maxAxisZ)
                {
                    UIManager.Instance.ShowScannerWarning();
                    _disableLater = true;
                    return false;
                }

                //Move Player
                var moveDir = _controller.transform.position - c.transform.position;
                moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
                _controller.characterController.Move(moveDir * 0.7f);
                return true;
            }
        }
        return true;
    }
}
