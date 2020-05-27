using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderHits : MonoBehaviour
{
    public ChargingEnemyAggressiveFSM enemyAggressiveFSM;

    private void OnTriggerEnter(Collider other)
    {
        if (enemyAggressiveFSM.enabled && other.gameObject.layer != LayerMask.NameToLayer("ScannerLayer") && other.gameObject.layer != LayerMask.NameToLayer("TriggerDetection"))
        { 
            enemyAggressiveFSM.HitHandler(other.transform.gameObject);            
        }
    }
}
