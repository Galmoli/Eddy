using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(EnemyStunFSM))]

public class EnemyHitFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        STUNNED,
        STAGGERED
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private EnemyStunFSM enemyStunFSM;

    private float straggeredTime;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        enemyStunFSM = GetComponent<EnemyStunFSM>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        enemyStunFSM.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.STUNNED);
                break;
            case States.STUNNED:
                if (blackboard.hit)
                {
                    ChangeState(States.STAGGERED);
                }
                break;
            case States.STAGGERED:
                if(straggeredTime <= 0)
                {
                    ChangeState(States.STUNNED);
                }
                else
                {
                    straggeredTime -= Time.deltaTime;
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
            case States.STUNNED:
                enemyStunFSM.enabled = false;
                blackboard.hit = false;
                break;
            case States.STAGGERED:
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.STUNNED:
                enemyStunFSM.enabled = true;
                break;
            case States.STAGGERED:
                straggeredTime = blackboard.staggeredTime;
                break;
        }

        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }
}
