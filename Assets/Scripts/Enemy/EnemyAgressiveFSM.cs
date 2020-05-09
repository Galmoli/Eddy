using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(EnemyPassiveFSM))]
[RequireComponent(typeof(ArrivePlusAvoid))]

public class EnemyAgressiveFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        PASSIVE,
        NOTICE,
        CHASE,
        ATTACK
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private EnemyPassiveFSM enemyPassiveFSM;
    private ArrivePlusAvoid arrivePlusAvoid;

    private float timeInNotice;
    private float minTimeBetweenAttacks;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        enemyPassiveFSM = GetComponent<EnemyPassiveFSM>();
        arrivePlusAvoid = GetComponent<ArrivePlusAvoid>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        enemyPassiveFSM.enabled = false;
        arrivePlusAvoid.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.PASSIVE);
                break;
            case States.PASSIVE:

                /*RaycastHit hit;
                if(Physics.Raycast(transform.position, blackboard.player.transform.position - transform.position, out hit, blackboard.detectionDistanceOnSight, blackboard.sightObstaclesLayers))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        ChangeState(States.NOTICE);
                        break;
                    }
                }*/
                
                if(Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.detectionDistanceOffSight)
                {
                    ChangeState(States.NOTICE);
                }
                break;
            case States.NOTICE:
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, blackboard.player.transform.position - transform.position, blackboard.rotationSpeed * Time.deltaTime, 0f));
                if(timeInNotice <= 0)
                {
                    ChangeState(States.CHASE);
                }
                else
                {
                    timeInNotice -= Time.deltaTime;
                }
                break;
            case States.CHASE:
                if(Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.attackDistance)
                {
                    ChangeState(States.ATTACK);
                }
                break;
            case States.ATTACK:     
                if (minTimeBetweenAttacks <= 0)
                {
                    if (Vector3.Distance(transform.position, blackboard.player.transform.position) >= blackboard.attackDistance)
                    {
                        ChangeState(States.PASSIVE);
                        break;
                    }

                    StartCoroutine(Attack());
                    minTimeBetweenAttacks = blackboard.minTimeBetweenAttacks;
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, blackboard.player.transform.position - transform.position, blackboard.rotationSpeed * Time.deltaTime, 0f));
                    minTimeBetweenAttacks -= Time.deltaTime;
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
            case States.PASSIVE:
                enemyPassiveFSM.enabled = false;
                break;
            case States.NOTICE:
                break;
            case States.CHASE:
                arrivePlusAvoid.enabled = false;
                break;
            case States.ATTACK:
                break;
        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.PASSIVE:
                enemyPassiveFSM.enabled = true;
                break;
            case States.NOTICE:
                timeInNotice = blackboard.timeInNotice;
                break;
            case States.CHASE:
                arrivePlusAvoid.enabled = true;
                arrivePlusAvoid.target = blackboard.player.gameObject;
                break;
            case States.ATTACK:
                break;
        }
        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }

    IEnumerator Attack()
    {
        blackboard.attack.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        blackboard.attack.SetActive(false);
    }
}
