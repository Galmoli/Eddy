using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToCutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoadGame()
    {
        SceneManager.LoadScene("InitialCutscene");
    }
}

