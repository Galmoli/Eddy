using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private PlayerCombatController _controller;

    public IdleState(PlayerCombatController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        
    }

    public override void Interact()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
