using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHelperTrigger : MonoBehaviour
{
    public int id;
    public float waitToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HelperDialogueController.Instance.ShowHelper(id);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(WaitToDisable());
        }
    }

    IEnumerator WaitToDisable()
    {
        yield return new WaitForSeconds(waitToDisable);
        HelperDialogueController.Instance.HideHelper(id);
    }
}
