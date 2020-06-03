using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEnemyListener : MonoBehaviour
{
    public string dialogueId;
    public EnemyBlackboard blackboard;

    private void Update()
    {
        if (blackboard.healthPoints <= 0)
        {
            InGameDialogue.Instance.EnableDialogue(dialogueId);
            this.enabled = false;
        }
    }

}
