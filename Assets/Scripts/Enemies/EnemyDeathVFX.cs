using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyDeathVFX : MonoBehaviour
{
    VisualEffect vfx;
    public float timer = 1f;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        StartCoroutine("DeactivateVFX");
    }

    IEnumerator DeactivateVFX()
    {
        yield return new WaitForSeconds(timer);
        vfx.Stop();
    }
}
