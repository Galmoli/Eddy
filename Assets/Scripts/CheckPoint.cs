using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPos;
    [SerializeField] private Light light;


    public void Activate()
    {
        GameManager.Instance.respawnPos = respawnPos.position;
        light.enabled = true;
    }
}
