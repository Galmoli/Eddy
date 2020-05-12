using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBlackboard : MonoBehaviour
{
    public GameObject attack;
    public Text statesText;
    
    [Header("General Stats")]
    public float healthPoints;
    public float attackPoints;
    public float rotationSpeed;

    [Header("Enemy Passive")]
    public float wanderSpeed;
    public LayerMask sightObstaclesLayers;

    [Header("Enemy Agressive")]
    public float detectionDistanceOnSight;
    public float detectionDistanceOffSight;
    public float timeInNotice;
    public float chasingSpeed;
    public float attackDistance;
    public float minTimeBetweenAttacks;

    [Header("Other Variables")]
    public float stunnedTime;
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

    private SphereCollider scanner;

    [Header("Wander Steering Variables")]
    public float wanderRate;
    public float wanderRadius;
    public float wanderOffset;

    [HideInInspector] public bool hit;
    [HideInInspector] public bool stunned;

    [HideInInspector] public PlayerMovementController player;
    
    

    void Start()
    {
        player = FindObjectOfType<PlayerMovementController>();

        scanner = FindObjectOfType<PlayerSwordScanner>().GetComponent<SphereCollider>();

        stunned = false;
        hit = false;

        if(GetComponent<ArrivePlusAvoid>() != null)
        {
            SetArrivePlusAvoidVariables();
        }

        if (GetComponent<WanderPlusAvoid>() != null)
        {
            SetWanderPlusAvoidVariables();
        }
    }

    private void Update()
    {
        statesText.transform.parent.transform.LookAt(Camera.main.transform.position);
    }

    public void Hit(int damage)
    {
        hit = true;
        healthPoints -= damage;
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
        arrivePlusVoid.scanner = scanner;
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
        wanderPlusAvoid.scanner = scanner;
    }
}
