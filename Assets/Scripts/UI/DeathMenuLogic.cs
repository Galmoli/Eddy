using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuLogic : MonoBehaviour
{
    public void Respawn() //Currently used by Button OnClick Event
    {
        GameManager.Instance.Respawn();
        gameObject.SetActive(false);
    }
    
    public void MainMenu() //Currently used by Button OnClick Event
    {
        UIManager.Instance.MainMenu();
    }
}
