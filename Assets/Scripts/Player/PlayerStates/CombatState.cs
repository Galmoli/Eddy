using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    private PlayerMovementController _controller;
    private PlayerCombatController _combatController;
    private float _timeToExitState;
    private float _currentTime;
    private bool _exitWhenFinished;

    public CombatState(PlayerMovementController controller, PlayerCombatController combatController, float time)
    {
        _controller = controller;
        _combatController = combatController;
        _timeToExitState = time;
        _exitWhenFinished = true;
    }

    public CombatState(PlayerMovementController controller, PlayerCombatController combatController)
    {
        _controller = controller;
        _combatController = combatController;
        _exitWhenFinished = false;
    }

    public override void Enter()
    {
        Debug.Log("Combat State");
        if(UIHelperController.Instance.actionToComplete == UIHelperController.HelperAction.Attack) UIHelperController.Instance.DisableHelper(1);
    }

    public override void Update()
    {
        _currentTime += Time.deltaTime;
        if (_exitWhenFinished && _currentTime >= _timeToExitState)
        {
            ExitState();
        }

        if (_combatController.GetState().GetType() == typeof(AreaAttackState))
        {
            var vector3D = PlayerUtils.RetargetVector(_controller.movementVector, _controller.cameraTransform, _controller.joystickDeadZone);
            vector3D *= Mathf.Lerp(_controller.minSpeed, _controller.maxSpeed, _controller.movementVector.magnitude);
            vector3D.y = _controller.characterController.velocity.y + Physics.gravity.y * Time.deltaTime;
            _controller.characterController.Move(vector3D * Time.deltaTime);
        }

        if (_combatController.target && _combatController.target.healthPoints > 0 && GetAngleBetweenPlayerAndTarget() > 5)
        {
            Vector3 targetDirection = (_combatController.target.transform.position - _controller.transform.position).normalized;
            targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            Vector3 newDirection = Vector3.RotateTowards(_controller.transform.forward, targetDirection, 2 * Time.deltaTime, 0);
            _controller.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public override void ExitState()
    {
        if (_currentTime < _timeToExitState)
        {
            _exitWhenFinished = true;
            return;
        }
        _controller.SetState(new MoveState(_controller));
    }

    private float GetAngleBetweenPlayerAndTarget()
    {
        var playerTarget = (_combatController.target.transform.position - _controller.transform.position).normalized;
        return Vector3.Angle(playerTarget, _controller.transform.forward);
    }
}
