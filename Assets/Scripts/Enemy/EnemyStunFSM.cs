using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(EnemyAgressiveFSM))]

public class EnemyStunFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        AGRESSIVE,
        STUNNED
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private EnemyAgressiveFSM enemyAgressiveFSM;

    private float stunnedTime;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        enemyAgressiveFSM = GetComponent<EnemyAgressiveFSM>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        enemyAgressiveFSM.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.AGRESSIVE);
                break;
            case States.AGRESSIVE:
                
                break;
            case States.STUNNED:
                if(stunnedTime <= 0)
                {
                    ChangeState(States.AGRESSIVE);
                }
                else
                {
                    stunnedTime -= Time.deltaTime;
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
            case States.AGRESSIVE:
                enemyAgressiveFSM.enabled = false;
                break;
            case States.STUNNED:
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.AGRESSIVE:
                enemyAgressiveFSM.enabled = true;
                break;
            case States.STUNNED:
                blackboard.agent.isStopped = true;
                stunnedTime = blackboard.stunnedTime;
                break;
        }

        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }
}
