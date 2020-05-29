using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivationTrigger : MonoBehaviour
{
    public BossController bossController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bossController.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
