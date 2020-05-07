using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackState : State
{
    private PlayerCombatController _controller;

    public SimpleAttackState(PlayerCombatController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        Debug.Log("Simple Attack");
    }

    public override void Interact()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
