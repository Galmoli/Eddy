using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBedScript : MonoBehaviour
{
    bool playerInside;
    private InputActions inputActions;

    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new InputActions();
        inputActions.PlayerControls.MoveObject.started += ctx => EndGame();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void EndGame()
    {
        Debug.Log(playerInside);
        if (playerInside)
        {
            
            UIManager.Instance.MainMenu();
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
