using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctionCaller : MonoBehaviour
{
    public PlayerMovementController movementController;
    public PlayerCombatController combatController;
    public PlayerSwordScanner swordScanner;
    public ThrowHandsEnemyAggressiveFSM aggressiveEnemy;

    private void Start()
    {
        movementController = FindObjectOfType<PlayerMovementController>();
        combatController = FindObjectOfType<PlayerCombatController>();
        swordScanner = FindObjectOfType<PlayerSwordScanner>();
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

    public void EnemyAttack()
    {
        aggressiveEnemy.Attack();
    }


    #region Sounds
    public void StepSound()
    {
        movementController.StepSound();
    }
    
    public void SimpleAttackSound()
    {
        combatController.SimpleAttackSound();
    }

    public void ComboAttackSound()
    {
        combatController.ComboAttackSound();
    }

    public void AreaAttackChargedSound()
    {
        combatController.AreaAttackChargedSound();
    }
    #endregion
}
