using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBedScript : MonoBehaviour
{
    private InputActions inputActions;

    // Start is called before the first frame update
    void Awake()
    {
        GeneralDialogue.OnDialogueDisabled += EndGame;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void EndGame(string id)
    {
        if (id == "Conversation_7")
            UIManager.Instance.MainMenu();

    }


}
