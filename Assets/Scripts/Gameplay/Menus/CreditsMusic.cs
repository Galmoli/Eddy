using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class CreditsMusic : MonoBehaviour
{
    [Header("Music References")]
    [FMODUnity.EventRef] public string creditsMusic;

    EventInstance credits_Event;

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic()
    {
        credits_Event = RuntimeManager.CreateInstance(creditsMusic);

        if (!credits_Event.Equals(null))
        {
            credits_Event.start();
        }
    }

    public void StopMusic()
    {
        credits_Event.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Destroy(this.gameObject);
    }
}
