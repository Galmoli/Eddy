using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargingEnemyAggressiveFSM))]

public class ChargingEnemyStunFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        AGRESSIVE,
        STUNNED
    }

    private States currentState;

    private ChargingEnemyBlackboard blackboard;
    private ChargingEnemyAggressiveFSM enemyAgressiveFSM;

    float timer;

    private void Start()
    {
        blackboard = GetComponent<ChargingEnemyBlackboard>();
        enemyAgressiveFSM = GetComponent<ChargingEnemyAggressiveFSM>();

        timer = 0;
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        enemyAgressiveFSM.enabled = false;
        blackboard.stunned = false;
        timer = 0;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.AGRESSIVE);
                break;
            case States.AGRESSIVE:
                
                if (blackboard.stunned)
                {
                    ChangeState(States.STUNNED);
                    break;
                }

                break;
            case States.STUNNED:
                if(timer >= blackboard.stunnedTime)
                {
                    ChangeState(States.AGRESSIVE);
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
            case States.AGRESSIVE:
                enemyAgressiveFSM.enabled = false;
                break;
            case States.STUNNED:
                timer = 0;
                blackboard.ownKS.orientation = transform.eulerAngles.y;
                blackboard.ownKS.linearVelocity = (blackboard.player.transform.position - transform.position).normalized;
                blackboard.stunned = false;
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
                blackboard.rb.AddForce(-transform.forward * blackboard.stunImpulse, ForceMode.Impulse);
                break;
        }

        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }
}
