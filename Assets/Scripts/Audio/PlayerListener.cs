using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListener : MonoBehaviour
{
    void Update()
    {
        if(Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
