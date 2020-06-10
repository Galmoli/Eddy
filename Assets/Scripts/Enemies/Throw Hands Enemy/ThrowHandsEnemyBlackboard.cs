using Steerings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ThrowHandsEnemyDeathFSM))]

public class ThrowHandsEnemyBlackboard : EnemyBlackboard
{
    public GameObject ragdoll;
    public Text statesText;

    [Header("General Stats")]
    public bool armored;
    public float initialHealthPoints;
    public float attackPoints;
    public float rotationSpeed;

    [Header("Enemy Passive")]
    public GameObject initialTransform;
    public bool canWander;
    public float wanderTime;
    public float idleTime;
    public float wanderSpeed;
    public LayerMask sightObstaclesLayers;

    [Header("Enemy Agressive")]
    public float enemyColliderChaseHeight;
    public float maxVerticalDistance;
    public float detectionDistanceOnSight;
    public float detectionDistanceOffSight;
    public float playerOutOfRangeDistance;
    public float visionAngle;
    public float timeInNotice;
    public float chasingSpeed;
    public float attackRange;
    public float timeAfterAttacks;
    public float damageZoneRadius;
    public Transform attackPoint;

    [Header("Enemy Stun")]
    public float stunnedTime;
    public float stunImpulse;

    [Header("Enemy Stagger")]
    public float staggerImpulse;
    public float staggeredTime;

    [Header("Enemy Death")]
    public float minDeathImpulse = 4000;
    public float maxDeathImpulse = 6000;

    [Header("Arrive Steering Variables")]
    public float closeEnoughRadius;
    public float slowDownRadius;

    [Header("Avoidance Steering Variables")]
    public float lookAheadLength;
    public float avoidDistance;
    public float secondaryWhiskerAngle;
    public float secondaryWhiskerRatio;
    public LayerMask avoidLayers;

    [HideInInspector] public SphereCollider scannerSphereCollider;

    [Header("Wander Steering Variables")]
    public float wanderRate;
    public float wanderRadius;
    public float wanderOffset;

    //Other variables
    private bool checkingInVolumeScannerOn;
    private bool checkingInVolumeScannerOff;

    [Header("Sound")]
    public string noticeSoundPath;
    public string stepSoundPath;
    public string attackSoundPath;
    public string deathSoundPath;

    public override void Start()
    {
        GameManager.Instance.enemySpawnManager.Add(this);

        player = FindObjectOfType<PlayerMovementController>();
        playerCombatController = FindObjectOfType<PlayerCombatController>();
        swordScanner = FindObjectOfType<PlayerSwordScanner>();

        scannerSphereCollider = FindObjectOfType<PlayerSwordScanner>().GetComponent<SphereCollider>();

        rb = GetComponent<Rigidbody>();
        ownKS = GetComponent<KinematicState>();
        col = GetComponent<CapsuleCollider>();

        /*if (respawnable)
            GameManager.Instance.enemySpawnManager.Add(this);
        else
            GameManager.Instance.nonRespawnableEnemies.Add(gameObject);*/

        healthPoints = initialHealthPoints;

        stunned = false;
        hit = false;
        dead = false;

        if (GetComponent<ArrivePlusAvoid>() != null)
        {
            SetArrivePlusAvoidVariables();
        }

        if (GetComponent<WanderPlusAvoid>() != null)
        {
            SetWanderPlusAvoidVariables();
        }

        initialTransform.transform.parent = null;

        checkingInVolumeScannerOn = false;
        checkingInVolumeScannerOff = false;
    }

    public override void Update()
    {
        statesText.transform.parent.transform.LookAt(Camera.main.transform.position);
    }

    public override void OnDestroy()
    {
        Destroy(initialTransform);
    }

    public override void Hit(int damage, Vector3 hitDirection)
    {
        this.hitDirection = hitDirection;
        hit = true;
        healthPoints -= damage;
    }

    public override void ResetHealth()
    {
        healthPoints = initialHealthPoints;
    }

