using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(ArrivePlusAvoid))]
[RequireComponent(typeof(WanderPlusAvoid))]

public class EnemyAgressiveFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        WANDER,
        NOTICE,
        CHASE,
        ATTACK
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private WanderPlusAvoid wanderPlusAvoid;
    private ArrivePlusAvoid arrivePlusAvoid;
    private KinematicState kinematicState;

    private float timeInNotice;
    private float minTimeBetweenAttacks;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        wanderPlusAvoid = GetComponent<WanderPlusAvoid>();
        arrivePlusAvoid = GetComponent<ArrivePlusAvoid>();
        kinematicState = GetComponent<KinematicState>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        wanderPlusAvoid.enabled = false;
        arrivePlusAvoid.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.WANDER);
                break;
            case States.WANDER:

                RaycastHit hit;
                if(Physics.Raycast(transform.position, blackboard.player.transform.position - transform.position, out hit, blackboard.detectionDistanceOnSight, blackboard.sightObstaclesLayers))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        ChangeState(States.NOTICE);
                        break;
                    }
                }
                
                if(Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.detectionDistanceOffSight)
                {
                    ChangeState(States.NOTICE);
                }
                break;
            case States.NOTICE:

                LookAtPlayer();

                if (timeInNotice <= 0)
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
                        ChangeState(States.WANDER);
                        break;
                    }

                    StartCoroutine(Attack());
                    minTimeBetweenAttacks = blackboard.minTimeBetweenAttacks;
                }
                else
                {
                    LookAtPlayer();
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
            case States.WANDER:
                wanderPlusAvoid.enabled = false;
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
            case States.WANDER:
                kinematicState.maxSpeed = blackboard.wanderSpeed;
                wanderPlusAvoid.enabled = true;
                break;
            case States.NOTICE:
                timeInNotice = blackboard.timeInNotice;
                break;
            case States.CHASE:
                kinematicState.maxSpeed = blackboard.chasingSpeed;
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

    private void LookAtPlayer()
    {
        transform.LookAt(blackboard.player.transform);

        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;

        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
