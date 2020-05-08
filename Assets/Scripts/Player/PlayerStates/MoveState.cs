using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private PlayerMovementController _controller;
    private Vector3 vector3D;
    private Vector3 verticalSnap;

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
        SnapToFloor();
        
        _controller.characterController.Move(vector3D * Time.deltaTime + verticalSnap);
        
        ExitState();
    }

    private void SnapToFloor()
    {
        float snapDistance = 0.4f;
        LayerMask layer;
        if (_controller.gameObject.layer == LayerMask.NameToLayer("Player")) layer = _controller.layersToCheckFloorOutsideScanner;
        else layer = _controller.layersToCheckFloorInsideScanner;
        
        if (!_controller.characterController.isGrounded)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(new Ray(_controller.feetOverlap.position, Vector3.down), out hitInfo, snapDistance, layer))
            {
                verticalSnap = hitInfo.point - _controller.transform.position;
            }
        }
        else
        {
            verticalSnap = Vector3.zero;
        }
    }

    public override void ExitState()
    {
        if (_controller.jump)
        {
            _controller.verticalSpeed = _controller.jumpSpeed;
            _controller.SetState(new JumpState(_controller));
        }

        if (_controller.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!Physics.CheckSphere(_controller.feetOverlap.position, 0.1f,_controller.layersToCheckFloorOutsideScanner))
            {
                _controller.SetState(new JumpState(_controller));
            }
        }
        
        else if (_controller.gameObject.layer == LayerMask.NameToLayer("inScanner"))
        {
            if (!Physics.CheckSphere(_controller.feetOverlap.position, 0.1f,_controller.layersToCheckFloorInsideScanner))
            {
                _controller.SetState(new JumpState(_controller));
            }
        }

        if (_controller.moveObject && _controller.moveObject.canMove && _controller.inputMoveObject && !_controller.scannerSword.UsingScannerInHand())
        {
            _controller.SetState(new PushState(_controller));
        }
    }
}
