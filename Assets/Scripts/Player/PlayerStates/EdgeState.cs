using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeState : State
{
    private PlayerMovementController _controller;
    private PlayerSwordScanner _scannerSword;
    private SphereCollider _scannerCollider;
    private EdgeDetection _edgeDetection;
    private bool autoStand;
    private Vector3 _projectedVector;

    public EdgeState(PlayerMovementController controller)
    {
        _controller = controller;
        _scannerSword = controller.scannerSword;
        _scannerCollider = controller.scannerCollider;
        _edgeDetection = GameObject.FindObjectOfType<EdgeDetection>();
    }
    public override void Enter()
    {
        Debug.Log("Edge State");
        _controller.onEdge = true;
        
        _projectedVector = _controller.GetProjectedVector();
        
        if (_controller.edgeGameObject.transform.position.y > _controller.transform.position.y) _controller.animator.SetTrigger("Hanging");
        
        TriggerDesiredAnimation(_controller.transform.position, _controller.edgePosition);
    }

    public override void Update()
    {
        if (_controller.standing || autoStand) return;
        
        //Puts player on grab position. 
        //This will only be used on hang on edge.
        if (_controller.onEdge && !_controller.standing)
        {
            var position = _controller.transform.position;
            Vector3 moveVector = Vector3.zero;
            
            Vector3 lVector = Vector3.Lerp(position,_controller.edgePosition + _projectedVector  + PlayerUtils.GetEdgeOffsetOnLocalSpace(_controller.edgeGameObject,_controller.edgeOffset), _controller.lerpVelocity);

            moveVector = lVector - position;
            _controller.RotateTowardsForward(-_controller.edgeGameObject.transform.forward);
            
            if(_controller.characterController.enabled) _controller.characterController.Move(moveVector);
        }

        if (PlayerUtils.InputEqualVector(-_controller.edgeGameObject.transform.forward, _controller.cameraTransform, _controller.movementVector) && !_controller.standing || _controller.inputToStand && !_controller.standing)
        {
            _controller.animator.SetTrigger("Climb");
            _controller.StandDeactivatePlayer();
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

    private void TriggerDesiredAnimation(Vector3 playerPos, Vector3 edgePos)
    {
        if (playerPos.y + _controller.edgeOffsetToWaist < edgePos.y) //Normal handing
        {
            Debug.Log("Normal");
            if (_scannerSword.UsingScannerInHand()) _scannerSword.ScannerOff();
            return; 
        }
        if (playerPos.y + _controller.edgeOffsetToKnee > edgePos.y) //Trigger Knee animation
        {
            Debug.Log("Knee");
            autoStand = true;
            //_controller.StandDeactivatePlayer();
            _controller.WaistStand();
            _controller.animator.SetTrigger("MidClimb");
        }
        else //Trigger waist animation
        {
            Debug.Log("Waist");
            autoStand = true;
            //_controller.StandDeactivatePlayer();
            _controller.WaistStand();
            _controller.animator.SetTrigger("MidClimb");
        }
    }
}
