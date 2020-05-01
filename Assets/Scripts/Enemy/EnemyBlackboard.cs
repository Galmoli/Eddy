using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBlackboard : MonoBehaviour
{
    public GameObject attack;
    public Text statesText;
    
    [Header("General Stats")]
    public float healthPoints;
    public float attackPoints;
    public float movementSpeed;
    public float rotationSpeed;

    [Header("Enemy Passive")]
    public float wanderRadius;
    public float timeInIdle;

    [Header("Enemy Agressive")]
    public float detectionDistanceOnSight;
    public float detectionDistanceOffSight;
    public float timeInNotice;
    public float attackDistance;
    public float minTimeBetweenAttacks;

    [Header("Other Variables")]
    public float stunnedTime;
    public float staggeredTime;

    [HideInInspector] public bool hit;
    [HideInInspector] public bool stunned;

    [HideInInspector] public PlayerMovementController player;
    [HideInInspector] public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        player = FindObjectOfType<PlayerMovementController>();

        stunned = false;
        hit = false;
    }

    private void Update()
    {
        statesText.transform.parent.transform.LookAt(Camera.main.transform.position);
    }
}
