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
}
