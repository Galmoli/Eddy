using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnedObject;

    public void Spawn()
    {
        Instantiate(spawnedObject, transform.position, transform.rotation);
    }
}
