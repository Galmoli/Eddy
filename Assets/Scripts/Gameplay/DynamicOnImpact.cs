﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOnImpact : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {

                GetComponent<Rigidbody>().isKinematic = false;
            
        }
    }

}
