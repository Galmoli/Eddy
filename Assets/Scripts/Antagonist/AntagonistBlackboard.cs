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
    public float endingPipeTime;

    public GameObject player;

    public Collider attackCollider;
    public GameObject guardingCol;
    public GameObject endCol;

    public GameObject firstObstacle;
    public GameObject secondObstacle;
    public GameObject thirdObstacle;

    public GameObject[] destinies;

    public GameObject pipe;
    public GameObject[] pipes;

    public GameObject[] zones;

    public GameObject[] pipePositions;

    public GameObject enemyCollider;

    public GameObject[] activateWhenGuillotine;

    public Animator animator;

    public GameObject head;
    public GameObject mesh;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
