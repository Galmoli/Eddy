using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(EnemyHitFSM))]

public class EnemyDeathFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        HIT,
        DEATH
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private EnemyHitFSM enemyHitFSM;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        enemyHitFSM = GetComponent<EnemyHitFSM>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        enemyHitFSM.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.HIT);
                break;
            case States.HIT:
                if(blackboard.healthPoints <= 0)
                {
                    ChangeState(States.DEATH);
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
            case States.HIT:
                enemyHitFSM.enabled = false;
                break;
            case States.DEATH:
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.HIT:
                enemyHitFSM.enabled = true;
                break;
            case States.DEATH:
                blackboard.agent.isStopped = true;
                break;
        }

        currentState = newState;
    }
}
