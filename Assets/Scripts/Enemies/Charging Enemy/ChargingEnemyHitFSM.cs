using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargingEnemyStunFSM))]

public class ChargingEnemyHitFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        STUNNED,
        STAGGERED
    }

    private States currentState;

    private ChargingEnemyBlackboard blackboard;
    private ChargingEnemyStunFSM enemyStunFSM;
    private Rigidbody rigidBody;
    private KinematicState kinematicState;

    float timer = 0;

    private void Start()
    {
        blackboard = GetComponent<ChargingEnemyBlackboard>();
        enemyStunFSM = GetComponent<ChargingEnemyStunFSM>();
        rigidBody = GetComponent<Rigidbody>();
        kinematicState = GetComponent<KinematicState>();
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

                if(timer >= blackboard.staggeredTime)
                {
                    ChangeState(States.STUNNED);
                    break;
                }

                timer += Time.deltaTime;
                
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
                kinematicState.position = transform.position;
                kinematicState.orientation = transform.eulerAngles.y;
                kinematicState.linearVelocity = (blackboard.player.transform.position - transform.position).normalized;
                timer = 0;
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
                rigidBody.AddForce(blackboard.hitDirection.normalized * blackboard.staggerImpulse, ForceMode.Impulse);
                break;
        }

        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }
}
