﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctionCaller : MonoBehaviour
{
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
}
