using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerCombatController : StateMachine
{
    [SerializeField] private PlayerSwordScanner sword;
    //Player Input
    private InputActions _input;
    
    //Components
    public AttackSO basicAttack;
    public AttackSO areaAttack;
    public AttackSO comboAttack;

    public HitDetection swordTrigger;

    [HideInInspector] public PlayerSounds playerSounds;
    
    //Variables
    public int attacksToCombo; 
    public float animStopTime;
    
    [SerializeField] private float timeToCancelCombo;
    [SerializeField] private float timeToStartCharging;
    [SerializeField] private float maxChargeTime;
    [HideInInspector] public int simpleAttackCount;
    [HideInInspector] public bool nextAttackReserved;
    [HideInInspector] public EnemyBlackboard target;
    
    private float _timeSinceLastSimpleAttack;
    private float _timeCharging;
    
    private Coroutine _comboCoroutine;
    private Coroutine _chargeCoroutine;
    private PlayerMovementController _movementController;
    private PlayerSwordScanner _swordScanner;

    [Header("Animation")]
    public Animator animator;

    [Header("VFX")]
    public VisualEffect damagedVFX;
    public float timerDamagedVFX = 0.2f;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.Attack.started += ctx => SimpleAttack(false);
        _input.PlayerControls.Attack.canceled += ctx => InputRelease();
        
        SetState(new IdleState(this));
        swordTrigger.OnHit += StateInteract;
        
        sword = FindObjectOfType<PlayerSwordScanner>();
        _movementController = GetComponent<PlayerMovementController>();
        playerSounds = GetComponent<PlayerSounds>();

    }

    private void Start()
    {
        swordTrigger.DisableTrigger();
    }

    private void Update()
    {
        state.Update();
        if(nextAttackReserved && state.GetType() == typeof(IdleState)) SimpleAttack(true);
    }

    private void SimpleAttack(bool auto)
    {
        if (!sword.HoldingSword()) return;
        if (_movementController.GetState().GetType() != typeof(MoveState) && _movementController.GetState().GetType() != typeof(CombatState)) return;
        if (_chargeCoroutine != null && !auto) StopCoroutine(_chargeCoroutine);

        if (state.GetType() == typeof(SimpleAttackState) && !auto)
        {
            if (simpleAttackCount == 0)
            {
                nextAttackReserved = false;
                return;
            }
            _chargeCoroutine = StartCoroutine(ChargeCounter());
            nextAttackReserved = true;
            return;
        }
        
        if (_comboCoroutine != null) StopCoroutine(_comboCoroutine);


        SetState(new SimpleAttackState(this));
        _comboCoroutine = StartCoroutine(ComboCounter());
        nextAttackReserved = false;
        if(!auto) _chargeCoroutine = StartCoroutine(ChargeCounter());
    }

    private void InputRelease()
    {
        if (state.GetType() == typeof(SimpleAttackState))
        {
            animator.SetBool("isChargingAttack", false);
            StopCoroutine(_chargeCoroutine);
            return;
        }

        if (state.GetType() == typeof(IdleChargedState))
        {
            StateInteract();
        }
    }

    private void StateInteract()
    {
        state.Interact();
    }

    public void SetMovementControllerCombatState(float time)
    {
        _movementController.SetState(new CombatState(_movementController, this, time));
    }

    public void SetMovementControllerToMove()
    {
        _movementController.GetState().ExitState();
    }

    public void SetTarget(EnemyBlackboard enemy)
    {
        target = enemy;
    }

    public bool IsAttacking()
    {
        return state.GetType() == typeof(SimpleAttackState) || state.GetType() == typeof(AreaAttackState) || state.GetType() == typeof(IdleChargedState);
    }

    private IEnumerator ComboCounter()
    {
        _timeSinceLastSimpleAttack = 0;
        while (_timeSinceLastSimpleAttack < timeToCancelCombo + basicAttack.attackTime)
        {
            _timeSinceLastSimpleAttack += Time.deltaTime;
            yield return true;    
        }
        simpleAttackCount = 0;
    }

    private IEnumerator ChargeCounter()
    {
        _timeCharging = 0;
        while (state.GetType() == typeof(SimpleAttackState) || state.GetType() == typeof(IdleState))
        {
            _timeCharging += Time.deltaTime;
            if (_timeCharging >= timeToStartCharging && state.GetType() != typeof(IdleChargedState))
            {
                swordTrigger.DisableTrigger();
                SetState(new IdleChargedState(this));
            }
            yield return null;
        }
    }

    public State GetState()
    {
        return state;
    }

    public void AnimStop()
    {
        StartCoroutine(Co_AnimStop());
    }

    private IEnumerator Co_AnimStop()
    {
        animator.enabled = false;
        yield return new WaitForSeconds(animStopTime);
        animator.enabled = true;
    }

    #region Sounds
    public void SimpleAttackSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.attackSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.attackSoundPath, transform);
        }
    }

    public void ComboAttackSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.comboAttackSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.comboAttackSoundPath, transform);
        }
    }

    public void AreaAttackSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.areaAttackSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.areaAttackSoundPath, transform);
        }
    }

    public void AreaAttackChargingSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.areaAttackChargingSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.areaAttackChargingSoundPath, transform);
        }
    }

    public void AreaAttackChargedSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.areaAttackChargedSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.areaAttackChargedSoundPath, transform);
        }
    }

    public void EnemyHitSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.enemyHitSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.enemyHitSoundPath, transform);
        }
    }

    public void ArmoredHitSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.enemyArmoredHitSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.enemyArmoredHitSoundPath, transform);
        }
    }

    public void WoodObjectHitSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.woodObjectHitSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.woodObjectHitSoundPath, transform);
        }
    }

    public void MetalObjectHitSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.metalObjectHitSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.metalObjectHitSoundPath, transform);
        }
    }

    #endregion
}
