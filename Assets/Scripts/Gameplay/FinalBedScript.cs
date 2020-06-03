using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBedScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        GeneralDialogue.OnDialogueDisabled += EndGame;
    }


    public void EndGame(string id)
    {
        if (id == "Conversation_7")
            UIManager.Instance.MainMenu();

    }


}
