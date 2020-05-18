using Steerings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrowHandsEnemyPassiveFSM))]
[RequireComponent(typeof(ArrivePlusAvoid))]

public class ThrowHandsEnemyAggressiveFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        ENEMY_PASSIVE,
        NOTICE,
        CHASE,
        ATTACK
    }

    private States currentState;

    private ThrowHandsEnemyBlackboard blackboard;
    private ThrowHandsEnemyPassiveFSM enemyPassiveFSM;
    private WanderPlusAvoid wanderPlusAvoid;
    private ArrivePlusAvoid arrivePlusAvoid;

    private float timer;
    private float timeAfterAttacks;

    private void Start()
    {
        blackboard = GetComponent<ThrowHandsEnemyBlackboard>();
        enemyPassiveFSM = GetComponent<ThrowHandsEnemyPassiveFSM>();
        wanderPlusAvoid = GetComponent<WanderPlusAvoid>();
        arrivePlusAvoid = GetComponent<ArrivePlusAvoid>();


    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        wanderPlusAvoid.enabled = false;
        enemyPassiveFSM.enabled = false;

        timer = 0;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.ENEMY_PASSIVE);
                break;
            case States.ENEMY_PASSIVE:

                RaycastHit hit;
                if(Physics.Raycast(transform.position, blackboard.player.transform.position - transform.position, out hit, blackboard.detectionDistanceOnSight, blackboard.sightObstaclesLayers))
                {                   
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        if (Mathf.Acos(Vector3.Dot((blackboard.player.transform.position - transform.position).normalized, Vector3.forward)) <= blackboard.visionAngle)
                        {
                            if(Math.Abs(blackboard.player.transform.position.y - transform.position.y) < blackboard.maxVerticalDistance)
                            {
                                ChangeState(States.NOTICE);
                                break;
                            }
                        }
                    }
                }

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.detectionDistanceOffSight)
                {
                    if (Math.Abs(blackboard.player.transform.position.y - transform.position.y) < blackboard.maxVerticalDistance)
                    {
                        ChangeState(States.NOTICE);
                    }                     
                }
                break;
            case States.NOTICE:
                
                if (timer >= blackboard.timeInNotice)
                {
                    ChangeState(States.CHASE);
                    break;
                }

                LookAtPlayer();
                timer += Time.deltaTime;
                
                break;
            case States.CHASE:

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) >= blackboard.playerOutOfRangeDistance || Math.Abs(blackboard.player.transform.position.y - transform.position.y) >= blackboard.maxVerticalDistance)
                {
                    ChangeState(States.ENEMY_PASSIVE);
                    break;
                }

                if(Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.attackRange)
                {
                    ChangeState(States.ATTACK);
                }

                break;
            case States.ATTACK:
                timeAfterAttacks -= Time.deltaTime;

                if(timeAfterAttacks <= 0)
                {
                    if (Vector3.Distance(transform.position, blackboard.player.transform.position) > blackboard.attackRange)
                    {
                        ChangeState(States.CHASE);
                        break;
                    }

                    ChangeState(States.ATTACK);
                    break;
                }       

                LookAtPlayer();

                break;
        }
    }

    private void ChangeState(States newState)
    {
        switch (currentState)
        {
            case States.INITIAL:
                break;
            case States.ENEMY_PASSIVE:
                enemyPassiveFSM.enabled = false;
                break;
            case States.NOTICE:
                break;
            case States.CHASE:
                arrivePlusAvoid.enabled = false;
                break;
            case States.ATTACK:
                break;

        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.ENEMY_PASSIVE:
                enemyPassiveFSM.enabled = true;
                break;
            case States.NOTICE:

                break;
            case States.CHASE:
                blackboard.ownKS.maxSpeed = blackboard.chasingSpeed;
                blackboard.ownKS.maxAcceleration = blackboard.chasingAcceleration;
                arrivePlusAvoid.enabled = true;
                arrivePlusAvoid.target = blackboard.player.gameObject;
                break;
            case States.ATTACK:
                Attack();
                timeAfterAttacks = blackboard.timeAfterAttacks;
                break;

        }
        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }

    private void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(blackboard.attackPoint.position, blackboard.damageZoneRadius);
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag.Equals("Player"))
            {
                blackboard.player.GetComponent<PlayerController>().Hit((int)blackboard.attackPoints);
                break;
            }
        }
    }

    private void LookAtPlayer()
    {
        transform.LookAt(blackboard.player.transform);

        Vector3 playerEulerAngles = transform.rotation.eulerAngles;
        playerEulerAngles.x = 0;
        playerEulerAngles.z = 0;

        transform.rotation = Quaternion.Euler(playerEulerAngles);
        blackboard.ownKS.orientation = playerEulerAngles.y;
    }
}
