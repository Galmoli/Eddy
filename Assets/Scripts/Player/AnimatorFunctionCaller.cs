using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctionCaller : MonoBehaviour
{
    public PlayerCombatController combatController;

    public void EnableTrigger()
    {
        combatController.swordTrigger.EnableTrigger();
    }
}
