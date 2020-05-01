using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyBlackboard))]

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
    private NavMeshAgent agent;

    private float timeInIdle;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.IDLE);
                break;
            case States.IDLE:
                if(timeInIdle < 0)
                {
                    ChangeState(States.ARRIVE);
                }
                else
                {
                    timeInIdle -= Time.deltaTime;
                }
                break;
            case States.ARRIVE:
                if (!agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete)
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
                SearchNewPatrolTarget();
                break;
        }

        currentState = newState;
    }

    void SearchNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * blackboard.wanderRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, blackboard.wanderRadius, 1);
        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);
    }
}
