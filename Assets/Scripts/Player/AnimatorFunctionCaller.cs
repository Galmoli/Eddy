using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctionCaller : MonoBehaviour
{
    //Player
    private PlayerMovementController movementController;
    private PlayerCombatController combatController;
    private PlayerSwordScanner swordScanner;

    //Enemies
    private EnemyBlackboard enemyBlackboard;

    //Throw Hands Enemy
    private ThrowHandsEnemyAggressiveFSM throwHandsEnemy;

    private void Start()
    {
        movementController = FindObjectOfType<PlayerMovementController>();
        combatController = FindObjectOfType<PlayerCombatController>();
        swordScanner = FindObjectOfType<PlayerSwordScanner>();

        enemyBlackboard = GetComponentInParent<EnemyBlackboard>();

        throwHandsEnemy = GetComponentInParent<ThrowHandsEnemyAggressiveFSM>();
    }

    public void EnableTrigger()
    {
        combatController.swordTrigger.EnableTrigger();
    }

    public void FinishStab()
    {
        swordScanner.FinishStab();
    }

    public void StandEdge()
    {
        movementController.StandEdge();
    }

    public void EnemyStep()
    {
        enemyBlackboard.StepSound();
    }

    public void EnemyAttack()
    {
        throwHandsEnemy.Attack();
    }


    #region Sounds
    public void StepSound()
    {
        movementController.StepSound();
    }
    
    public void SimpleAttackSound_1()
    {
        combatController.SimpleAttackSound_1();
    }

    public void SimpleAttackSound_2()
    {
        combatController.SimpleAttackSound_2();
    }

    public void ComboAttackSound()
    {
        combatController.ComboAttackSound();
    }

    public void AreaAttackChargedSound()
    {
        combatController.StopAreaAttackChargingSound_1();
        combatController.AreaAttackChargedSound();
        combatController.AreaAttackChargingSound_2();
    }
    #endregion
}
