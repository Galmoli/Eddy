using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

    void Start()
    {
        topMinIntensity = topSpotlight.intensity;
        sceneMaxIntensity = sceneLights[0].intensity;
        fireplaceMaxIntensity = fireplace.intensity;
        crowdMinHeight = crowd.localPosition.y;
        crowd.gameObject.SetActive(false);
    }

    // Update is called once per frame
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
        confettiLeft.Play();
        yield return new WaitForSeconds(1);
        confettiLeft.Stop();
    }
    IEnumerator RightConfetti()
    {
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
}
