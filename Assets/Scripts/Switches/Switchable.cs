using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switchable : MonoBehaviour
{
    public abstract void Start();
    public abstract void Update();
    public abstract void SwitchOn();
    public abstract void SwitchOff();
}
