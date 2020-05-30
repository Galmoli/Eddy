using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Basics")]
    public string stoneStepSoundPath;
    public string woodStepSoundPath;
    public string damageReceivedSoundPath;
    public string deathSoundPath;
    public string jumpSoundPath;
    public string landSoundPath;

    [Header("Combat")]
    public string attackSoundPath;
    public string comboAttackSoundPath;
    public string areaAttackSoundPath;
    public string areaAttackChargingSoundPath;
    public string areaAttackChargedSoundPath;
    public string enemyHitSoundPath;
    public string enemyArmoredHitSoundPath;
    public string woodObjectHitSoundPath;
    public string metalObjectHitSoundPath;

    [Header("Others")]
    public string draggableObjectSoundPath;
    public string elevatorSoundPath;
}