    void SetArrivePlusAvoidVariables()
    {
        ArrivePlusAvoid arrivePlusVoid = GetComponent<ArrivePlusAvoid>();

        arrivePlusVoid.closeEnoughRadius = closeEnoughRadius;
        arrivePlusVoid.slowDownRadius = slowDownRadius;

        arrivePlusVoid.lookAheadLength = lookAheadLength;
        arrivePlusVoid.avoidDistance = avoidDistance;
        arrivePlusVoid.secondaryWhiskerAngle = secondaryWhiskerAngle;
        arrivePlusVoid.secondaryWhiskerRatio = secondaryWhiskerRatio;
        arrivePlusVoid.avoidLayers = avoidLayers;
        arrivePlusVoid.scanner = scannerSphereCollider;
    }

    void SetWanderPlusAvoidVariables()
    {
        WanderPlusAvoid wanderPlusAvoid = GetComponent<WanderPlusAvoid>();

        wanderPlusAvoid.wanderRate = wanderRate;
        wanderPlusAvoid.wanderRadius = wanderRadius;
        wanderPlusAvoid.wanderOffset = wanderOffset;

        wanderPlusAvoid.lookAheadLength = lookAheadLength;
        wanderPlusAvoid.avoidDistance = avoidDistance;
        wanderPlusAvoid.secondaryWhiskerAngle = secondaryWhiskerAngle;
        wanderPlusAvoid.secondaryWhiskerRatio = secondaryWhiskerRatio;
        wanderPlusAvoid.avoidLayers = avoidLayers;
        wanderPlusAvoid.scanner = scannerSphereCollider;
    }

    public override bool CanBeDamaged()
    {
        return !armored || InScanner();
    }

    private bool InScanner()
    {
        return swordScanner.activeScanner && scannerSphereCollider.bounds.Contains(transform.position);
    }

    public override void Death()
    {
        Vector3 dir = transform.position - player.transform.position;
        dir = dir.normalized;

        GameObject rd = Instantiate(ragdoll, transform.position, Quaternion.identity);
        rd.transform.rotation = transform.rotation;

        float deathImpulse = Random.Range(minDeathImpulse, maxDeathImpulse);
        rd.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(dir * deathImpulse);
        gameObject.SetActive(false);

        dead = true;
        DeathSound();
    }
    
    public override void AnimStop()
    {
        StartCoroutine(Co_AnimStop());
    }

    private IEnumerator Co_AnimStop()
    {
        animator.enabled = false;
        yield return new WaitForSeconds(playerCombatController.animStopTime);
        animator.enabled = true;
    }

    public override void EnemyInVolume(bool scannerOn)
    {
        if (scannerOn) checkingInVolumeScannerOn = true;
        else checkingInVolumeScannerOff = true;

        StartCoroutine(CheckingInVolumeCoroutine());
    }

    private IEnumerator CheckingInVolumeCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        checkingInVolumeScannerOn = false;
        checkingInVolumeScannerOff = false;
    }

    public override void OnCollisionStay(Collision other)
    {
        if (checkingInVolumeScannerOff && other.gameObject.layer == LayerMask.NameToLayer("Hide"))
        {
            healthPoints = 0;
        }

        if (checkingInVolumeScannerOn && other.gameObject.layer == LayerMask.NameToLayer("Appear"))
        {
            healthPoints = 0;
        }
    }

    #region Sounds
    public void AttackSound()
    {
        if (AudioManager.Instance.ValidEvent(attackSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(attackSoundPath, transform);
        }
    }

    public void NoticeSound()
    {
        if (AudioManager.Instance.ValidEvent(noticeSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(noticeSoundPath, transform);
        }
    }

    public void DeathSound()
    {
        if (AudioManager.Instance.ValidEvent(deathSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(deathSoundPath, transform);
        }
    }

    public override void StepSound()
    {
        if (AudioManager.Instance.ValidEvent(stepSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(stepSoundPath, transform);
        }
    }
    #endregion
}
