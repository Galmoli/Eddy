using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackState : State
{
    private PlayerCombatController _controller;
    private AttackSO _attackObject;
    private float _currentTime;

    public SimpleAttackState(PlayerCombatController controller)
    {
        _controller = controller;
        _currentTime = 0;
    }

    public override void Enter()
    {
        Debug.Log("Simple Attack");
        
        //_controller.swordTrigger.EnableTrigger();
        _controller.simpleAttackCount++;

        switch (_controller.simpleAttackCount)
        {
            case 1:
                _controller.animator.SetTrigger("Attack1");
                break;
            case 2:
                _controller.animator.SetTrigger("Attack2");
                break;
            case 3:
                _controller.animator.SetTrigger("Attack3");
                break;
        }

        if (_controller.simpleAttackCount < _controller.attacksToCombo)
        {
            _attackObject = _controller.basicAttack;
        }
        else
        {
            _attackObject = _controller.comboAttack;
            _controller.simpleAttackCount = 0;
        }
        
        _controller.SetMovementControllerCombatState(_attackObject.attackTime);
    }

    public override void Update()
    {
        if (_currentTime < _attackObject.attackTime)
        {
            _currentTime += Time.deltaTime;
        }
        else
        {
            ExitState();
        }
    }

    public override void Interact()
    {
        _controller.swordTrigger.hitObject.GetComponent<EnemyBlackboard>().healthPoints -= _attackObject.damage;
        Debug.Log("Enemy damaged: " + _attackObject.damage);
        if (_attackObject == _controller.comboAttack) _controller.simpleAttackCount = 0;
    }

    public override void ExitState()
    {
        _controller.swordTrigger.DisableTrigger();
    }
}
