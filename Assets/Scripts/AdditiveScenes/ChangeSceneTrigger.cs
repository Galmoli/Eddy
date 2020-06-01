using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    private enum RespectiveToTrigger
    {
        Right,
        Left
    }
    
    [SerializeField] private bool playerExitsOnRight;
    private Transform _player;
    private AdditiveSceneManager _sceneManager;
    private RespectiveToTrigger _respectiveToTrigger;
    private bool triggerActivated = false;

    private void Awake()
    {
        _sceneManager = transform.parent.gameObject.GetComponent<AdditiveSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            _respectiveToTrigger = TriggerEnterCheck();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            if(_respectiveToTrigger != TriggerEnterCheck()) WhereIsPlayer();
        }
    }

    private void OnPlayerEnter()
    {
        StartCoroutine(_sceneManager.LoadNextScene());
    }

    private void OnPlayerExit()
    {
        StartCoroutine(_sceneManager.LoadPreviousScene());
    }

    private void WhereIsPlayer()
    {
        if (!playerExitsOnRight)
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

    private RespectiveToTrigger TriggerEnterCheck()
    {
        if (GetDotProduct() < 0) return RespectiveToTrigger.Left;
        return RespectiveToTrigger.Right;
    }

    private float GetDotProduct()
    {
        return Vector3.Dot(transform.right, (_player.position - transform.position).normalized);
    }
}
