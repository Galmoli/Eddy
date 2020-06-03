using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPos;
    [SerializeField] private Light light;

    public bool dialogue = false;

    public void Activate()
    {
        GameManager.Instance.respawnPos = respawnPos.position;
        FindObjectOfType<PlayerController>().RestoreHealth();
        light.enabled = true;

        if (dialogue)
        {
            GeneralDialogue.Instance.EnableDialogue("Conversation_4");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.NailSword, transform.position+ Vector3.up * 2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UIHelperController.Instance.DisableHelper();
        }
    }
}
