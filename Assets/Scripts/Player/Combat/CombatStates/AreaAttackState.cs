using UnityEngine;

public class AreaAttackState : State
{
    private PlayerCombatController _controller;
    private AttackSO _attackObject;
    private float _currentTime;
    private float _damageMultiplier;

    public AreaAttackState(PlayerCombatController controller, float damageMultiplier)
    {
        _controller = controller;
        _damageMultiplier = damageMultiplier;
        _currentTime = 0;
    }

    public override void Enter()
    {
        //_controller.swordTrigger.EnableTrigger();
        _attackObject = _controller.areaAttack;

        _controller.SetMovementControllerCombatState(_attackObject.attackTime);
        _controller.animator.SetTrigger("AreaAttack");

        _controller.StopAreaAttackChargingSound_1();
        _controller.AreaAttackSound();

        if (UIHelperController.Instance.actionsToComplete.Contains(UIHelperController.HelperAction.SpinAttack)) UIHelperController.Instance.DisableHelper(1, UIHelperController.HelperAction.SpinAttack);
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

        _controller.StopAreaAttackChargingSound_2();
    }

    public override void Interact()
    {
        if (_controller.swordTrigger.hitObject.tag == "Enemy")
        {
            if (_controller.swordTrigger.hitObject.GetComponent<EnemyBlackboard>().CanBeDamaged())
            {
                _controller.swordTrigger.hitObject.GetComponent<EnemyBlackboard>().Hit(_attackObject.damage * _damageMultiplier, _controller.transform.forward);

                Debug.Log("Enemy damaged: " + _attackObject.damage * _damageMultiplier);

                if (_damageMultiplier == 1) _controller.swordTrigger.hitObject.GetComponent<EnemyBlackboard>().stunned = true;

                VibrationManager.Instance.Vibrate(VibrationManager.Presets.NORMAL_HIT);

                _controller.AnimStop();
                _controller.EnemyHitSound();
                return;
            }

            _controller.ArmoredHitSound();
            return;
        }

        if (_controller.swordTrigger.hitObject.tag == "Wood")
        {
            _controller.WoodObjectHitSound();
            return;
        }

        if (_controller.swordTrigger.hitObject.tag == "Metal")
        {
            _controller.MetalObjectHitSound();
            return;
        }
    }

    public override void ExitState()
    {
        _controller.simpleAttackCount = 0;
        _controller.swordTrigger.DisableTrigger();
        _controller.SetMovementControllerToMove();
        _controller.SetState(new IdleState(_controller));
    }
}
