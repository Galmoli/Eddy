using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainSword : MonoBehaviour
{
    bool playerInside;

    public CheckPoint checkpoint;

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.PlayerControls.MoveObject.started += ctx => GiveSwordToPlayer();
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
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }
}
