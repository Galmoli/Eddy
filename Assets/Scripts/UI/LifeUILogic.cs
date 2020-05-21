using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUILogic : MonoBehaviour
{
    [SerializeField] private Image[] healDots;
    [SerializeField] private float timeToRegenerate;

    public void Hit()
    {
        GetHealthDot(true).fillAmount -= 0.25f;
        StopAllCoroutines();
        StartCoroutine(Co_Heal());
    }

    private void Heal()
    {
        GetHealthDot(false).fillAmount += 0.25f;
        UIManager.OnHeal();
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

    private IEnumerator Co_Heal()
    {
        var currentTime = 0f;
        while (GetHealthPoints() < healDots.Length * 4)
        {
            if (currentTime < timeToRegenerate)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                Heal();
            }
            yield return null;
        }
    }

    private int GetHealthPoints()
    {
        int p = 0;
        foreach (var hd in healDots)
        {
            switch (hd.fillAmount)
            {
                case 0f:
                    break;
                case 0.25f:
                    p++;
                    break;
                case 0.5f:
                    p += 2;
                    break;
                case 0.75f:
                    p += 3;
                    break;
                case 1f:
                    p += 4;
                    break;
            }
        }

        return p;
    }
}
