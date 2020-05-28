using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

[RequireComponent(typeof(WanderPlusAvoid))]
[RequireComponent(typeof(ArrivePlusAvoid))]

public class ThrowHandsEnemyPassiveFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        BACK_TO_INITIAL,
        IDLE,
        WANDER
    }

    private States currentState;

    private ThrowHandsEnemyBlackboard blackboard;
    private WanderPlusAvoid wanderPlusAvoid;
    private ArrivePlusAvoid arrivePlusAvoid;

    private float timer;

    private void Start()
    {
        blackboard = GetComponent<ThrowHandsEnemyBlackboard>();
        wanderPlusAvoid = GetComponent<WanderPlusAvoid>();
        arrivePlusAvoid = GetComponent<ArrivePlusAvoid>();

        timer = 0;
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        wanderPlusAvoid.enabled = false;
        arrivePlusAvoid.enabled = false;
        blackboard.animator.SetFloat("speed", 0);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:

                ChangeState(States.BACK_TO_INITIAL);

                break;
            case States.BACK_TO_INITIAL:
                blackboard.animator.SetFloat("speed", blackboard.ownKS.linearVelocity.magnitude);
                if (Vector3.Distance(transform.position, blackboard.initialTransform.transform.position) <= blackboard.closeEnoughRadius)
                {
                    ChangeState(States.IDLE);
                    break;
                }

                break;
            case States.IDLE:

                if (blackboard.canWander)
                {

                    if (timer >= blackboard.idleTime)
                    {
                        ChangeState(States.WANDER);
                        break;
                    }

                    timer += Time.deltaTime;
                }

                break;
            case States.WANDER:
                blackboard.animator.SetFloat("speed", blackboard.ownKS.linearVelocity.magnitude);
                if (timer >= blackboard.wanderTime)
                {
                    ChangeState(States.IDLE);
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

            case States.BACK_TO_INITIAL:
                arrivePlusAvoid.enabled = false;
                blackboard.rb.velocity = blackboard.ownKS.linearVelocity;
                break;
            case States.IDLE:
                timer = 0;
                break;
            case States.WANDER:
                wanderPlusAvoid.enabled = false;
                timer = 0;
                blackboard.rb.velocity = blackboard.ownKS.linearVelocity;
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.BACK_TO_INITIAL:
                arrivePlusAvoid.target = blackboard.initialTransform;
                arrivePlusAvoid.enabled = true;
                break;
            case States.IDLE:
                
                break;
            case States.WANDER:
                blackboard.ownKS.maxSpeed = blackboard.wanderSpeed;
                wanderPlusAvoid.enabled = true;
                break;
        }
        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }
}
