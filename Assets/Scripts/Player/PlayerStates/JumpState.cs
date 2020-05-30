using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpState : State
{
    private PlayerMovementController _controller;
    private PlayerSounds _playerSounds;
    private float currentTime = 0;
    private bool hitHead;
    private float _residualCollisionAvoidanceSpeed = 5;

    public JumpState(PlayerMovementController controller)
    {
        _controller = controller;
        _playerSounds = controller.playerSounds;
    }
    public override void Enter()
    {
        Debug.Log("Jump State");
        _controller.jump = false;
        if(_controller.edgeAvailable && PlayerUtils.CanInteractWithEdge(_controller.transform.forward, _controller.edgeGameObject.transform.forward, _controller.angleToAllowClimbEdge)) ExitState();
        _controller.animator.SetBool("isOnAir", true);
        if(UIHelperController.Instance.actionToComplete == UIHelperController.HelperAction.Jump) UIHelperController.Instance.DisableHelper();
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
            if (AudioManager.Instance.ValidEvent(_playerSounds.landSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(_playerSounds.landSoundPath, _controller.transform);
            }

            ExitState();
        }
    }

    public override void ExitState()
    {
        
        
        _controller.animator.SetBool("isOnAir", false);
        _controller.verticalSpeed = 0;
        
        if (_controller.edgeAvailable) _controller.SetState(new EdgeState(_controller));
        else _controller.SetState(new MoveState(_controller));
    }
    
    private bool CheckFloor(Collider[] colliders)
    {
        var list = colliders.ToList();
        if (colliders.Length > 0 && currentTime >= 0.2f)
        {
            //At the moment only the moveObject it's checked because it detects the trigger as floor
            //I don't know if it's possible to make it work with layers.
            //As it's the only problem for now, it will be hardcoded.

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
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f + fRight * 0.25f, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f - fRight * 0.25f, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f + fRight * 0.5f, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward * 0.5f - fRight * 0.5f, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        if (CheckFloor(PlayerUtils.GetResidualColliders(_controller, _controller.feetOverlap.position - fForward, sword)))
        {
            vector3D.x += t.forward.x;
            vector3D.z += t.forward.z;
        }
        
        //Other positions can be added here. But just checking the back solves the major problem.

        if (vector3D != Vector3.zero) vector3D = vector3D.normalized * _residualCollisionAvoidanceSpeed;
        return vector3D;
    }
}
