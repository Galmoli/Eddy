using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEnableDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueTrigger;
    public EnemyBlackboard blackboard;

    private void Update()
    {
        if (blackboard.healthPoints <= 0)
        {
            dialogueTrigger.SetActive(true);
            this.enabled = false;
        }
    }
}
