using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMOD.Studio;

public class WaveScene : MonoBehaviour
{
    public Light topSpotlight;
    public float topMaxIntensity;
    private float topMinIntensity;

    public Light[] sceneLights;
    private float sceneMaxIntensity;
    public float sceneMinIntensity;

    public Light fireplace;
    private float fireplaceMaxIntensity;
    public float fireplaceMinIntensity;

    public Transform crowd;
    private float crowdMinHeight;
    public float crowdMaxHeight;

    public bool waveActivated = false;

    public float lerpSpeed = 0.2f;

    public VisualEffect confettiLeft;
    public VisualEffect confettiRight;

    private PlayerSounds sounds;
    private EventInstance crowdSoundEvent;

    void Start()
    {
        topMinIntensity = topSpotlight.intensity;
        sceneMaxIntensity = sceneLights[0].intensity;
        fireplaceMaxIntensity = fireplace.intensity;
        crowdMinHeight = crowd.localPosition.y;
        crowd.gameObject.SetActive(false);

        sounds = FindObjectOfType<PlayerSounds>();
    }

    void Update()
    {
        if (waveActivated)
        {
            crowd.gameObject.SetActive(true);
            topSpotlight.intensity = Mathf.Lerp(topSpotlight.intensity, topMaxIntensity, lerpSpeed);
            foreach (Light sl in sceneLights)
            {
                sl.intensity = Mathf.Lerp(sl.intensity, sceneMinIntensity, lerpSpeed);
            }
            fireplace.intensity = Mathf.Lerp(fireplace.intensity, fireplaceMinIntensity, lerpSpeed);
            crowd.localPosition = Vector3.Lerp(crowd.localPosition, new Vector3(crowd.localPosition.x, crowdMaxHeight, crowd.localPosition.z), lerpSpeed);
        }
        else
        {
            topSpotlight.intensity = Mathf.Lerp(topSpotlight.intensity, topMinIntensity, lerpSpeed);
            foreach (Light sl in sceneLights)
            {
                sl.intensity = Mathf.Lerp(sl.intensity, sceneMaxIntensity, lerpSpeed);
            }
            fireplace.intensity = Mathf.Lerp(fireplace.intensity, fireplaceMaxIntensity, lerpSpeed);
            crowd.localPosition = Vector3.Lerp(crowd.localPosition, new Vector3(crowd.localPosition.x, crowdMinHeight, crowd.localPosition.z), lerpSpeed);
        }
    }

    public void playLeftConfetti()
    {
        StartCoroutine("LeftConfetti");
    }
    public void playRightConfetti()
    {
        StartCoroutine("RightConfetti");     
    }
    public void destroyAllCrowd()
    {
        StartCoroutine("DestroyCrowd");    
    }

    IEnumerator LeftConfetti()
    {
        ConfettiPopSound(confettiLeft.transform);
        confettiLeft.Play();
        yield return new WaitForSeconds(1);
        confettiLeft.Stop();
    }
    IEnumerator RightConfetti()
    {
        ConfettiPopSound(confettiRight.transform);
        confettiRight.Play();
        yield return new WaitForSeconds(1);
        confettiRight.Stop();
    }
    IEnumerator DestroyCrowd()
    {
        yield return new WaitForSeconds(3);
        Destroy(crowd.gameObject);
        Destroy(this);
    }

    public void PlayCrowdSound()
    {
        if (!AudioManager.Instance.isPlaying(crowdSoundEvent))
        {
            if (AudioManager.Instance.ValidEvent(sounds.crowdSoundPath))
            {
                crowdSoundEvent = AudioManager.Instance.PlayEvent(sounds.crowdSoundPath, transform);
            }
        }
    }

    public void StopCrowdSound()
    {
        crowdSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ApplauseSound()
    {
        if (AudioManager.Instance.ValidEvent(sounds.applauseSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(sounds.applauseSoundPath, crowd);
        }
    }

    public void CheersSound()
    {
        if (AudioManager.Instance.ValidEvent(sounds.cheersSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(sounds.cheersSoundPath, sounds.transform);
        }
    }

    public void ConfettiPopSound(Transform t)
    {
        if (AudioManager.Instance.ValidEvent(sounds.confettiPopSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(sounds.confettiPopSoundPath, t);
        }
    }

    public void FinalRoundSound()
    {
        if (AudioManager.Instance.ValidEvent(sounds.finalRoundSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(sounds.finalRoundSoundPath, sounds.transform);
        }
    }
}
