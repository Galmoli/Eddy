using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(ArrivePlusAvoid))]

public class EnemyPassiveFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        IDLE,
        ARRIVE
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private ArrivePlusAvoid arrivePlusAvoid;

    private GameObject wanderingTarget;
    private float timeInIdle;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        arrivePlusAvoid = GetComponent<ArrivePlusAvoid>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        arrivePlusAvoid.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.IDLE);
                break;
            case States.IDLE:
                if(timeInIdle <= 0)
                {
                    ChangeState(States.ARRIVE);
                }
                else
                {
                    timeInIdle -= Time.deltaTime;
                }
                break;
            case States.ARRIVE:
                if (Vector3.Distance(transform.position, arrivePlusAvoid.target.transform.position) <= arrivePlusAvoid.closeEnoughRadius)
                {
                    ChangeState(States.IDLE);
                }
                break;
        }      
    }

    private void ChangeState(States newState)
    {
        switch (currentState)
        {
            case States.INITIAL:
                break;
            case States.IDLE:
                break;
            case States.ARRIVE:
                arrivePlusAvoid.enabled = false;
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.IDLE:
                timeInIdle = blackboard.timeInIdle;
                break;
            case States.ARRIVE:
                arrivePlusAvoid.enabled = true;
                arrivePlusAvoid.target = SearchNewPatrolTarget();
                break;
        }

        currentState = newState;

        blackboard.statesText.text = currentState.ToString();
    }

    GameObject SearchNewPatrolTarget()
    {
        if (wanderingTarget == null)
        {
            wanderingTarget = new GameObject("Enemy Wandering Target");
            wanderingTarget.transform.position = transform.position;
        }
        
        Vector3 vector = Quaternion.Euler(0f, Random.Range(-45, 45), 0f) * transform.forward;

        wanderingTarget.transform.position += vector.normalized * blackboard.wanderRadius;

        return wanderingTarget;

        /*Vector3 randomDirection = Random.insideUnitSphere * blackboard.wanderRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, blackboard.wanderRadius, 1);
        Vector3 finalPosition = hit.position;

        blackboard.agent.SetDestination(finalPosition);*/
    }
}
