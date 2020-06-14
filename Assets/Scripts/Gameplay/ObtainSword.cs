using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObtainSword : MonoBehaviour
{
    bool playerInside;

    public CheckPoint checkpoint;
    public GameObject[] dialogueTriggers;
    public GameObject[] disableObjects;
    public GameObject[] draggables;

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.PlayerControls.MoveObject.started += ctx => GiveSwordToPlayer();
        GeneralDialogue.OnDialogueDisabled += SwordDialogueCompleted;
    }

    private void OnDestroy()
    {
        GeneralDialogue.OnDialogueDisabled -= SwordDialogueCompleted;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void GiveSwordToPlayer()
    {
        if (playerInside)
        {
            checkpoint.Activate();
            FindObjectOfType<PlayerSwordScanner>().UnlockSword();
            transform.parent.gameObject.SetActive(false);

            for (int i= 0; i< dialogueTriggers.Length; i++)
            {
                dialogueTriggers[i].SetActive(true);
            }

            for (int i = 0; i < disableObjects.Length; i++)
            {
                disableObjects[i].SetActive(false);
            }

            /*draggables[0].SetActive(false);
            draggables[1].SetActive(true);*/

            VibrationManager.Instance.Vibrate(VibrationManager.Presets.SUCCESS);

            GeneralMusicManager.Instance.UpdateLevel1Event(0.7f, 0);

            GeneralDialogue.Instance.EnableDialogue("Conversation_2");
            UIHelperController.Instance.DisableHelper();
        }
    }

    public void SwordDialogueCompleted(string id)
    {
        if (id == "Conversation_2")
        {
            UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Scanner, GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.up * 2,
                GameObject.FindGameObjectWithTag("Player").transform);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Pick, transform.position + Vector3.up*2.5f);
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
