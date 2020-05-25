using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargingEnemyHitFSM))]

public class ChargingEnemyDeathFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        HIT,
        DEATH
    }

    private States currentState;

    private ChargingEnemyBlackboard blackboard;
    private ChargingEnemyHitFSM enemyHitFSM;

    private void Start()
    {
        blackboard = GetComponent<ChargingEnemyBlackboard>();
        enemyHitFSM = GetComponent<ChargingEnemyHitFSM>();
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
                blackboard.Death();
                break;
        }

        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }
}
