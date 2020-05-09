using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private PlayerController _playerController;

    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    [HideInInspector] public  Vector3 respawnPos;

    private void Start()
    {
        var player = GameObject.Find("Player");
        respawnPos = player.transform.position;
        _playerController = player.GetComponent<PlayerController>();
    }

    public void Respawn()
    {
        _playerController.Spawn();
        //Re-locate enemies.
    }
}
