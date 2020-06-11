using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        _controller.edgeAvailable = false;
    }

    public override void Update()
    {
        if (UIManager.Instance.paused)
        {
            _controller.animator.SetFloat("Speed", 0);
            return;
        }
        
        vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
        _controller.RotateTowardsForward(vector3D);

        if (UIHelperController.Instance.actionToComplete == UIHelperController.HelperAction.Move && _controller.movementVector.magnitude >= 0.7f)
        {
            UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Jump, _controller.transform.position + Vector3.up * 2, _controller.transform);
            UIHelperController.Instance.DisableHelper(UIHelperController.HelperAction.Move);
        } 
        
        if (!UIManager.Instance.popUpEnabled) vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);
        else vector3D *= _controller.minSpeed;
        
        SnapToFloor();
        
        if (!_controller.combatController.nextAttackReserved)
        {
            _controller.characterController.Move(vector3D * Time.deltaTime + verticalSnap);
            _controller.animator.SetFloat("Speed", vector3D.magnitude);
        }

        ExitState();
    }

    private void SnapToFloor()
    {
        float snapDistance = 0.4f;
        LayerMask layer;
        if (_controller.gameObject.layer == LayerMask.NameToLayer("Player")) layer = _controller.layersToCheckFloorOutsideScanner;
        else layer = _controller.layersToCheckFloorInsideScanner;
        
        if (!_controller.characterController.isGrounded || PlayerOnRamp())
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

    private bool PlayerOnRamp()
    {
        RaycastHit hit;
        if (Physics.Raycast(_controller.transform.position - Vector3.up * (_controller.characterController.height / 2), -_controller.transform.up, out hit, 0.5f, ~LayerMask.GetMask("Player")))
        {
            float surfaceAngle = Vector3.Angle(hit.normal, _controller.transform.up);
            if(surfaceAngle > 10f)
            {
                return true;
            }
        }
        
        return false;
    }

    public override void ExitState()
    {
        if (_controller.jump)
        {
            _controller.verticalSpeed = _controller.jumpSpeed;
            _controller.SetState(new JumpState(_controller));
        }

        CheckFloor(PlayerUtils.GetFloorColliders(_controller, _controller.feetOverlap.position));

        if (_controller.moveObject && _controller.moveObject.canMove && _controller.inputMoveObject && _controller.moveObject.HasFloor())
        {
            _controller.SetState(new PushState(_controller));
        }
    }
    
    private void CheckFloor(Collider[] colliders)
    {
        var list = colliders.ToList();
        if (colliders.Length > 0)
        {
            //At the moment only the moveObject it's checked because it detects the trigger as floor
            //I don't know if it's possible to make it work with layers.
            //As it's the only problem for now, it will be hardcoded.
            
            if(list.All(c => c.name == "Trigger")) _controller.SetState(new JumpState(_controller));
        }
        else _controller.SetState(new JumpState(_controller));
    }
}
