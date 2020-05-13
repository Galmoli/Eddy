using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager
{
    public struct EnemySpawner
    {
        public EnemyBlackboard enemyB;
        public GameObject enemyO;
        public Vector3 spawnPos;
    }

    public List<EnemySpawner> enemyList;

    public EnemySpawnManager()
    {
        enemyList = new List<EnemySpawner>();
    }

    public void Add(EnemyBlackboard eb)
    {
        EnemySpawner es = new EnemySpawner {enemyB = eb, enemyO = eb.gameObject, spawnPos = eb.transform.position};
        enemyList.Add(es);
    }
}
