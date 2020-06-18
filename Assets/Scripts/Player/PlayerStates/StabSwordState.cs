using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabSwordState : State
{
    private PlayerMovementController _controller;
    private float _currentTime;
    
    public StabSwordState(PlayerMovementController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        if (UIHelperController.Instance.actionsToComplete.Contains(UIHelperController.HelperAction.NailSword)) UIHelperController.Instance.DisableHelper(UIHelperController.HelperAction.NailSword);
    }

    public override void Update()
    {
        if (_currentTime < 0.8f)
        {
            _currentTime += Time.deltaTime;
        }else ExitState();
    }

    public override void ExitState()
    {
        _controller.SetState(new MoveState(_controller));
    }
}
