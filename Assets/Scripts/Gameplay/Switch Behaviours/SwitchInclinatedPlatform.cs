using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInclinatedPlatform : Switchable
{
    bool active;
    Vector3 startRotation;
    float perc = 0;

    public GameObject platform;
    public float angleZ;

    public override void Start()
    {
        startRotation = platform.transform.eulerAngles;
    }

    public override void SwitchOff()
    {

    }

    public override void SwitchOn()
    {
        active = true;
    }

    public override void Update()
    {
        if (active)
        {

            if (perc <= 1.0f)
            {
                perc += Time.deltaTime * 3;
                platform.transform.eulerAngles = Vector3.Lerp(startRotation, startRotation + new Vector3(0, 0, angleZ), perc);
            }

        }
    }

}
