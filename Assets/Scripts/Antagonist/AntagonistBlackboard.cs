using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistBlackboard : MonoBehaviour
{
    public float guardingSpeed;
    public float persecutionSpeed;
    public float obstacleImpactForce;
    public float stunnedTime;
    public float downPipeImpulse;
    public float pipeTime;

    public GameObject player;

    public Collider attackCollider;
    public GameObject guardingCol;
    public GameObject endCol;

    public GameObject firstObstacle;
    public GameObject secondObstacle;
    public GameObject thirdObstacle;

    public GameObject firstDestiny;
    public GameObject secondDestiny;

    public GameObject pipe;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
