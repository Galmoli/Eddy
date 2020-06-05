using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornedEnemyWall : MonoBehaviour
{
    public float power, radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && !collision.gameObject.GetComponent<HornedEnemyWall>())
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null && hit.transform.tag != "Enemy")
                {
                    if (rb.isKinematic && rb.GetComponent<HornedEnemyWall>())
                    {
                        rb.isKinematic = false;
                        rb.AddExplosionForce(power, explosionPos - collision.GetContact(0).normal * 2, radius);
                        Destroy(rb.gameObject, 5.0f);
                    }

                }
            }
        }
    }
}
