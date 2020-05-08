using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steerings : MonoBehaviour
{
    [HideInInspector] public Vector3 steeringForce;
    
    [Header("Priority")]
    [Range(1, 100)] public  uint combinationScale;

    public abstract void Update();
}
