using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderHits : MonoBehaviour
{
    public ChargingEnemyAggressiveFSM enemyAggressiveFSM;

    private void OnTriggerEnter(Collider other)
    {
        if (enemyAggressiveFSM.enabled && other.gameObject.layer != LayerMask.NameToLayer("ScannerLayer")
            && other.gameObject.layer != LayerMask.NameToLayer("TriggerDetection") && other.gameObject.tag != "EnemyDead")
        { 
            if(other.tag == "Metal") enemyAggressiveFSM.HitHandler(other.transform.gameObject, true);
            else enemyAggressiveFSM.HitHandler(other.transform.gameObject, false);
        }
    }
}
