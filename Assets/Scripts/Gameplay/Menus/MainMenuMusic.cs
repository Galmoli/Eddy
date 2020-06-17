using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class MainMenuMusic : MonoBehaviour
{
    [Header("Music References")]
    [FMODUnity.EventRef] public string mainMenuMusic;

    EventInstance mainMenu_Event;

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        StopMusic();
    }

    public void PlayMusic()
    {
        mainMenu_Event = RuntimeManager.CreateInstance(mainMenuMusic);

        if (!mainMenu_Event.Equals(null))
        {
            mainMenu_Event.start();
        }
    }

    public void StopMusic()
    {
        mainMenu_Event.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
