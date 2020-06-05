using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivation : MonoBehaviour
{
    private GameObject player;
    public float distanceToActivate = 15;
    private ParticleSystem particles;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToActivate)
        {
            if (!particles.isPlaying) particles.Play();
        }
        else particles.Stop();
    }
}
