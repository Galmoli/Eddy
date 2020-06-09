using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToMainMenu : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {

    }

    public void LoadGame()
    {
        animator.SetTrigger("Out");
        StartCoroutine(WaitToLoadLevel());       
    }

    IEnumerator WaitToLoadLevel()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }


}
