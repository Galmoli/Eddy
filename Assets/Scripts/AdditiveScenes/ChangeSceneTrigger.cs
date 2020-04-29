using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] private bool playerExitsOnRight;
    private Transform _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            WhereIsPlayer();
        }
    }

    private void OnPlayerEnter()
    {
        StartCoroutine(transform.parent.gameObject.GetComponent<AdditiveSceneManager>().LoadNextScene());
    }

    private void OnPlayerExit()
    {
        StartCoroutine(transform.parent.gameObject.GetComponent<AdditiveSceneManager>().LoadPreviousScene());
    }

    private void WhereIsPlayer()
    {
        if (playerExitsOnRight)
        {
            if (GetDotProduct() < 0) OnPlayerEnter();
            else OnPlayerExit();
        }
        else
        {
            if (GetDotProduct() < 0) OnPlayerExit();
            else OnPlayerEnter();
        }
    }

    private float GetDotProduct()
    {
        return Vector3.Dot(transform.right, (_player.position - transform.position).normalized);
    }
}
