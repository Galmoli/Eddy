﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private GameObject tempSword;
    //Player Input
    private InputActions _input;
    
    //Local variables
    private bool _attack;

    private void Awake()
    {
        _input = new InputActions();
        _input.PlayerControls.Attack.started += Attack;
    }

    private void Start()
    {
        tempSword.SetActive(false);
    }
    
    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        tempSword.SetActive(true);
        StartCoroutine(Co_Attack());
    }

    private IEnumerator Co_Attack()
    {
        yield return new WaitForSeconds(0.2f);
        tempSword.SetActive(false);
    }
}
