using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnedObject;
    public ChargingEnemyBlackboard currentBlackboard;

    private void Update()
    {
        if (currentBlackboard.healthPoints <= 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        currentBlackboard = Instantiate(spawnedObject, transform.position, transform.rotation).GetComponent<ChargingEnemyBlackboard>();
    }
}
