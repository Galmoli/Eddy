using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpState : State
{
    private PlayerMovementController _controller;
    private float currentTime = 0;

    public JumpState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    public override void Enter()
    {
        Debug.Log("Jump State");
        _controller.jump = false;
        if(_controller.edgeAvailable) ExitState();
        _controller.animator.SetBool("isOnAir", true);
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;
        var vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        _controller.RotateTowardsForward(vector3D);
        vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);

        if (_controller.edgeAvailable)
        {
            ExitState();
        }
        
        _controller.verticalSpeed += Physics.gravity.y * Time.deltaTime * _controller.gravityMultiplier;
        vector3D.y = _controller.verticalSpeed;
            
        vector3D.x = vector3D.x * _controller.speedMultiplierWhenJump;
        vector3D.z = vector3D.z * _controller.speedMultiplierWhenJump;
            
        var collisionFlags = _controller.characterController.Move(vector3D * Time.deltaTime);

        CheckFloor(GetFloorColliders());

        if ((collisionFlags & CollisionFlags.Above) != 0 && _controller.verticalSpeed > 0) _controller.verticalSpeed = 0;
    }

    public override void ExitState()
    {
        _controller.animator.SetBool("isOnAir", false);
        _controller.verticalSpeed = 0;
        if(_controller.edgeAvailable) _controller.SetState(new EdgeState(_controller));
        else _controller.SetState(new MoveState(_controller));
    }

    private Collider[] GetFloorColliders()
    {
        if (_controller.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return Physics.OverlapSphere(_controller.feetOverlap.position, 0.1f, _controller.layersToCheckFloorOutsideScanner);
        }
        return Physics.OverlapSphere(_controller.feetOverlap.position, 0.1f, _controller.layersToCheckFloorInsideScanner);
    }

    private void CheckFloor(Collider[] colliders)
    {
        var list = colliders.ToList();
        if (colliders.Length > 0 && currentTime >= 0.2f)
        {
            if(list.All(c => c.name != "Trigger")) ExitState();
            else if(list.Any(c => c.CompareTag("MoveObject"))) ExitState();
        }
    }
}
