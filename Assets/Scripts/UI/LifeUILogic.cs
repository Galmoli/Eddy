using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUILogic : MonoBehaviour
{
    [SerializeField] private Image[] healDots;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Hit(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            GetHealthDot(true).enabled = false;
            anim.SetTrigger("hit");
        }
        StopAllCoroutines();
    }

    public void Heal()
    {
        GetHealthDot(false).enabled = true;
    }

    public void RestoreHealth()
    {
        foreach (var hd in healDots)
        {
            hd.enabled = true;
        }
    }

    private Image GetHealthDot(bool hit)
    {
        if (hit)
        {
            for (int i = healDots.Length - 1; i >= 0; i--)
            {
                if (healDots[i].enabled) return healDots[i];
            }
        }

        foreach (var hd in healDots)
        {
            if (hd.enabled != true) return hd;
        }

        return null;
    }
}
