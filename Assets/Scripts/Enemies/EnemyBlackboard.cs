using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBlackboard : MonoBehaviour
{
    public abstract void Start();
    public abstract void Update();

    //Functions
    public abstract void ResetHealth();
    public abstract void Hit(int damage, Vector3 hitDirection);
    public abstract bool CanBeDamaged();

    //Variables
    [HideInInspector] public bool stunned;
    [HideInInspector] public float healthPoints;
}
