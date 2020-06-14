using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class GeneralMusicManager : MonoBehaviour
{

    private static GeneralMusicManager instance;

    public static GeneralMusicManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GeneralMusicManager>();
            return instance;
        }
    }

    [Header("Music References")]
    [FMODUnity.EventRef] public string levelMusic1;
    [FMODUnity.EventRef] public string levelMusic2;

    EventInstance levelMusic1_Event;
    EventInstance levelMusic2_Event;

    int currentMusic;

    bool listenToChangeMusic;
    int levelToChange;

    // Start is called before the first frame update
    void Start()
    {
        PlayLevel1Music();
    }

    private void Update()
    {
        if (listenToChangeMusic)
        {
            if (currentMusic == 1)
            {
                if (!AudioManager.Instance.isPlaying(levelMusic1_Event))
                {
                    if (levelToChange == 1)
                    {
                        PlayLevel1Music();
                    }
                    if (levelToChange == 2)
                    {
                        PlayLevel2Music();
                    }

                    listenToChangeMusic = false;
                }
            }

            if (currentMusic == 2)
            {
                if (!AudioManager.Instance.isPlaying(levelMusic2_Event))
                {
                    if (levelToChange == 1)
                    {
                        PlayLevel1Music();
                    }
                    if (levelToChange == 2)
                    {
                        PlayLevel2Music();
                    }

                    listenToChangeMusic = false;
                }
            }
        }
    }

    public void ChangeMusic (int newMusic)
    {
        if (currentMusic == 1)
        {
            levelMusic1_Event.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //UpdateLevel1Event(1, 2);
        }

        if (currentMusic == 2)
        {
            levelMusic2_Event.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //UpdateLevel2Event(1, 2);
        }

        if (newMusic == 1)
        {
            PlayLevel1Music();
        }

        if (newMusic == 2)
        {
            PlayLevel2Music();
        }

        //listenToChangeMusic = true;
        //levelToChange = newMusic;
    }
    

    public void PlayLevel1Music()
    {
        currentMusic = 1;

        if (AudioManager.Instance.ValidEvent(levelMusic1))
        {
            levelMusic1_Event = AudioManager.Instance.PlayMusic(levelMusic1);
        }
    }

    public void PlayLevel2Music()
    {

        currentMusic = 2;

        if (AudioManager.Instance.ValidEvent(levelMusic2))
        {
            levelMusic2_Event = AudioManager.Instance.PlayMusic(levelMusic2);
        }
    }

    public void UpdateLevel1Event(float progress, float death)
    {
        List<SoundManagerParameter> parameters = new List<SoundManagerParameter>();

        SoundManagerParameter parameter1 = new SoundManagerParameter("PROGRESS", progress);
        SoundManagerParameter parameter2 = new SoundManagerParameter("DEATH", death);

        parameters.Add(parameter1);
        parameters.Add(parameter2);

        AudioManager.Instance.UpdateEventParameters(levelMusic1_Event, parameters);
    }

    public void UpdateLevel2Event(float progress, float death)
    {
        List<SoundManagerParameter> parameters = new List<SoundManagerParameter>();

        SoundManagerParameter parameter1 = new SoundManagerParameter("PROGRESS", progress);
        SoundManagerParameter parameter2 = new SoundManagerParameter("DEATH", death);

        parameters.Add(parameter1);
        parameters.Add(parameter2);

        AudioManager.Instance.UpdateEventParameters(levelMusic2_Event, parameters);
    }
}
