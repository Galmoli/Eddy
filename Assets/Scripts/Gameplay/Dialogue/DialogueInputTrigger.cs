using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInputTrigger : MonoBehaviour
{
    public string dialogueId;
    public bool general = true;
    public UIHelperController.HelperAction action;

    bool playerInside;
    private InputActions inputActions;

    public void Awake()
    {
        inputActions = new InputActions();
        inputActions.PlayerControls.MoveObject.started += ctx => OpenDialogue();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void OpenDialogue()
    {
        if (playerInside)
        {
            if (general)
            {
                GeneralDialogue.Instance.EnableDialogue(dialogueId);
            }
            else
            {
                InGameDialogue.Instance.EnableDialogue(dialogueId);
            }

            UIHelperController.Instance.DisableHelper();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIHelperController.Instance.EnableHelper(action, transform.position);
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
           UIHelperController.Instance.DisableHelper();
           playerInside = false;
        }
    }
}
