using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool waveActivated = false;

    public float lerpSpeed = 0.2f;

    void Start()
    {
        topMinIntensity = topSpotlight.intensity;
        sceneMaxIntensity = sceneLights[0].intensity;
        fireplaceMaxIntensity = fireplace.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveActivated)
        {
            topSpotlight.intensity = Mathf.Lerp(topSpotlight.intensity, topMaxIntensity, lerpSpeed);
            foreach (Light sl in sceneLights)
            {
                sl.intensity = Mathf.Lerp(sl.intensity, sceneMinIntensity, lerpSpeed);
            }
            fireplace.intensity = Mathf.Lerp(fireplace.intensity, fireplaceMinIntensity, lerpSpeed);
        }
        else
        {
            topSpotlight.intensity = Mathf.Lerp(topSpotlight.intensity, topMinIntensity, lerpSpeed);
            foreach (Light sl in sceneLights)
            {
                sl.intensity = Mathf.Lerp(sl.intensity, sceneMaxIntensity, lerpSpeed);
            }
            fireplace.intensity = Mathf.Lerp(fireplace.intensity, fireplaceMaxIntensity, lerpSpeed);
        }
    }
}
