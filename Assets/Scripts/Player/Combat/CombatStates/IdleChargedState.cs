using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleChargedState : State
{
    private PlayerCombatController _controller;

    public IdleChargedState(PlayerCombatController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        Debug.Log("Idle Charged");

        _controller.animator.SetTrigger("StartChargeAttack");

        if (AudioManager.Instance.ValidEvent(_controller.playerSounds.areaAttackChargingSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(_controller.playerSounds.areaAttackChargingSoundPath, _controller.transform);
        }
    }

    public override void Interact()
    {
        ExitState();
    }

    public override void ExitState()
    {
        if (AudioManager.Instance.ValidEvent(_controller.playerSounds.areaAttackSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(_controller.playerSounds.areaAttackSoundPath, _controller.transform);
        }

        _controller.SetState(new AreaAttackState(_controller, 1));
    }
}
