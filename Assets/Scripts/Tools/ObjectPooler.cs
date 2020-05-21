using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    
    private List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;

    void Awake() {
        SharedInstance = this;
    }
    
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(false); 
            pooledObjects.Add(obj);
        }
    }
    
    public GameObject GetPooledObject()
    {
        foreach (var t in pooledObjects)
        {
            if (!t.activeInHierarchy) return t;
        }

        if (shouldExpand) {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

    public void DisableAllObjects()
    {
        foreach (var o in pooledObjects)
        {
            o.SetActive(false);
        }
    }
}
