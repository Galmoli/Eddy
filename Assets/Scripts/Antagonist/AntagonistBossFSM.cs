using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntagonistBossFSM : MonoBehaviour
{
    public States currentState;
    public BossController bossController;

    private AntagonistBlackboard blackboard;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    bool dialogueShowed;

    int currentDestiny;
    Vector3 currentPipePos;

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

                if (timer >= blackboard.pipeTime)
                {
                    ChangeState(States.PERSECUTION);
                }

                break;
            case States.PERSECUTION:
                navMeshAgent.SetDestination(blackboard.player.transform.position);
                break;
            case States.STUNNED:
                timer += Time.deltaTime;

                if (timer >= blackboard.stunnedTime)
                {
                    ChangeState(States.BACKTOPIPE);
                }
                break;
            case States.BACKTOPIPE:

                if ((navMeshAgent.destination - transform.position).magnitude <= 2.54f)
                {
                    ChangeState(States.CLIMBING);
                }

                break;
            case States.CLIMBING:

                transform.position = Vector3.Lerp(transform.position, currentPipePos + Vector3.up * 10, 5 * Time.deltaTime);

                if ((currentPipePos + Vector3.up * 10 -  transform.position).magnitude <= 0.1f)
                {
                    ChangeState(States.WAITONPIPE);
                    bossController.StartWaveCorutine();
                }
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
                navMeshAgent.enabled = true;
                blackboard.attackCollider.enabled = false;
                break;
            case States.STUNNED:
                navMeshAgent.enabled = true;
                rigidbody.isKinematic = true;
                timer = 0;
                break;
            case States.BACKTOPIPE:
                break;
            case States.CLIMBING:
                navMeshAgent.enabled = true;
                rigidbody.isKinematic = true;
                timer = 0;
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.WAITONPIPE:

                navMeshAgent.enabled = false;
                int pipe = Random.Range(0, blackboard.pipePositions.Length);
                transform.position = blackboard.pipePositions[pipe].transform.position + Vector3.up * 10;
                currentPipePos = blackboard.pipePositions[pipe].transform.position;

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

                navMeshAgent.enabled = false;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(-transform.forward * blackboard.obstacleImpactForce, ForceMode.Impulse);
                blackboard.attackCollider.enabled = false;

                break;
            case States.BACKTOPIPE:

                navMeshAgent.SetDestination(currentPipePos);

                break;
            case States.CLIMBING:

                navMeshAgent.enabled = false;

                break;
        }

        currentState = newState;
    }

    public enum States
    {
        INITIAL, WAITONPIPE, FALLING, PERSECUTION, STUNNED, BACKTOPIPE, CLIMBING
    }
}
