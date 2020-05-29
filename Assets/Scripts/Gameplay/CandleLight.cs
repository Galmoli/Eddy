using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLight : MonoBehaviour
{
    private Light candleLight;
    public float intensity;
    private float intensityGoal;
    public float intensityVariation;
    public float speed;
    private float iniSpeed;
    public float speedVariation;

    void Start()
    {
        candleLight = GetComponent<Light>();
        Debug.Log("candle" + candleLight.intensity.ToString()); 
        iniSpeed = speed;

        intensityGoal = Random.Range(intensity - intensityVariation, intensity + intensityVariation);
        speed = Random.Range(iniSpeed - speedVariation, iniSpeed - speedVariation);
    }

    void Update()
    {
        candleLight.intensity = Mathf.Lerp(candleLight.intensity, intensityGoal, speed);

        if (intensityGoal - candleLight.intensity < 0.2f)
        {
            intensityGoal = Random.Range(intensity - intensityVariation, intensity + intensityVariation);
            speed = Random.Range(iniSpeed - speedVariation, iniSpeed + speedVariation);
        }
    }
}
