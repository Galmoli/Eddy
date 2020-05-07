using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : StateMachine
{
    [SerializeField] private PlayerSwordScanner sword;
    //Player Input
    private InputActions _input;
    
    //Components
    public AttackSO basicAttack;
    public AttackSO areaAttack;
    public AttackSO comboAttack;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.Attack.canceled += ctx => SimpleAttack();
        _input.PlayerControls.Attack.performed += ctx => AreaAttack();
        
        sword = FindObjectOfType<PlayerSwordScanner>();
    }

    private void Start()
    {
        SetState(new IdleState(this));
    }

    private void SimpleAttack()
    {
        if (state.GetType() == typeof(AreaAttackState)) return;
        
        SetState(new SimpleAttackState(this));
    }

    private void AreaAttack()
    {
        SetState(new AreaAttackState(this));
    }
}
