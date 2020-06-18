using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCameraTravelling : MonoBehaviour
{
    public PlayerMovementController playerMovementController;
    public CameraController cameraController;
    public Animator animator;
    public GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        PausePlayer();
    }

    public void PausePlayer()
    {
        cameraController.enabled = false;
        playerMovementController.dontMove = true;
        healthBar.SetActive(false);
    }

    public void EnablePlayer()
    {
        playerMovementController.dontMove = false;
        cameraController.enabled = true;
        animator.enabled = false;
        healthBar.SetActive(true);
        UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Move, playerMovementController.transform.position + Vector3.up * 2, playerMovementController.transform);
    }
}
