using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeState : State
{
    private PlayerMovementController _controller;
    private PlayerSwordScanner _scannerSword;

    public EdgeState(PlayerMovementController controller)
    {
        _controller = controller;
        _scannerSword = controller.scannerSword;
    }
    public override void Enter()
    {
        Debug.Log("Edge State");
        _controller.onEdge = true;

        if (_scannerSword.UsingScannerInHand())
        {
            _scannerSword.ScannerOff();
        }

        if (_controller.edgeGameObject.transform.position.y > _controller.transform.position.y) _controller.animator.SetTrigger("Hanging");
    }

    public override void Update()
    {
        if (_controller.standing) return;
        
        if (_controller.onEdge && !_controller.standing)
        {
            var position = _controller.transform.position;
            Vector3 moveVector = Vector3.zero;
            
            Vector3 lVector = Vector3.Lerp(position, _controller.edgePosition + _controller.edgeOffset, _controller.lerpVelocity);
            
            moveVector = new Vector3(0, (lVector - position).y, 0);
            
            _controller.RotateTowardsForward(-_controller.edgeGameObject.transform.forward);
            
            if(_controller.characterController.enabled) _controller.characterController.Move(moveVector);
        }
        
        var projectedVector = Vector3.ProjectOnPlane(_controller.transform.position - _controller.edgePosition, _controller.edgeGameObject.transform.forward);
        projectedVector = Vector3.ProjectOnPlane(projectedVector, _controller.transform.up);

        if (PlayerUtils.InputEqualVector(-_controller.edgeGameObject.transform.forward, _controller.cameraTransform, _controller.movementVector) && !_controller.standing || _controller.inputToStand && !_controller.standing)
        {
            _controller.StandEdge(projectedVector + _controller.edgePosition  + _controller.edgeCompletedOffset);
            _controller.animator.SetTrigger("Climb");
        }
        if (PlayerUtils.InputEqualVector(_controller.edgeGameObject.transform.forward, _controller.cameraTransform, _controller.movementVector) && !_controller.standing)
        {
            _controller.onEdge = false;
            _controller.edgeAvailable = false;
            ExitState();
        }
    }

    public override void ExitState()
    {
        if (!_controller.onEdge)
        {
            _controller.edgeAvailable = false;
            _controller.SetState(new JumpState(_controller));
        }
    }
}
