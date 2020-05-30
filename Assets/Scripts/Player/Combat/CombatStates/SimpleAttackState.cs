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
        _controller.animator.SetBool("isChargingAttack", true);

        if (_controller.simpleAttackCount < _controller.attacksToCombo)
        {
            if (AudioManager.Instance.ValidEvent(_controller.playerSounds.attackSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(_controller.playerSounds.attackSoundPath, _controller.transform);
            }

            _attackObject = _controller.basicAttack;
        }
        else
        {
            if (AudioManager.Instance.ValidEvent(_controller.playerSounds.comboAttackSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(_controller.playerSounds.comboAttackSoundPath, _controller.transform);
            }

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
        if (_controller.swordTrigger.hitObject.GetComponent<EnemyBlackboard>().CanBeDamaged())
        {
            _controller.swordTrigger.hitObject.GetComponent<EnemyBlackboard>().Hit((int)_attackObject.damage, _controller.transform.forward);
            Debug.Log("Enemy damaged: " + _attackObject.damage);

            if (_attackObject == _controller.comboAttack) _controller.simpleAttackCount = 0;
            return;
        }
        Debug.Log("Armor hit");
    }

    public override void ExitState()
    {
        _controller.swordTrigger.DisableTrigger();
        _controller.SetState(new IdleState(_controller));
        _controller.SetMovementControllerToMove();
    }
}
