using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    [HideInInspector] public Vector3 respawnPos;
    [HideInInspector] public EnemySpawnManager enemySpawnManager;
    private PlayerController _playerController;

    private void Start()
    {
        var player = GameObject.Find("Player");
        respawnPos = player.transform.position;
        _playerController = player.GetComponent<PlayerController>();
        enemySpawnManager = new EnemySpawnManager();
    }

    public void Respawn()
    {
        _playerController.Spawn();
        foreach (var e in enemySpawnManager.enemyList)
        {
            if(!e.enemyO.activeSelf) e.enemyO.SetActive(true);
            e.enemyO.transform.position = e.spawnPos;
            e.enemyB.ResetHealth();
        }
    }
}
