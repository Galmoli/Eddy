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

    [Header("CameraShake")]
    private CameraShake cameraShake;
    public float damagedShake = 0.5f;
    public float deadShake = 2;

    private void Awake()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _combatController = GetComponent<PlayerCombatController>();
        _playerSounds = GetComponent<PlayerSounds>();
        UIManager.OnHeal += Heal;
        SceneManager.activeSceneChanged += EnablePlayer;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= EnablePlayer;
    }

    private void Start()
    {       
        health = initialHealth;
        _isDead = false;
        cameraShake = FindObjectOfType<CameraShake>();
    }

    public void Hit(int damage)
    {
        if (_isDead) return;
        health -= damage;
        
        if (health <= 0)
        {
            PlayDeathSound();

            SetDeadState();
            _isDead = true;
            StartCoroutine(UIManager.Instance.ShowDeathMenu());
            StartCoroutine("PlayDamaged");
            if (cameraShake != null) cameraShake.ShakeCamera(deadShake, 0.2f);
            UIManager.Instance.Hit(damage);
        }
        else
        {
            float number = UnityEngine.Random.Range(1, 4);
            if (!_combatController.IsAttacking())
            {
                VibrationManager.Instance.Vibrate(VibrationManager.Presets.NORMAL_HIT);
                _movementController.animator.SetTrigger("Hit" + number);
            }
            else
            {
               
            }
            StopAllCoroutines();
            StartCoroutine(Co_Regenerate());
            StartCoroutine("PlayDamaged");
            if (cameraShake != null) cameraShake.ShakeCamera(damagedShake, 0.2f);
            UIManager.Instance.Hit(damage);

            PlayDamageSound();
        }
    }

    IEnumerator PlayDamaged()
    {
        _combatController.damagedVFX.transform.localEulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 70), 0);
        _combatController.damagedVFX.Play();
        _combatController.meshRenderer.material = _combatController.damagedMeshMat;
        yield return new WaitForSeconds(_combatController.timerDamagedVFX);
        _combatController.damagedVFX.Stop();
        _combatController.meshRenderer.material = _combatController.iniMeshMat;
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
        _combatController.animator.gameObject.SetActive(true);
        CameraController cameraController = GameObject.FindObjectOfType<CameraController>();
        //cameraController.target = _movementController.transform;
        cameraController.enabled = true;
        HeadLookAt cameraLook = cameraController.gameObject.GetComponent<HeadLookAt>();
        cameraLook.enabled = false;
        FovController fovController = GameObject.FindObjectOfType<FovController>();
        fovController.activated = true;
        fovController.isGoingToMin = false;
        if (_movementController.rag != null) Destroy(_movementController.rag);
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
        FindObjectOfType<CameraController>().SetPositionImmediately();
    }

    #region Sounds
    private void PlayDamageSound()
    {
        if (AudioManager.Instance.ValidEvent(_playerSounds.damageReceivedSoundPath))
        {
            AudioManager.Instance.PlayEvent(_playerSounds.damageReceivedSoundPath, transform);
        }
    }

    private void PlayDeathSound()
    {
        if (AudioManager.Instance.ValidEvent(_playerSounds.deathSoundPath))
        {
            AudioManager.Instance.PlayEvent(_playerSounds.deathSoundPath, transform);
        }
    }
    #endregion
}
