using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Additive_EnterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(transform.parent.gameObject.GetComponent<AdditiveSceneManager>().LoadNextScene());
        }
    }
}
