using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]

public class EnemyStunFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL
    }

    private States currentState;

    private EnemyBlackboard blackboard;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
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

                break;
        }
    }

    private void ChangeState(States newState)
    {
        switch (currentState)
        {
            case States.INITIAL:
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
        }

        currentState = newState;
    }
}
