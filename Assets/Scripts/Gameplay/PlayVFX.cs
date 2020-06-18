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

    PlayerSounds sounds;

    void Start()
    {
        iniCooldown = cooldown;

        sounds = FindObjectOfType<PlayerSounds>();
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
        Sound();
    }

    IEnumerator PlayAndStop()
    {
        cooldown = 0;
        vfx.Play();
        yield return new WaitForSeconds(timer);
        vfx.Stop();
    }

    private void Sound()
    {
        if (AudioManager.Instance.ValidEvent(sounds.confettiPopSoundPath))
        {
            AudioManager.Instance.PlayEvent(sounds.confettiPopSoundPath, transform);
        }
    }
}
