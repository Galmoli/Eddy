using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private PlayerSwordScanner sword;
    //Player Input
    private InputActions _input;
    
    //Components
    private BasicAttack _basicAttack;
    
    //Local variables
    private bool _attack;

    private void Awake()
    {
        _input = new InputActions();
        _input.PlayerControls.Attack.started += Attack;
        _basicAttack = GetComponent<BasicAttack>();

        sword = FindObjectOfType<PlayerSwordScanner>();
    }

    private void Start()
    {
        //tempSword.gameObject.SetActive(false);
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
        if (!sword.activeScanner)
        {
            sword.gameObject.SetActive(false);
            if (_basicAttack.Attack().colliding)
            {
                if (_basicAttack.Attack().hitObject)
                {
                    _basicAttack.Attack().hitObject.GetComponent<EnemyBlackboard>().Hit(_basicAttack.damage);
                }
            }
            
            StartCoroutine(Co_Attack());
        }
    }

    private IEnumerator Co_Attack()
    {
        yield return new WaitForSeconds(0.2f);
        sword.gameObject.SetActive(true);
    }
}
