using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KinematicState))]

public abstract class EnemyBlackboard : MonoBehaviour
{
    public abstract void Start();
    public abstract void Update();
    public abstract void OnDestroy();
    public abstract void OnCollisionStay(Collision other);

    //Functions
    public abstract void ResetHealth();
    public abstract void Hit(int damage, Vector3 hitDirection);
    public abstract bool CanBeDamaged();
    public abstract void EnemyInVolume(bool scannerOn);
    public abstract void Death();

    //Variables
    [HideInInspector] public PlayerMovementController player;
    [HideInInspector] public PlayerSwordScanner swordScanner;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public KinematicState ownKS;

    [HideInInspector] public float healthPoints;
    [HideInInspector] public bool stunned;
    [HideInInspector] public bool hit;
    [HideInInspector] public Vector3 hitDirection;
}
