using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderHits : MonoBehaviour
{
    public EnemyAggressiveFSM enemyAggressiveFSM;


    private void OnTriggerEnter(Collider other)
    {
        if (enemyAggressiveFSM.enabled && other.gameObject.layer != LayerMask.NameToLayer("ScannerLayer"))
        { 
            enemyAggressiveFSM.HitHandler(other.transform.gameObject);            
        }
    }
}
