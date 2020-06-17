using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RagdollHitVFX : MonoBehaviour
{
    private VisualEffect vfx;
    public float timer = 0.2f;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        StartCoroutine("PlayVFX");
    }

    IEnumerator PlayVFX()
    {
        vfx.Play();
        yield return new WaitForSeconds(timer);
        vfx.Stop();
    }
}
