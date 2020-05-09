using UnityEngine;

public class AreaAttackState : State
{
    private PlayerCombatController _controller;
    private AttackSO _attackObject;
    private float _currentTime;

    public AreaAttackState(PlayerCombatController controller)
    {
        _controller = controller;
        _currentTime = 0;
    }

    public override void Enter()
    {
        Debug.Log("Area Attack");
        _controller.swordTrigger.EnableTrigger();
        _attackObject = _controller.areaAttack;
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
    }

    public override void ExitState()
    {
        _controller.swordTrigger.DisableTrigger();
        _controller.SetState(new IdleState(_controller));
    }
}
