using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController _movementController;
    private PlayerCombatController _combatController;
    private PlayerSounds _playerSounds;
    [SerializeField] private float timeToRegenerate;
    [SerializeField] private float timeToStartRegeneration;

    public int initialHealth;
    public int health;
    private bool _isDead;

    private void Awake()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _combatController = GetComponent<PlayerCombatController>();
        _playerSounds = GetComponent<PlayerSounds>();
        UIManager.OnHeal += Heal;
        SceneManager.activeSceneChanged += EnablePlayer;
    }

    private void Start()
    {
        UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Move, transform.position + Vector3.up*2, transform);
        health = initialHealth;
        _isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Provisional to trigger death state
        if (Input.GetKeyDown(KeyCode.K))
        {
            Hit(3);
        }   
    }

    public void Hit(int damage)
    {
        if (_isDead) return;
        health -= damage;
        if (health <= 0)
        {
            if (AudioManager.Instance.ValidEvent(_playerSounds.deathSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(_playerSounds.deathSoundPath, transform);
            }

            SetDeadState();
            _isDead = true;
            StartCoroutine(UIManager.Instance.ShowDeathMenu());
            UIManager.Instance.Hit(damage);
        }
        else
        {
            float number = UnityEngine.Random.Range(1, 4);
            if(!_combatController.IsAttacking()) _movementController.animator.SetTrigger("Hit" + number);
            StopAllCoroutines();
            StartCoroutine(Co_Regenerate());
            UIManager.Instance.Hit(damage);

            if (AudioManager.Instance.ValidEvent(_playerSounds.damageReceivedSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(_playerSounds.damageReceivedSoundPath, transform);
            }
        }
    }

    private void Heal()
    {
        health++;
        UIManager.Instance.Heal();
    }

    private void SetDeadState()
    {
        _movementController.SetState(new DeadState(_movementController));
        _combatController.SetState(new CombatDeadState());
    }

    public void Spawn()
    {
        _isDead = false;
        _movementController.Spawn();
        _movementController.SetState(new MoveState(_movementController));
        _combatController.SetState(new IdleState(_combatController));
        RestoreHealth();
    }

    public void RestoreHealth()
    {
        _movementController.animator.SetTrigger("Revive");
        health = initialHealth;
        UIManager.Instance.RestoreHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeadZone")
        {
            SetDeadState();
            StartCoroutine(UIManager.Instance.ShowDeathMenu());
        }
    }

    private IEnumerator Co_Regenerate()
    {
        yield return new WaitForSeconds(timeToStartRegeneration);
        StartCoroutine(Co_Heal());
    }
    
    private IEnumerator Co_Heal()
    {
        var currentTime = 0f;
        while (health < initialHealth)
        {
            if (currentTime < timeToRegenerate)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                Heal();
            }
            yield return null;
        }
    }

    private void EnablePlayer(Scene p, Scene n)
    {
        if (n.buildIndex != GameManager.Instance.checkpointSceneIndex) return;
        var cc = GetComponent<CharacterController>();
        if (!cc.enabled) cc.enabled = true;
    }
}
