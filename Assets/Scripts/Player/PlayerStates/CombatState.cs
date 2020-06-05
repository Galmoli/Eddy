using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    private PlayerMovementController _controller;
    private PlayerCombatController _combatController;
    private PlayerSwordScanner _scannerSword;
    private float _timeToExitState;
    private float _currentTime;
    private bool _exitWhenFinished;

    public CombatState(PlayerMovementController controller, PlayerCombatController combatController, float time)
    {
        _controller = controller;
        _combatController = combatController;
        _scannerSword = controller.scannerSword;
        _timeToExitState = time;
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
            _controller.characterController.Move(vector3D * Time.deltaTime);
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
}
