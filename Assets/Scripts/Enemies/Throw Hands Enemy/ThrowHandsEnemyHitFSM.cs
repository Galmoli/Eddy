using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrowHandsEnemyStunFSM))]

public class ThrowHandsEnemyHitFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        STUNNED,
        STAGGERED
    }

    private States currentState;

    private ThrowHandsEnemyBlackboard blackboard;
    private ThrowHandsEnemyStunFSM enemyStunFSM;
    private Rigidbody rigidBody;

    float timer = 0;

    private void Start()
    {
        blackboard = GetComponent<ThrowHandsEnemyBlackboard>();
        enemyStunFSM = GetComponent<ThrowHandsEnemyStunFSM>();
        rigidBody = GetComponent<Rigidbody>();
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
                blackboard.ownKS.position = transform.position;
                blackboard.ownKS.orientation = transform.eulerAngles.y;
                blackboard.ownKS.linearVelocity = (blackboard.player.transform.position - transform.position).normalized;
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
