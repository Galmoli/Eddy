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
    [SerializeField] private float timeToShowMenu;
    [SerializeField] private GameObject deathMenu;
    
    public IEnumerator ShowDeathMenu()
    {
        var currentTime = 0f;
        while (currentTime < timeToShowMenu)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        deathMenu.SetActive(true);
    }
}
