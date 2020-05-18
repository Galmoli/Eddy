using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ThrowHandsEnemyDeathFSM))]

public class ThrowHandsEnemyBlackboard : EnemyBlackboard
{
    public Text statesText;
    public bool respawnable = false;

    [Header("General Stats")]
    public bool armored;
    public float initialHealthPoints;
    public float attackPoints;
    public float rotationSpeed;

    [Header("Enemy Passive")]
    public GameObject initialTransform;
    public bool canWander;
    public float wanderTime;
    public float idleTime;
    public float wanderSpeed;
    public float wanderAcceleration;
    public LayerMask sightObstaclesLayers;

    [Header("Enemy Agressive")]
    public float enemyColliderChaseHeight;
    public float maxVerticalDistance;
    public float detectionDistanceOnSight;
    public float detectionDistanceOffSight;
    public float playerOutOfRangeDistance;
    public float visionAngle;
    public float timeInNotice;
    public float chasingSpeed;
    public float chasingAcceleration;
    public float attackRange;
    public float timeAfterAttacks;
    public Transform attackPoint;

    [Header("Enemy Stun")]
    public float stunnedTime;
    public float stunImpulse;

    [Header("Enemy Stagger")]
    public float staggerImpulse;
    public float staggeredTime;

    [Header("Arrive Steering Variables")]
    public float closeEnoughRadius;
    public float slowDownRadius;

    [Header("Avoidance Steering Variables")]
    public float lookAheadLength;
    public float avoidDistance;
    public float secondaryWhiskerAngle;
    public float secondaryWhiskerRatio;
    public LayerMask avoidLayers;

    private SphereCollider scannerSphereCollider;

    [Header("Wander Steering Variables")]
    public float wanderRate;
    public float wanderRadius;
    public float wanderOffset;

    public override void Start()
    {
        GameManager.Instance.enemySpawnManager.Add(this);

        player = FindObjectOfType<PlayerMovementController>();
        swordScanner = FindObjectOfType<PlayerSwordScanner>();

        scannerSphereCollider = FindObjectOfType<PlayerSwordScanner>().GetComponent<SphereCollider>();

        rb = GetComponent<Rigidbody>();
        ownKS = GetComponent<KinematicState>();

        if (respawnable)
            GameManager.Instance.enemySpawnManager.Add(this);
        else
            GameManager.Instance.nonRespawnableEnemies.Add(gameObject);

        healthPoints = initialHealthPoints;

        stunned = false;
        hit = false;

        if (GetComponent<ArrivePlusAvoid>() != null)
        {
            SetArrivePlusAvoidVariables();
        }

        if (GetComponent<WanderPlusAvoid>() != null)
        {
            SetWanderPlusAvoidVariables();
        }

        initialTransform.transform.parent = null;

    }

    public override void Update()
    {
        statesText.transform.parent.transform.LookAt(Camera.main.transform.position);
    }

    public override void OnDestroy()
    {
        Destroy(initialTransform);
    }

    public override void Hit(int damage, Vector3 hitDirection)
    {
        this.hitDirection = hitDirection;
        hit = true;
        healthPoints -= damage;
    }

    public override void ResetHealth()
    {
        healthPoints = initialHealthPoints;
    }

    void SetArrivePlusAvoidVariables()
    {
        ArrivePlusAvoid arrivePlusVoid = GetComponent<ArrivePlusAvoid>();

        arrivePlusVoid.closeEnoughRadius = closeEnoughRadius;
        arrivePlusVoid.slowDownRadius = slowDownRadius;

        arrivePlusVoid.lookAheadLength = lookAheadLength;
        arrivePlusVoid.avoidDistance = avoidDistance;
        arrivePlusVoid.secondaryWhiskerAngle = secondaryWhiskerAngle;
        arrivePlusVoid.secondaryWhiskerRatio = secondaryWhiskerRatio;
        arrivePlusVoid.avoidLayers = avoidLayers;
        arrivePlusVoid.scanner = scannerSphereCollider;
    }

    void SetWanderPlusAvoidVariables()
    {
        WanderPlusAvoid wanderPlusAvoid = GetComponent<WanderPlusAvoid>();

        wanderPlusAvoid.wanderRate = wanderRate;
        wanderPlusAvoid.wanderRadius = wanderRadius;
        wanderPlusAvoid.wanderOffset = wanderOffset;

        wanderPlusAvoid.lookAheadLength = lookAheadLength;
        wanderPlusAvoid.avoidDistance = avoidDistance;
        wanderPlusAvoid.secondaryWhiskerAngle = secondaryWhiskerAngle;
        wanderPlusAvoid.secondaryWhiskerRatio = secondaryWhiskerRatio;
        wanderPlusAvoid.avoidLayers = avoidLayers;
        wanderPlusAvoid.scanner = scannerSphereCollider;
    }

    public override bool CanBeDamaged()
    {
        return !armored || InScanner();
    }

    private bool InScanner()
    {
        return swordScanner.activeScanner && scannerSphereCollider.bounds.Contains(transform.position);
    }
}
