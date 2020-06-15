using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using FMOD.Studio;

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
    [HideInInspector] public bool nextSpinAttackReserved;
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
    public SkinnedMeshRenderer meshRenderer;
    [HideInInspector] public Material iniMeshMat;
    public Material damagedMeshMat;

    private EventInstance areaAttackChargingSoundEvent_1;
    private EventInstance areaAttackChargingSoundEvent_2;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.Attack.started += ctx => SimpleAttack(false);
        //_input.PlayerControls.Attack.canceled += ctx => InputRelease();
        _input.PlayerControls.SpinAttack.started += ctx => SpinAttack();
        _input.PlayerControls.SpinAttack.canceled += ctx => InputRelease();
        
        SetState(new IdleState(this));
        swordTrigger.OnHit += StateInteract;
        
        sword = FindObjectOfType<PlayerSwordScanner>();
        _movementController = GetComponent<PlayerMovementController>();
        playerSounds = GetComponent<PlayerSounds>();

    }

    private void Start()
    {
        swordTrigger.DisableTrigger();
        iniMeshMat = meshRenderer.material;
    }

    private void Update()
    {
        state.Update();

        Debug.Log(nextSpinAttackReserved);
        if (nextAttackReserved && state.GetType() == typeof(IdleState)) SimpleAttack(true);
        else if (nextSpinAttackReserved && state.GetType() == typeof(IdleState))
        {
            nextSpinAttackReserved = false;
            SpinAttack();
        }
    }

    public void SpinAttack()
    {
        if (!sword.HoldingSword()) return;
        if (_movementController.GetState().GetType() != typeof(MoveState) && _movementController.GetState().GetType() != typeof(CombatState)) return;

        if (state.GetType() == typeof(SimpleAttackState))
        {
            nextSpinAttackReserved = true;
            return;
        }

        StartCoroutine(ChargeCounter());
    }

    private void SimpleAttack(bool auto)
    {
        if (!sword.HoldingSword()) return;
        if (_movementController.GetState().GetType() != typeof(MoveState) && _movementController.GetState().GetType() != typeof(CombatState)) return;
        if (state.GetType() == typeof(IdleChargedState) || state.GetType() == typeof(AreaAttackState)) return;

        //if (_chargeCoroutine != null && !auto) StopCoroutine(_chargeCoroutine);

        if (state.GetType() == typeof(SimpleAttackState) && !auto)
        {
            if (simpleAttackCount == 0)
            {
                nextAttackReserved = false;
                return;
            }
            //_chargeCoroutine = StartCoroutine(ChargeCounter());
            nextAttackReserved = true;
            return;
        }
        
        if (_comboCoroutine != null) StopCoroutine(_comboCoroutine);


        SetState(new SimpleAttackState(this));
        _comboCoroutine = StartCoroutine(ComboCounter());
        nextAttackReserved = false;
        //if(!auto) _chargeCoroutine = StartCoroutine(ChargeCounter());
    }

    private void InputRelease()
    {
        /*if (state.GetType() == typeof(SimpleAttackState))
        {
            //animator.SetBool("isChargingAttack", false);
            //StopCoroutine(_chargeCoroutine);
            return;
        }*/

        nextSpinAttackReserved = false;

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

    public void SetMovementControllerCombatState()
    {
        _movementController.SetState(new CombatState(_movementController, this));
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
    public void SimpleAttackSound_1()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.attackSoundPath_1))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.attackSoundPath_1, transform);
        }
    }

    public void SimpleAttackSound_2()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.attackSoundPath_2))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.attackSoundPath_2, transform);
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

    public void AreaAttackChargingSound_1()
    {
        if (!AudioManager.Instance.isPlaying(areaAttackChargingSoundEvent_1))
        {
            if (AudioManager.Instance.ValidEvent(playerSounds.areaAttackChargingSoundPath_1))
            {
                areaAttackChargingSoundEvent_1 = AudioManager.Instance.PlayEvent(playerSounds.areaAttackChargingSoundPath_1, transform);
            }
        }
    }

    public void StopAreaAttackChargingSound_1()
    {
        areaAttackChargingSoundEvent_1.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void AreaAttackChargingSound_2()
    {
        if (!AudioManager.Instance.isPlaying(areaAttackChargingSoundEvent_2))
        {
            if (AudioManager.Instance.ValidEvent(playerSounds.areaAttackChargingSoundPath_2))
            {
                areaAttackChargingSoundEvent_2 = AudioManager.Instance.PlayEvent(playerSounds.areaAttackChargingSoundPath_2, transform);
            }
        }
    }

    public void StopAreaAttackChargingSound_2()
    {
        areaAttackChargingSoundEvent_2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
