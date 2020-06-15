using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelperTrigger : MonoBehaviour
{
    public bool spin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!spin)
                UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Attack, other.transform.position + Vector3.up * 1.5f, other.transform);
            else
                UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.SpinAttack, other.transform.position + Vector3.up * 1.5f, other.transform);

            Destroy(gameObject);
        }
    }

}
