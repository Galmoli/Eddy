using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelperTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Attack, other.transform.position + Vector3.up * 1.5f, other.transform);
            Destroy(gameObject);
        }
    }

}
