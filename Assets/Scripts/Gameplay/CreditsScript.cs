using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CreditsScript : MonoBehaviour
{

    public WaveScene waveScene;
    public VisualEffect[] effects;

    // Start is called before the first frame update
    void Start()
    {
       

       
    }

    public void Crowd()
    {
        waveScene.waveActivated = true;

        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Play();
        }
    }

    public void PlayConfetti()
    {
        waveScene.playLeftConfetti();
        waveScene.playRightConfetti();
    }
}
