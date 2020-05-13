using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    private PlayerMovementController _controller;

    public JumpState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    public override void Enter()
    {
        Debug.Log("Jump State");
        _controller.jump = false;
        if(_controller.edgeAvailable) ExitState();
    }

    public override void Update()
    {
        var vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        _controller.RotateTowardsForward(vector3D);
        vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);

        if (_controller.verticalSpeed < -0.2f && _controller.edgeAvailable)
        {
            ExitState();
        }
        
        _controller.verticalSpeed += Physics.gravity.y * Time.deltaTime * _controller.gravityMultiplier;
        vector3D.y = _controller.verticalSpeed;
            
        vector3D.x = vector3D.x * _controller.speedMultiplierWhenJump;
        vector3D.z = vector3D.z * _controller.speedMultiplierWhenJump;
            
        var collisionFlags = _controller.characterController.Move(vector3D * Time.deltaTime);

        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            ExitState();
        }

        if ((collisionFlags & CollisionFlags.Above) != 0 && _controller.verticalSpeed > 0) _controller.verticalSpeed = 0;
    }

    public override void ExitState()
    {
        _controller.verticalSpeed = 0;
        if(_controller.edgeAvailable) _controller.SetState(new EdgeState(_controller));
        else _controller.SetState(new MoveState(_controller));
    }
}
