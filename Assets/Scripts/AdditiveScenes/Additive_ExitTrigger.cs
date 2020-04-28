using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Additive_ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<AdditiveSceneManager>().DestroyPreviousScene();
    }
}
