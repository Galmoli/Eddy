using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntagonistPersecutionFSM : MonoBehaviour
{
    public States currentState;

    private AntagonistBlackboard blackboard;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        blackboard = GetComponent<AntagonistBlackboard>();

        currentState = States.INITIAL;
    }

    public void Reset()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.GUARDING);
                break;
            case States.GUARDING:

                Vector3 guardingPosition;

                guardingPosition = transform.position - blackboard.player.transform.position;
                guardingPosition = Vector3.ProjectOnPlane(guardingPosition, blackboard.guardingCol.transform.forward);
                guardingPosition = blackboard.player.transform.position + guardingPosition;


                navMeshAgent.SetDestination(blackboard.player.transform.position);

                break;
            case States.FOLLOWING:
                break;
            case States.PERSECUTION:
                break;
            case States.STUNNED:
                break;
            case States.APPEARING_DOWN:
                break;
            case States.APPEARING_ROTATING:
                break;
        }
    }

    public void ChangeState(States newState)
    {
        switch (currentState)
        {
            case States.INITIAL:
                break;
            case States.GUARDING:
                break;
            case States.FOLLOWING:
                break;
            case States.PERSECUTION:
                break;
            case States.STUNNED:
                break;
            case States.APPEARING_DOWN:
                break;
            case States.APPEARING_ROTATING:
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.GUARDING:
                navMeshAgent.speed = blackboard.guardingSpeed;
                break;
            case States.FOLLOWING:
                break;
            case States.PERSECUTION:
                blackboard.attackCollider.enabled = true;
                blackboard.firstObstacle.SetActive(false);
                break;
            case States.STUNNED:
                break;
            case States.APPEARING_DOWN:
                break;
            case States.APPEARING_ROTATING:
                break;
        }

        currentState = newState;
    }

    public enum States
    {
        INITIAL, GUARDING, FOLLOWING, PERSECUTION, STUNNED, APPEARING_DOWN, APPEARING_ROTATING
    }
}


