using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : State
{
    private PlayerMovementController _controller;
    private PlayerSwordScanner _scannerSword;
    private PlayerSounds _playerSounds;

    private EventInstance dragSoundEvent;

    public PushState(PlayerMovementController controller)
    {
        _controller = controller;
        _scannerSword = controller.scannerSword;
        _playerSounds = controller.playerSounds;
    }

    public override void Enter()
    {
        Debug.Log("Push State");
        _controller.animator.SetBool("isGrabbing", true);
        if (_scannerSword.UsingScannerInHand())
        {
            _scannerSword.ScannerOff();
        }
        _controller.RotateTowardsForward(GetLookCenterVector());
        if (UIHelperController.Instance.actionToComplete == UIHelperController.HelperAction.Drag) UIHelperController.Instance.DisableHelper();
    }

    public override void Update()
    {
        var vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);

        if (_controller.moveObject && _controller.moveObject.canMove && _controller.inputMoveObject && !_controller.scannerSword.UsingScannerInHand() && vector3D.magnitude >= _controller.joystickDeadZone)
        {
            if (PlayerUtils.InputDirectionTolerance(_controller.moveObject.moveVector, _controller.moveObject.angleToAllowMovement, _controller.cameraTransform, _controller.movementVector) && _controller.moveObject.canPull)
            {
                _controller.characterController.Move(_controller.moveObject.moveVector * (_controller.moveObject.speedWhenMove * Time.deltaTime));
                _controller.moveObject.UnlockPosConstraints();
                _controller.moveObject.Pull();
                _controller.animator.SetBool("isDragging", true);
                _controller.animator.SetBool("isPushing", false);
            }

            if (PlayerUtils.InputDirectionTolerance(-_controller.moveObject.moveVector, _controller.moveObject.angleToAllowMovement, _controller.cameraTransform, _controller.movementVector) && _controller.moveObject.canPush)
            {
                _controller.characterController.Move(-_controller.moveObject.moveVector * (_controller.moveObject.speedWhenMove * Time.deltaTime));
                _controller.moveObject.UnlockPosConstraints();
                _controller.moveObject.Push();
                _controller.animator.SetBool("isPushing", true);
                _controller.animator.SetBool("isDragging", false);
            }

            if (!_controller.moveObject.moving)
            {
                if (_controller.moveObject.swordStabbed) _controller.scannerIntersect.DeleteIntersections();
                else _controller.scannerIntersect.CheckIntersections(_controller.moveObject.GetComponent<BoxCollider>());
                _controller.moveObject.moving = true;
            }

            if (!AudioManager.Instance.isPlaying(dragSoundEvent))
            {
                if (AudioManager.Instance.ValidEvent(_playerSounds.draggableObjectSoundPath))
                {
                    dragSoundEvent = AudioManager.Instance.PlayEvent(_playerSounds.landSoundPath, _controller.transform);
                }
            }


        }
        else if (_controller.moveObject && vector3D.magnitude < _controller.joystickDeadZone)
        {
            _controller.moveObject.moving = false;
            _controller.animator.SetBool("isPushing", false);
            _controller.animator.SetBool("isDragging", false);

            dragSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        ExitState();
    }

    public override void ExitState()
    {
        if (!_controller.inputMoveObject || !_controller.moveObject.canMove || !_controller.moveObject.HasFloor())
        {
            if (_controller.moveObject.HasFloor())
            {
                _controller.moveObject.LockAllConstraints();
            }
            else
            {
                _controller.moveObject.UnlockAllConstrains();
            }

            _controller.CheckCollisions();
            _controller.SetState(new MoveState(_controller));
            _controller.animator.SetBool("isGrabbing", false);
        }
    }

    private Vector3 GetLookCenterVector()
    {
        return -_controller.moveObject.GetClosestVector();
    }
}
