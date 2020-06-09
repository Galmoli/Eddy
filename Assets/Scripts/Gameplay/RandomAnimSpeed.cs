using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimSpeed : MonoBehaviour
{
    private Animator anim;
    public float minSpeed;
    public float maxSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(minSpeed, maxSpeed);
    }
}
