using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [Header("Editable Variables")] 
    [SerializeField] private int damage;
    [SerializeField] private float timeToCancellCombo;
    [SerializeField] private HitDetection hitDetection;

    public bool Attack()
    {
        return hitDetection.Check();
    }
}
