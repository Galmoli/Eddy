using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    public int level; //1 or 2
    public float progression;

    public bool changeLevel;
    public bool disable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (changeLevel)
            {
                GeneralMusicManager.Instance.ChangeMusic(level);
            }
            else
            {
                if (level == 1)
                {
                    GeneralMusicManager.Instance.UpdateLevel1Event(progression, 0);
                }
                else
                {
                    GeneralMusicManager.Instance.UpdateLevel2Event(progression, 0);
                }
            }

            if (disable)
                Destroy(this.gameObject);

        }
    }
}
