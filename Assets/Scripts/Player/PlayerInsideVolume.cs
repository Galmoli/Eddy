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
    private void Awake()
    {
        _controller = GetComponent<PlayerMovementController>();
    }

    public bool CanActivateScanner()
    {
        Collider[] colliders = Physics.OverlapSphere(_controller.feetOverlap.position, radiusToCheck);
        foreach (var c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Appear") && c.transform.root == c.transform)
            {
                if (c.bounds.size.x >= maxAxisX && c.bounds.size.x >= maxAxisZ)
                {
                    print("Cant activate sword");
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
        Collider[] colliders = Physics.OverlapSphere(_controller.feetOverlap.position, radiusToCheck);
        foreach (var c in colliders)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Hide") && c.transform.root == c.transform)
            {
                if (c.bounds.size.x >= maxAxisX && c.bounds.size.x >= maxAxisZ)
                {
                    print("Cant disable sword sword");
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
