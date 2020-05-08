using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetection : MonoBehaviour
{
    private PlayerMovementController _playerMovementController;

    private void Awake()
    {
        _playerMovementController = transform.parent.gameObject.GetComponent<PlayerMovementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EdgeTrigger"))
        {
            _playerMovementController.OnEdge();
            _playerMovementController.edgePosition = other.transform.position;
            _playerMovementController.edgeGameObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EdgeTrigger"))
        {
            _playerMovementController.OffEdge();
        }
    }
}
