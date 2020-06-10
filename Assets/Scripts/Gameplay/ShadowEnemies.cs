using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShadowEnemies : MonoBehaviour
{
    public Transform target;
    private Vector3 iniTarget;
    public float timer;
    public float lerpSpeed = 0.1f;
    private bool hasBeenActivated;

    void Start()
    {
        iniTarget = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenActivated) StartCoroutine("ActivateShadows");
    }

    IEnumerator ActivateShadows()
    {
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(timer);
        while (Vector3.Distance(transform.position, iniTarget) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, iniTarget, lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
