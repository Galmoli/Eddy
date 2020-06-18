using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{

    private static VibrationManager instance;

    public static VibrationManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<VibrationManager>();
            return instance;
        }
    }

    public float testLeftAmount, testRightAmount, testTime;

    public void Vibrate(Presets preset)
    {
        if (Gamepad.current != null)
        {
            switch (preset)
            {
                case Presets.NORMAL_HIT:
                    StartCoroutine(NormalHitImplementation());
                    break;
                case Presets.HARD_HIT:
                    StartCoroutine(HardHitImplementation());
                    break;
                case Presets.HEARTBEAT:
                    break;
                case Presets.SUCCESS:
                    StartCoroutine(SuccessImplementation());
                    break;
                case Presets.DESTRUCTION:
                    StartCoroutine(DestructionImplementation());
                    break;
                case Presets.TEST:
                    StartCoroutine(TestImplementation());
                    break;
            }
        }
    }

    IEnumerator NormalHitImplementation()
    {
        Gamepad.current.SetMotorSpeeds(0.1f, 0.8f);
        yield return new WaitForSeconds(0.2f);
        Gamepad.current.SetMotorSpeeds(0,0);
    }

    IEnumerator HardHitImplementation()
    {
        Gamepad.current.SetMotorSpeeds(0.3f, 0.65f);
        yield return new WaitForSeconds(0.4f);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    IEnumerator SuccessImplementation()
    {
        Gamepad.current.SetMotorSpeeds(0.6f, 0.1f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    IEnumerator DestructionImplementation()
    {
        Gamepad.current.SetMotorSpeeds(0.9f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    IEnumerator TestImplementation()
    {
        Gamepad.current.SetMotorSpeeds(testLeftAmount, testRightAmount);
        yield return new WaitForSeconds(testTime);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    public enum Presets
    {
        NORMAL_HIT, HARD_HIT, HEARTBEAT, SUCCESS, DESTRUCTION, TEST
    }
}
