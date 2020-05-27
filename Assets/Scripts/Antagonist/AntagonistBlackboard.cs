using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistBlackboard : MonoBehaviour
{
    public float guardingSpeed;
    public GameObject player;

    public Collider attackCollider;
    public GameObject guardingCol;

    public GameObject firstObstacle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
