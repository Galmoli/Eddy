using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{
    public VisualEffect vfx;
    public float timer;

    public float cooldown;
    private float iniCooldown;

    void Start()
    {
        iniCooldown = cooldown;
    }

    void Update()
    {
        if (cooldown <= iniCooldown) cooldown += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && cooldown >= iniCooldown) PlayAndStopParticles();
    }
    public void PlayAndStopParticles()
    {
        StartCoroutine("PlayAndStop");
    }

    IEnumerator PlayAndStop()
    {
        cooldown = 0;
        vfx.Play();
        yield return new WaitForSeconds(timer);
        vfx.Stop();
    }
}
