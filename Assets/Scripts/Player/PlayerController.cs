using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController _movementController;
    private PlayerCombatController _combatController;
    private PlayerSounds _playerSounds;
    [SerializeField] private float timeToRegenerate;

    public int initialHealth;
    public int health;

    private void Awake()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _combatController = GetComponent<PlayerCombatController>();
        _playerSounds = GetComponent<PlayerSounds>();
        UIManager.OnHeal += Heal;
    }

    private void Start()
    {
        health = initialHealth;
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
        health -= damage;
        if (health <= 0)
        {
            //Trigger Death Animation
            //

            if (AudioManager.Instance.ValidEvent(_playerSounds.deathSoundPath))
            {
                AudioManager.Instance.PlayOneShotSound(_playerSounds.deathSoundPath, transform);
            }

            SetDeadState();
            StartCoroutine(UIManager.Instance.ShowDeathMenu());
        }
        else
        {
            float number = UnityEngine.Random.Range(1, 4);
            _movementController.animator.SetTrigger("Hit" + number.ToString());
            StopAllCoroutines();
            StartCoroutine(Co_Heal());
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
        _movementController.Spawn();
        _movementController.SetState(new MoveState(_movementController));
        _combatController.SetState(new IdleState(_combatController));
        RestoreHealth();
    }

    public void RestoreHealth()
    {
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
}
