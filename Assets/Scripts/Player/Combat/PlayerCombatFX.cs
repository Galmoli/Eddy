

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatFX : MonoBehaviour
{
    public ParticleSystem swordTrailParticles;
    public ParticleSystem comboTrailParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrailOn()
    {
        swordTrailParticles.gameObject.SetActive(true);
    }
    public void TrailOff()
    {
        swordTrailParticles.gameObject.SetActive(false);
    }
    public void ComboTrailOn()
    {
        comboTrailParticles.gameObject.SetActive(true);
    }
    public void ComboTrailOff()
    {
        comboTrailParticles.gameObject.SetActive(false);
    }
}
