using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : Switchable
{
    public ObjectSpawner objSpawner;

    public override void Start()
    {
        
    }

    public override void SwitchOff()
    {
        
    }

    public override void SwitchOn()
    {
        objSpawner.Spawn();
    }

    public override void Update()
    {
        
    }
}
