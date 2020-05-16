using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpState : State
{
    private PlayerMovementController _controller;
    private float currentTime = 0;
    private bool hitHead;

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
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;
        var vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        _controller.RotateTowardsForward(vector3D);
        vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);

        if (_controller.edgeAvailable && PlayerUtils.CanInteractWithEdge(_controller.transform.forward, _controller.edgeGameObject.transform.forward, _controller.angleToAllowClimbEdge))
        {
            ExitState();
        }
        
        _controller.verticalSpeed += Physics.gravity.y * Time.deltaTime * _controller.gravityMultiplier;
        vector3D.y = _controller.verticalSpeed;
            
        vector3D.x = vector3D.x * _controller.speedMultiplierWhenJump;
        vector3D.z = vector3D.z * _controller.speedMultiplierWhenJump;
            
        _controller.characterController.Move(vector3D * Time.deltaTime);

        CheckFloor(PlayerUtils.GetFloorColliders(_controller));
    }

    public override void ExitState()
    {
        _controller.animator.SetBool("isOnAir", false);
        _controller.verticalSpeed = 0;
        
        if (_controller.edgeAvailable) _controller.SetState(new EdgeState(_controller));
        else _controller.SetState(new MoveState(_controller));
    }
    
    private void CheckFloor(Collider[] colliders)
    {
        var list = colliders.ToList();
        if (colliders.Length > 0 && currentTime >= 0.2f)
        {
            //At the moment only the moveObject it's checked because it detects the trigger as floor
            //I don't know if it's possible to make it work with layers.
            //As it's the only problem for now, it will be hardcoded.
            
            if(list.All(c => c.name != "Trigger")) ExitState();
            else if(list.Any(c => c.CompareTag("MoveObject")) && colliders.Length != 1) ExitState();
        }
    }
}
