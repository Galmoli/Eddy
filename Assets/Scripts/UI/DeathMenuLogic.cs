using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuLogic : MonoBehaviour
{
    public void Respawn() //Currently used by Button OnClick Event
    {
        GameManager.Instance.Respawn();
        gameObject.SetActive(false);
    }
    
    private void MainMenu()
    {
        //Change scene and goes to main menu
    }
}
