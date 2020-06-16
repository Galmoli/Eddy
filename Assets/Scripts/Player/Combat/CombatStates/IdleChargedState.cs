using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleChargedState : State
{
    private PlayerCombatController _controller;
    private float _currentTime;

    public IdleChargedState(PlayerCombatController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        Debug.Log("Idle Charged");
        
        _controller.animator.SetTrigger("StartChargeAttack");
        _controller.SetMovementControllerCombatState();
        _controller.AreaAttackChargingSound_1();
        _currentTime = 0;
    }

    public override void Update()
    {
        _currentTime += Time.deltaTime;
    }

    public override void Interact()
    {
        ExitState();
    }

    public override void ExitState()
    {
        _controller.SetState(new AreaAttackState(_controller, Mathf.Clamp(_currentTime/_controller.areaAttack.chargeTime, 0, 1)));
    }
}
