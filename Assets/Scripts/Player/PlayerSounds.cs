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

    [Header("Sword / Scanner")]
    public string scannerOnSoundPath;
    public string scannerOffSoundPath;
    public string scannerActiveSoundPath;
    public string stabSoundPath;
    public string swordBackSoundPath;
    public string switchSoundPath;
    public string checkpointSoundPath;

    [Header("Others")]
    public string draggableObjectSoundPath;
    public string elevatorSoundPath;

    public void PlayerStep()
    {
        if (AudioManager.Instance.ValidEvent(woodStepSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(woodStepSoundPath, transform);
        }

        //Step on stone? Check surface type

        /*if (AudioManager.Instance.ValidEvent(stoneStepSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(stoneStepSoundPath, transform);
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (AudioManager.Instance.ValidEvent(woodStepSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(woodStepSoundPath, transform);
            }
        }
    }
}
