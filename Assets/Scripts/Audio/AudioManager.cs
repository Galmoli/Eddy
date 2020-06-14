using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    private List<EventInstance> eventsList;

    private EventInstance music;

    private List<SoundManagerMovingSound> positionEvents;

    private Bus musicBus;
    private Bus SFXBus;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                instance = go.AddComponent<AudioManager>();
                instance.name = "AudioManager";
            }
            return instance;
        }
    }

    void Awake()
    {
        if ((instance != null && instance != this))
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
        }

        musicBus = RuntimeManager.GetBus("bus:/Master/Music");
        SFXBus = RuntimeManager.GetBus("bus:/Master/SFX");
        
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume", 1));
        SetSFXVolume(PlayerPrefs.GetFloat("sfxVolume", 1));
    }

    void Update()
    {
        if (positionEvents != null && positionEvents.Count > 0)
        {
            for (int i = 0; i < positionEvents.Count; i++)
            {
                PLAYBACK_STATE state;
                EventInstance eventInst = positionEvents[i].GetEventInstance();
                eventInst.getPlaybackState(out state);
                if (state == PLAYBACK_STATE.STOPPED)
                {
                    positionEvents.RemoveAt(i);
                }
                else
                {
                    eventInst.set3DAttributes(RuntimeUtils.To3DAttributes(positionEvents[i].GetTransform().position));
                }
            }
        }
    }

    private void Init()
    {
        eventsList = new List<EventInstance>();
        positionEvents = new List<SoundManagerMovingSound>();
    }

    public void PlayOneShotSound(string path, Vector3 pos, List<SoundManagerParameter> parameters = null)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            if(parameters!=null)
                for (int i = 0; i < parameters.Count; i++)
                    soundEvent.setParameterByName(parameters[i].GetName(), parameters[i].GetValue());

            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            soundEvent.release();
        }
    }

    public void PlayOneShotSound(string path, Transform transform)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.start();
            SoundManagerMovingSound movingSound = new SoundManagerMovingSound(transform, soundEvent);
            positionEvents.Add(movingSound);
            soundEvent.release();
        }
    }

    public EventInstance PlayEvent(string path, Vector3 pos)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            eventsList.Add(soundEvent);
        }
        return soundEvent;
    }

    public EventInstance PlayEvent(string path, Transform transform)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.start();
            SoundManagerMovingSound movingSound = new SoundManagerMovingSound(transform, soundEvent);
            positionEvents.Add(movingSound);
            eventsList.Add(soundEvent);
        }
        return soundEvent;
    }

    public EventInstance PlayMusic(string path)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.start();
        }
        return soundEvent;
    }

    public void UpdateEventParameter(EventInstance soundEvent, SoundManagerParameter parameter)
    {
        soundEvent.setParameterByName(parameter.GetName(), parameter.GetValue());
    }

    public void UpdateEventParameters(EventInstance soundEvent, List<SoundManagerParameter> parameters)
    {
        for (int i = 0; i < parameters.Count; i++)
            soundEvent.setParameterByName(parameters[i].GetName(), parameters[i].GetValue());
    }

    public void StopEvent(EventInstance soundEvent, bool fadeout = true)
    {
		soundEvent.clearHandle ();
        if (eventsList.Remove(soundEvent))
        {
            if (fadeout)
                soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            else
                soundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void PauseEvent(EventInstance soundEvent)
    {
        if (eventsList.Contains(soundEvent))
        {
            soundEvent.setPaused(true);
        }
    }

    public void ResumeEvent(EventInstance soundEvent)
    {
        if (eventsList.Contains(soundEvent))
        {
            soundEvent.setPaused(false);
        }
    }

    public void StopAllEvents(bool fadeout)
    {
        for (int i = 0; i < eventsList.Count; i++)
        {
            if (fadeout)
                eventsList[i].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            else
                eventsList[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        eventsList.Clear();
    }
    public void PauseAllEvents()
    {
        for (int i = 0; i < eventsList.Count; i++)
        {
            eventsList[i].setPaused(true);
        }
    }

    public void ResumeAllEvents()
    {
        for (int i = 0; i < eventsList.Count; i++)
        {
            eventsList[i].setPaused(false);
        }
    }

	public bool isPlaying(EventInstance soundEvent) {
		PLAYBACK_STATE state;
		soundEvent.getPlaybackState (out state);
		return !state.Equals (PLAYBACK_STATE.STOPPED);
	}

    //MIXER
    public void SetChannelVolume(string channel, float channelVolume)
    {
        VCA vca;
        if (RuntimeManager.StudioSystem.getVCA("vca:/"+channel, out vca) != FMOD.RESULT.OK)
            return;
        vca.setVolume(channelVolume);
    }

    public void SetMusicVolume(float volume)
    {   
        musicBus.setVolume(Mathf.Clamp(volume, 0f, 1f));
    }

    public void SetSFXVolume(float volume)
    {
        SFXBus.setVolume(Mathf.Clamp(volume, 0f, 1f));
    }

    public bool ValidEvent(string eventPath)
    {
        EventDescription desc;
        RuntimeManager.StudioSystem.getEvent(eventPath, out desc);

        if (desc.isValid())
        {
            return true;
        }

        Debug.Log("SOUND NOT FOUND");
        return false;
    }
}

public class SoundManagerParameter
{
    string name;
    float value;

    public SoundManagerParameter(string name, float value)
    {
        this.name = name;
        this.value = value;
    }

    public string GetName()
    {
        return name;
    }

    public float GetValue()
    {
        return value;
    }
}

class SoundManagerMovingSound
{
    Transform transform;
    EventInstance eventIns;

    public SoundManagerMovingSound(Transform transform, EventInstance eventIns)
    {
        this.transform = transform;
        this.eventIns = eventIns;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public EventInstance GetEventInstance()
    {
        return eventIns;
    }
}