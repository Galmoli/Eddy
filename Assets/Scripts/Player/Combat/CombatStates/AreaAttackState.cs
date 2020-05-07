using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackState : State
{
    private PlayerCombatController _controller;

    public AreaAttackState(PlayerCombatController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        Debug.Log("Area Attack");
    }

    public override void Interact()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
