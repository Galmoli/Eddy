using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToBootScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void LoadGame()
    {
        SceneManager.LoadScene("Additive_BootScene");
        FindObjectOfType<CutsceneMusic>().StopMusic();
    }
}
