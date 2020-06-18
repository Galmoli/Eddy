﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertPlayerInCloth : MonoBehaviour
{
    Cloth cloth;
    // Start is called before the first frame update
    void Start()
    {
        cloth = GetComponent<Cloth>();
        cloth.capsuleColliders[0] = GameObject.Find("PlayerDummyForPhysics").GetComponent<CapsuleCollider>();
    }

}
