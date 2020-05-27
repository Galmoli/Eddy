using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EddyPipe : MonoBehaviour
{
    public Transform[] eddysInstancePos;
    public GameObject EddyRagdoll;

    public int numberOfInstances;
    public float secondsInterval;

    bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !activated)
        {
            StartCoroutine(InstantiateEddies());
            activated = true;
        }
    }

    IEnumerator InstantiateEddies()
    {
        for (int i = 0; i < numberOfInstances; i++) {
            int randomPos = Random.Range(0, eddysInstancePos.Length);
            Instantiate(EddyRagdoll, eddysInstancePos[randomPos].position, eddysInstancePos[randomPos].rotation);
            yield return new WaitForSeconds(secondsInterval);
        }
    }
}
