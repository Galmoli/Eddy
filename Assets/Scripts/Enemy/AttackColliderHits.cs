using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderHits : MonoBehaviour
{
    public EnemyAggressiveFSM enemyAggressiveFSM;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (enemyAggressiveFSM.enabled)
        {
            enemyAggressiveFSM.HitHandler(other.transform.gameObject);
        }
    }
}
