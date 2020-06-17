using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShadowEnemies : MonoBehaviour
{
    public Transform target;
    private Vector3 iniTarget;
    public float timer;
    public float lerpSpeed = 0.1f;
    private bool hasBeenActivated;

    private PlayerSounds sounds;

    public enum CreepySounds
    {
        LAUGH_1,
        LAUGH_2,
        CRY
    }

    [Header("Sound")]
    public bool playSound;
    public CreepySounds chosenSound;

    void Start()
    {
        iniTarget = transform.position;

        sounds = FindObjectOfType<PlayerSounds>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenActivated)
        {
            StartCoroutine("ActivateShadows");

            if (playSound)
            {
                CreepySound();
            }

        }
    }

    IEnumerator ActivateShadows()
    {
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(timer);
        while (Vector3.Distance(transform.position, iniTarget) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, iniTarget, lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private void CreepySound()
    {
        switch (chosenSound)
        {
            case CreepySounds.LAUGH_1:
                if (AudioManager.Instance.ValidEvent(sounds.creepyShadowLaugh_1))
                {
                    AudioManager.Instance.PlayEvent(sounds.creepyShadowLaugh_1, sounds.transform);
                }
                break;
            case CreepySounds.LAUGH_2:
                if (AudioManager.Instance.ValidEvent(sounds.creepyShadowLaugh_2))
                {
                    AudioManager.Instance.PlayEvent(sounds.creepyShadowLaugh_2, sounds.transform);
                }
                break;
            case CreepySounds.CRY:
                if (AudioManager.Instance.ValidEvent(sounds.creepyShadowCry))
                {
                    AudioManager.Instance.PlayEvent(sounds.creepyShadowCry, sounds.transform);
                }
                break;
        }
    }
}
