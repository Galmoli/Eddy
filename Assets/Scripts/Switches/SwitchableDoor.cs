using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableDoor : Switchable
{
    public override void Start()
    {

    }

    public override void Update()
    {
        
    }

    public override void SwitchOn()
    {
        Debug.Log("SWITCH ON");
    }

    public override void SwitchOff()
    {
        Debug.Log("SWITCH OFF");
    }
}
