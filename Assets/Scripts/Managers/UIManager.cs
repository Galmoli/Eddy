using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }
    [SerializeField] private GameObject deathMenu;
    
    public void ShowDeathMenu()
    {
        deathMenu.SetActive(true);
    }
}
