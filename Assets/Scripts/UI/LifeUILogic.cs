using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUILogic : MonoBehaviour
{
    /*
    [SerializeField] private Image[] healDots;
    

    public void Hit(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            GetHealthDot(true).fillAmount -= 0.25f;
        }
        StopAllCoroutines();
    }

    public void Heal()
    {
        GetHealthDot(false).fillAmount += 0.25f;
    }

    public void RestoreHealth()
    {
        foreach (var hd in healDots)
        {
            hd.fillAmount = 1;
        }
    }

    private Image GetHealthDot(bool hit)
    {
        if (hit)
        {
            for (int i = healDots.Length -1; i >= 0; i--)
            {
                if (healDots[i].fillAmount != 0) return healDots[i];
            }
        }

        foreach (var hd in healDots)
        {
            if (hd.fillAmount != 1) return hd;
        }

        return null;
    }
    */
    [SerializeField] private Image[] healDots;


    public void Hit(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            GetHealthDot(true).enabled = false;
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
                if (healDots[i].enabled != false) return healDots[i];
            }
        }

        foreach (var hd in healDots)
        {
            if (hd.enabled != true) return hd;
        }

        return null;
    }
}
