using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovController : MonoBehaviour
{
    private Camera thisCamera;

    public float minFov = 40;
    public float maxFov = 60;
    public float lerpSpd = 0.2f;

    public bool activated = false;
    public bool isGoingToMin = true;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (activated)
        {
            if (isGoingToMin)
            {
                thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, minFov, lerpSpd);
                if (Mathf.Abs(thisCamera.fieldOfView - minFov) < 0.1f) activated = false;
            }
            else
            {
                thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, maxFov, lerpSpd);
                if (Mathf.Abs(thisCamera.fieldOfView - maxFov) < 0.1f) activated = false;
            }


        }
    }
}
