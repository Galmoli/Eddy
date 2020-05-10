using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPos;
    
    
    public void Activate()
    {
        GameManager.Instance.respawnPos = respawnPos.position;
    }
}
