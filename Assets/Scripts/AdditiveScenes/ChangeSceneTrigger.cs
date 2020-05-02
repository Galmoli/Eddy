using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    private enum TriggerPos
    {
        Right,
        Left
    }
    
    [SerializeField] private bool playerChangesSceneOnRight;
    private Transform _player;
    private AdditiveSceneManager _sceneManager;
    private bool triggerActivated = false;
    private TriggerPos playerEnteredPos;

    private void Awake()
    {
        _sceneManager = transform.parent.gameObject.GetComponent<AdditiveSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            SetPlayerEnterPos();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WhereIsPlayer();
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
        var playerExitPos = GetDotProduct();
        if (playerExitPos == playerEnteredPos) return;
        
        if (playerChangesSceneOnRight)
        {
            if (playerExitPos == TriggerPos.Left) OnPlayerEnter();
            else OnPlayerExit();
        }
        else
        {
            if (playerExitPos == TriggerPos.Left) OnPlayerExit();
            else OnPlayerEnter();
        }
    }

    private void SetPlayerEnterPos()
    {
        var dot = Vector3.Dot(transform.right, (_player.position - transform.position).normalized);
        if (dot < 0) playerEnteredPos = TriggerPos.Left;
        else playerEnteredPos = TriggerPos.Right;
    }

    private TriggerPos GetDotProduct()
    {
        var dot = Vector3.Dot(transform.right, (_player.position - transform.position).normalized);
        
        if (dot < 0) return TriggerPos.Left;
        return TriggerPos.Right;
    }
}
