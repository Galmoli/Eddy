using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogueId;
    public bool general = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (general)
            {
                GeneralDialogue.Instance.EnableDialogue(dialogueId);
            }
            else
            {
                InGameDialogue.Instance.EnableDialogue(dialogueId);
            }

            Destroy(this.gameObject);
        }
    }
}
