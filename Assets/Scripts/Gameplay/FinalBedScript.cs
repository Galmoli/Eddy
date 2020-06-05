using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        {
            UIManager.Instance.FadeIn();
            StartCoroutine(LoadFinalDialogue());
        }

    }

    IEnumerator LoadFinalDialogue()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("LastDialogueScene");
    }


}
