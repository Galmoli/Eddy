using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    
    [Header("Editable Variables")] 
    public int damage;
    [SerializeField] private float timeToCancellCombo;
    [SerializeField] private HitDetection hitDetection;

    public HitDetection.HitStruct Attack()
    {
        return hitDetection.Check();
    }
}
