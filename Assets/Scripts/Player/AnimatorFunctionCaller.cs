using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctionCaller : MonoBehaviour
{
    public PlayerMovementController movementController;
    public PlayerCombatController combatController;
    public PlayerSwordScanner swordScanner;

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

    
    //SOUNDS
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
}
