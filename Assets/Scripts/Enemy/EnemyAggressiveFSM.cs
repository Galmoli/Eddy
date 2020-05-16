using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(WanderPlusAvoid))]
[RequireComponent(typeof(EnemyPassiveFSM))]

public class EnemyAggressiveFSM : MonoBehaviour
{
    public enum States
    {
        INITIAL,
        ENEMY_PASSIVE,
        NOTICE,
        CHASE
    }

    private States currentState;

    private EnemyBlackboard blackboard;
    private WanderPlusAvoid wanderPlusAvoid;
    private Seek seek;
    private EnemyPassiveFSM enemyPassiveFsm;
    private CapsuleCollider enemyCol;
    private KinematicState kinematicState;

    private float timer;

    private void Start()
    {
        blackboard = GetComponent<EnemyBlackboard>();
        wanderPlusAvoid = GetComponent<WanderPlusAvoid>();
        seek = GetComponent<Seek>();
        enemyPassiveFsm = GetComponent<EnemyPassiveFSM>();
        kinematicState = GetComponent<KinematicState>();
        enemyCol = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        currentState = States.INITIAL;
    }

    private void OnDisable()
    {
        wanderPlusAvoid.enabled = false;
        seek.enabled = false;
        enemyPassiveFsm.enabled = false;
        blackboard.attackCollider.enabled = false;

        enemyCol.height = 2.0f;
        enemyCol.center = Vector3.zero;

        timer = 0;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.INITIAL:
                ChangeState(States.ENEMY_PASSIVE);
                break;
            case States.ENEMY_PASSIVE:

                RaycastHit hit;
                if(Physics.Raycast(transform.position, blackboard.player.transform.position - transform.position, out hit, blackboard.detectionDistanceOnSight, blackboard.sightObstaclesLayers))
                {
                    
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        if (Mathf.Acos(Vector3.Dot((blackboard.player.transform.position - transform.position).normalized, Vector3.forward)) <= blackboard.visionAngle)
                        {
                            ChangeState(States.NOTICE);
                            break;
                        }
                    }
                }

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.detectionDistanceOffSight)
                {
                    ChangeState(States.NOTICE);
                }
                break;
            case States.NOTICE:
               

                if (timer >= blackboard.timeInNotice)
                {
                    ChangeState(States.CHASE);
                    break;
                }

                LookAtPlayer();
                timer += Time.deltaTime;
                
                break;
            case States.CHASE:

                RaycastHit hit2;

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) >= blackboard.playerOutOfRangeDistance)
                {
                    ChangeState(States.ENEMY_PASSIVE);
                    break;
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
            case States.ENEMY_PASSIVE:
                enemyPassiveFsm.enabled = false;
                break;
            case States.NOTICE:
                break;
            case States.CHASE:
                enemyCol.height = 2.0f;
                enemyCol.center = Vector3.zero;
                blackboard.attackCollider.enabled = false;
                seek.enabled = false;
                break;

        }

        switch (newState)
        {
            case States.INITIAL:
                break;
            case States.ENEMY_PASSIVE:
                enemyPassiveFsm.enabled = true;
                break;
            case States.NOTICE:

                break;
            case States.CHASE:
                enemyCol.height = blackboard.enemyColliderChaseHeight;
                enemyCol.center = enemyCol.center - new Vector3(0, (2.0f - blackboard.enemyColliderChaseHeight) / 2, 0);
                blackboard.attackCollider.enabled = true;
                kinematicState.maxSpeed = blackboard.chasingSpeed;
                seek.enabled = true;
                seek.target = blackboard.player.gameObject;
                break;

        }
        currentState = newState;
        blackboard.statesText.text = currentState.ToString();
    }

    public void HitHandler(GameObject objectHit)
    {
        HornedEnemyWall enemyWall = objectHit.GetComponent<HornedEnemyWall>();

        if (enemyWall == null)
        {
            if (objectHit.tag == "Player")
            {
                blackboard.player.GetComponent<PlayerController>().Hit((int)blackboard.attackPoints);
            }

            blackboard.stunned = true;
        }

      
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
