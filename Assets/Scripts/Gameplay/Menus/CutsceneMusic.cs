using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class CutsceneMusic : MonoBehaviour
{
    [Header("Music References")]
    [FMODUnity.EventRef] public string cutsceneMusic;

    EventInstance cutscene_Event;

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
        cutscene_Event = RuntimeManager.CreateInstance(cutsceneMusic);

        if (!cutscene_Event.Equals(null))
        {
            cutscene_Event.start();
        }
    }

    public void StopMusic()
    {
        cutscene_Event.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Destroy(this.gameObject);
    }
}
