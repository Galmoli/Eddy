using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuLogic : MonoBehaviour
{
    private void Respawn()
    {
        GameManager.Instance.Respawn();
        gameObject.SetActive(false);
    }
    
    private void MainMenu()
    {
        //Change scene and goes to main menu
    }
}
