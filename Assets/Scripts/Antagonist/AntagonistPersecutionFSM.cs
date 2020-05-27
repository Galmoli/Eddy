using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntagonistPersecutionFSM : MonoBehaviour
{
    public States currentState;

    private AntagonistBlackboard blackboard;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    bool dialogueShowed;

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

                navMeshAgent.SetDestination(blackboard.player.transform.position);

                break;
            case States.CHARGING:
               
                navMeshAgent.SetDestination(blackboard.firstDestiny.transform.position);

                break;
            case States.STUNNED:
                timer += Time.deltaTime;

                if (timer >= blackboard.stunnedTime)
                {
                    ChangeState(States.CHARGING);
                }

                break;
            case States.APPEARING_DOWN:

                timer += Time.deltaTime;

                if (timer >= blackboard.pipeTime/3 && !dialogueShowed)
                {
                    InGameDialogue.Instance.EnableDialogue("PopUp_14");
                    dialogueShowed = true;
                }

                if (timer >= blackboard.pipeTime)
                {
                    ChangeState(States.PERSECUTION);
                }
                break;
            case States.APPEARING_ROTATING:
                break;
            case States.WAITINGFORPIPE:
                break;
            case States.WAITINGFORPLAYER:
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
                navMeshAgent.enabled = true;
                rigidbody.isKinematic = true;
                timer = 0;
                break;
            case States.CHARGING:
                break;
            case States.APPEARING_DOWN:
                navMeshAgent.enabled = true;
                rigidbody.isKinematic = true;
                timer = 0;
                dialogueShowed = false;
                break;
            case States.APPEARING_ROTATING:
                break;
            case States.WAITINGFORPIPE:
                navMeshAgent.enabled = true;
                break;
            case States.WAITINGFORPLAYER:
                navMeshAgent.enabled = true;
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
            case States.WAITINGFORPIPE:
                navMeshAgent.enabled = false;
                transform.position = blackboard.pipe.transform.position + Vector3.up * 4;
                break;
            case States.PERSECUTION:
                navMeshAgent.speed = blackboard.persecutionSpeed;
                blackboard.attackCollider.enabled = true;
                blackboard.firstObstacle.SetActive(false);
                blackboard.secondObstacle.SetActive(false);
                break;
            case States.CHARGING:
                navMeshAgent.speed = blackboard.persecutionSpeed;
                blackboard.attackCollider.enabled = true;
                blackboard.firstObstacle.SetActive(false);
                break;
            case States.STUNNED:
                navMeshAgent.enabled = false;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(-transform.forward * blackboard.obstacleImpactForce, ForceMode.Impulse);
                blackboard.attackCollider.enabled = false;
                break;
            case States.APPEARING_DOWN:                
                navMeshAgent.enabled = false;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(Vector3.down * blackboard.downPipeImpulse, ForceMode.Impulse);
                blackboard.attackCollider.enabled = false;
                blackboard.thirdObstacle.SetActive(true);
                blackboard.endCol.SetActive(true);
                break;
            case States.APPEARING_ROTATING:
                break;
            case States.WAITINGFORPLAYER:
                navMeshAgent.enabled = false;
                transform.position = blackboard.secondDestiny.transform.position;
                blackboard.firstObstacle.SetActive(true);
                break;
        }

        currentState = newState;
    }

    public enum States
    {
        INITIAL, GUARDING, FOLLOWING, PERSECUTION, CHARGING, STUNNED, WAITINGFORPIPE, APPEARING_DOWN, APPEARING_ROTATING, WAITINGFORPLAYER
    }
}


