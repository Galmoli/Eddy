using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    private PlayerMovementController _controller;
    private Vector3 vector3D;

    public MoveState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        Debug.Log("Move State");
    }

    public override void Update()
    {
        vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        _controller.RotateTowardsForward(vector3D);
        vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);
        _controller.characterController.Move(vector3D * Time.deltaTime);
        
        ExitState();
    }

    public override void ExitState()
    {
        if (_controller.jump)
        {
            _controller.verticalSpeed = _controller.jumpSpeed;
            _controller.SetState(new JumpState(_controller));
        }
        if (!Physics.CheckSphere(_controller.feetOverlap.position, 0.1f, LayerMask.NameToLayer("Player"))) _controller.SetState(new JumpState(_controller));

        if (_controller.moveObject && _controller.moveObject.canMove && _controller.inputMoveObject && !_controller.scannerSword.UsingScannerInHand())
        {
            _controller.SetState(new PushState(_controller));
        }
        
    }
}
