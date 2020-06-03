using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntagonistEndingFSM : MonoBehaviour
{
    public States currentState;

    private AntagonistBlackboard blackboard;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        blackboard = GetComponent<AntagonistBlackboard>();
        rigidbody = GetComponent<Rigidbody>();

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
                ChangeState(States.WAITONPIPE);
                break;
            case States.WAITONPIPE:
                break;
            case States.FALLING:
                timer += Time.deltaTime;

                if (timer >= blackboard.endingPipeTime)
                {
                    ChangeState(States.PERSECUTION);
                }

                break;
            case States.PERSECUTION:
                navMeshAgent.SetDestination(blackboard.player.transform.position);
                break;
            case States.STUNNED:

                ChangeState(States.HEADTOCHOP);

                break;
            case States.HEADTOCHOP:

               

                break;
        }
    }

    public void ChangeState(States newState)
    {
        switch (currentState)
        {
            case States.INITIAL:
                break;
            case States.WAITONPIPE:
                navMeshAgent.enabled = true;
                break;
            case States.FALLING:
                navMeshAgent.enabled = true;
                rigidbody.isKinematic = true;
                timer = 0;
                break;
            case States.PERSECUTION:
                navMeshAgent.speed = blackboard.persecutionSpeed;
                navMeshAgent.enabled = true;
                blackboard.attackCollider.enabled = false;
                break;
            case States.STUNNED:
                navMeshAgent.enabled = true;
                rigidbody.isKinematic = true;
                timer = 0;
                break;
            case States.HEADTOCHOP:

                break;
          
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.WAITONPIPE:

                navMeshAgent.enabled = false;
                transform.position = blackboard.pipePositions[1].transform.position + Vector3.up * 10;

                break;
            case States.FALLING:

                navMeshAgent.enabled = false;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(Vector3.down * blackboard.downPipeImpulse, ForceMode.Impulse);

                break;
            case States.PERSECUTION:

                navMeshAgent.enabled = true;
                navMeshAgent.speed = blackboard.persecutionSpeed;
                blackboard.attackCollider.enabled = true;

                break;
            case States.STUNNED:

                break;

            case States.HEADTOCHOP:

                blackboard.enemyCollider.SetActive(false);
                navMeshAgent.enabled = false;

                transform.rotation = blackboard.destinies[2].transform.rotation;
                transform.position = blackboard.destinies[2].transform.position;

                break;
          
        }

        currentState = newState;
    }

    public enum States
    {
        INITIAL, WAITONPIPE, FALLING, PERSECUTION, STUNNED, HEADTOCHOP
    }
}
