using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpState : State
{
    private PlayerMovementController _controller;
    private float currentTime = 0;
    private bool _hitHead;
    private bool _onEnemy;
    private float _residualCollisionAvoidanceSpeed = 5;

    public JumpState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    public override void Enter()
    {
        Debug.Log("Jump State");
        _controller.jump = false;
        if(_controller.edgeAvailable && PlayerUtils.CanInteractWithEdge(_controller.transform.forward, _controller.edgeGameObject.transform.forward, _controller.angleToAllowClimbEdge)) ExitState();
        _controller.animator.SetBool("isOnAir", true);
        if(UIHelperController.Instance.actionsToComplete.Contains(UIHelperController.HelperAction.Jump)) UIHelperController.Instance.DisableHelper(1, UIHelperController.HelperAction.Jump);
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;
        var vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        _controller.RotateTowardsForward(vector3D);
        
        if (!UIManager.Instance.popUpEnabled) vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);
        else vector3D *= _controller.minSpeed;

        if (_controller.edgeAvailable && PlayerUtils.CanInteractWithEdge(_controller.transform.forward, _controller.edgeGameObject.transform.forward, _controller.angleToAllowClimbEdge))
        {
            ExitState();
        }
        
        _controller.verticalSpeed += Physics.gravity.y * Time.deltaTime * _controller.gravityMultiplier;
        vector3D.y = _controller.verticalSpeed;
            
        vector3D.x = vector3D.x * _controller.speedMultiplierWhenJump;
        vector3D.z = vector3D.z * _controller.speedMultiplierWhenJump;
        vector3D += CheckResidualCollisions();
            
        _controller.characterController.Move(vector3D * Time.deltaTime);

        if(CheckFloor(PlayerUtils.GetFloorColliders(_controller, _controller.feetOverlap.position)))
        {
            _controller.LandingSound();
            ExitState();
        }
    }

    public override void ExitState()
    {
        _controller.animator.SetBool("isOnAir", false);
        _controller.verticalSpeed = 0;
        _onEnemy = false;
        
        if (_controller.edgeAvailable && ValidEdge()) _controller.SetState(new EdgeState(_controller));
        else _controller.SetState(new MoveState(_controller));
    }
    
    private bool CheckFloor(Collider[] colliders)
    {
        var list = colliders.ToList();
        if (colliders.Length > 0 && currentTime >= 0.2f)
        {
            if (list.Any(c => c.CompareTag("Enemy")))
            {
                _onEnemy = true;
                return false;
            }
            if (list.All(c => c.name != "Trigger")) return true;
            if (list.Any(c => c.CompareTag("MoveObject")) && colliders.Length != 1) return true;
        }
        return false;
    }

    private Vector3 CheckResidualCollisions()
    {
        var vector3D = Vector3.zero;
        var t = _controller.transform;
        var fForward = _controller.feetOverlap.forward;
        var fRight = _controller.feetOverlap.right;
        var sword = _controller.scannerSword;
        bool _backForceApplied = false;
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        } else if (_onEnemy)
        {
            vector3D.x += t.forward.x * 0.5f;
            vector3D.z += t.forward.z * 0.5f;
            _backForceApplied = true;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f + fRight * 0.25f, sword)))
        {
            vector3D.x += t.forward.x * 0.5f;
            vector3D.z += t.forward.z * 0.5f;
            vector3D.x -= t.right.x * 0.5f;
            vector3D.z -= t.right.z * 0.5f;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f - fRight * 0.25f, sword)))
        {
            vector3D.x += t.forward.x * 0.5f;
            vector3D.z += t.forward.z * 0.5f;
            vector3D.x += t.right.x * 0.5f;
            vector3D.z += t.right.z * 0.5f;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f + fRight * 0.5f, sword)))
        {
            vector3D.x -= t.right.x;
            vector3D.z -= t.right.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f - fRight * 0.5f, sword)))
        {
            vector3D.x += t.right.x;
            vector3D.z += t.right.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position + fRight * 0.5f, sword)))
        {
            vector3D.x -= t.right.x * 0.25f;
            vector3D.z -= t.right.z * 0.25f;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fRight * 0.5f, sword)))
        {
            vector3D.x += t.right.x * 0.25f;
            vector3D.z += t.right.z * 0.25f;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position + fForward * 0.5f, sword)))
        {
            if (!PlayerUtils.HasObjectInFront(_controller, t.position, t.forward))
            {
                if (_onEnemy && !_backForceApplied)
                {
                    vector3D.x -= t.forward.x * 0.5f;
                    vector3D.z -= t.forward.z * 0.5f;
                }
                else
                {
                    vector3D.x += t.forward.x;
                    vector3D.z += t.forward.z;
                }
            }
        }

        if (_onEnemy  && !_backForceApplied)
        {
            vector3D.x -= t.forward.x;
            vector3D.z -= t.forward.z;
        }
        
        //Other positions can be added here. But just checking the back solves the major problem.

        if (vector3D != Vector3.zero) vector3D = vector3D.normalized * _residualCollisionAvoidanceSpeed;
        return vector3D.normalized;
    }

    private bool ValidEdge()
    {
        if(_controller.edgeGameObject.layer == LayerMask.NameToLayer("Appear"))
        {
            if (_controller.scannerSword.UsingScannerInHand())
            {
                _controller.scannerSword.ScannerOff();
                return false;
            }
        }

        return true;
    }
}
