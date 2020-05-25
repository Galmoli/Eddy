using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerSwordScanner : MonoBehaviour
{
    private InputActions input;
    private PlayerMovementController playerMovement;

    public float scannerRadius;

  
    public Transform floorDetectionPoint;
    public float hitObjectDistance;
    public LayerMask stabSwordLayers;

    [Header("MOVE AWAY TO STAB")]
    public float moveAwayDistance;
    public float moveAwaySpeed;
    public float moveAwayTime;

    private Transform playerHand;
    private GameObject swordHolder;

    [HideInInspector] public bool activeScanner;

    private bool scannerInput;
    private bool swordUnlocked;
    private ScannerIntersectionManager _scannerIntersectionManager;
    private SwordProgressiveColliders _swordProgressiveColliders;
    private PlayerInsideVolume _playerInsideVolume;
    private SphereCollider _sphereCollider;

    private Vector3 swordInitPos;
    private Quaternion swordInitRot;

    private bool movingAwayToStab;
    private Vector3 moveAwayVector;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _swordProgressiveColliders = GetComponent<SwordProgressiveColliders>();
        _scannerIntersectionManager = GetComponent<ScannerIntersectionManager>();
        _playerInsideVolume = GameObject.Find("Player").GetComponent<PlayerInsideVolume>();
    }

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementController>();

        playerHand = transform.parent;
        swordInitPos = transform.localPosition;
        swordInitRot = transform.localRotation;

        swordUnlocked = false;
        activeScanner = false;
        scannerInput = false;
        movingAwayToStab = false;

        transform.GetChild(0).localScale *= scannerRadius * 2f;
        _sphereCollider.radius = scannerRadius;

        input = new InputActions();
        input.Enable();

        input.PlayerControls.Scanner.started += ctx => ScannerOnInput();
        input.PlayerControls.Scanner.canceled += ctx => ScannerOffInput();
    }

    void Update()
    {
        //Shortcut
        if (Input.GetKeyDown(KeyCode.Return) && !swordUnlocked)
        {
            UnlockSword();
        }
        //

        //Sword
        if (input.PlayerControls.Sword.triggered && CanStab() && swordUnlocked)
        {
            if (HoldingSword())
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.root.position, transform.root.forward, out hit, hitObjectDistance, stabSwordLayers))
                {   
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Hide"))
                    {
                        if (!GetComponent<SphereCollider>().bounds.Contains(hit.point))
                        {
                            DistanceToStab(playerMovement.transform.position, hit.point);
                            Stab(hit.collider.gameObject, false);
                            return;
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Appear"))
                    {
                        if (GetComponent<SphereCollider>().bounds.Contains(hit.point))
                        {
                            DistanceToStab(playerMovement.transform.position, hit.point);
                            Stab(hit.collider.gameObject, false);
                            return;
                        }
                    }
                    else
                    {
                        DistanceToStab(playerMovement.transform.position, hit.point);
                        Stab(hit.collider.gameObject, false);
                        return;
                    }                   
                }

                if(Physics.Raycast(floorDetectionPoint.position, -transform.root.up, out hit, playerMovement.characterController.height / 2 + 0.5f, stabSwordLayers))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Hide"))
                    {
                        if (!GetComponent<SphereCollider>().bounds.Contains(hit.point))
                        {
                            Stab(hit.collider.gameObject, true);
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Appear"))
                    {
                        if (GetComponent<SphereCollider>().bounds.Contains(hit.point))
                        {
                            Stab(hit.collider.gameObject, true);
                        }
                    }
                    else
                    {
                        Stab(hit.collider.gameObject, true);
                    }
                }
            }
            else
            {
                SwordBack();
            }       
        }

        //animation
        if (HoldingSword()) playerMovement.animator.SetBool("isHoldingSword", true);
        else playerMovement.animator.SetBool("isHoldingSword", false);
    }

    private void LateUpdate()
    {
        if (movingAwayToStab)
        {
            playerMovement.characterController.Move(moveAwayVector * Time.deltaTime * moveAwaySpeed);
        }
    }

    private void DistanceToStab(Vector3 playerPos, Vector3 hitPointPos)
    {
        if(Vector3.Distance(playerPos, hitPointPos) < moveAwayDistance)
        {
            moveAwayVector = (playerPos - hitPointPos).normalized;
            StartCoroutine(MoveAway());
        }
    }

    private IEnumerator MoveAway()
    {
        movingAwayToStab = true;
        yield return new WaitForSeconds(moveAwayTime);
        movingAwayToStab = false;
    }

    private void ScannerOnInput()
    {
        scannerInput = true;
        if (HoldingSword() && CanUseScanner())
        {
            if (!activeScanner)
            {
                ScannerOn();
            }
        }
    }

    private void ScannerOffInput()
    {
        scannerInput = false;
        if (activeScanner && HoldingSword() && !playerMovement.inputMoveObject)
        {
            ScannerOff();    
        }
    }

    public void ScannerOn()
    {
        if (!_playerInsideVolume.CanActivateScanner()) return;

        EnemyBlackboard[] enemies = GameObject.FindObjectsOfType<EnemyBlackboard>();

        foreach (EnemyBlackboard enemy in enemies)
        {
            enemy.EnemyInVolume(true);
        }

        activeScanner = true;
        
        transform.GetChild(0).gameObject.SetActive(true);
        _sphereCollider.enabled = true;
        _swordProgressiveColliders.EnableSword();

        playerMovement.animator.SetBool("isUsingScanner", true);
    }

    public void ScannerOff()
    {
        if (!_playerInsideVolume.CanDisableScanner()) return;

        EnemyBlackboard[] enemies = GameObject.FindObjectsOfType<EnemyBlackboard>();

        foreach (EnemyBlackboard enemy in enemies)
        {
            enemy.EnemyInVolume(false);
        }

        activeScanner = false;

        transform.GetChild(0).gameObject.SetActive(false);
        _sphereCollider.enabled = false;
        _swordProgressiveColliders.DisableSword();
        _scannerIntersectionManager.DeleteIntersections();

        playerMovement.animator.SetBool("isUsingScanner", false);
    }

    private void Stab(GameObject obj, bool vertical)
    {
        if (vertical) playerMovement.animator.SetTrigger("NailDown");
        else playerMovement.animator.SetTrigger("NailForward");
        swordHolder = obj;
        playerMovement.SetState(new StabSwordState(playerMovement));
    }

    public void FinishStab()
    {
        transform.parent = null;

        if (swordHolder.CompareTag("MoveObject"))
        {
            var moveObject = swordHolder.GetComponent<PushPullObject>();
            if (moveObject) moveObject.swordStabbed = true;
            else
            {
                moveObject = swordHolder.transform.parent.gameObject.GetComponent<PushPullObject>();
                moveObject.swordStabbed = true;
            }
        }

        if (swordHolder.CompareTag("CheckPoint"))
        {
            CheckPoint c = swordHolder.GetComponent<CheckPoint>();
            c.Activate();
        }

        transform.parent = swordHolder.transform;

        if (swordHolder.GetComponent<Switchable>() != null)
        {
            swordHolder.GetComponent<Switchable>().SwitchOn();
        }

        if (activeScanner) _scannerIntersectionManager.CheckIntersections();
    }

    private void SwordBack()
    {
        if (!_playerInsideVolume.CanDisableScanner()) return;

        EnemyBlackboard[] enemies = GameObject.FindObjectsOfType<EnemyBlackboard>();

        foreach (EnemyBlackboard enemy in enemies)
        {
            enemy.EnemyInVolume(false);
        }

        if (swordHolder.GetComponent<Switchable>() != null)
        {
            swordHolder.GetComponent<Switchable>().SwitchOff();
        }
        
        if(!scannerInput) ScannerOff();
        
        transform.parent = null;

        transform.parent = playerHand;
        transform.parent = playerHand;

        transform.localPosition = swordInitPos;
        transform.localRotation = swordInitRot;
    }

    public void UnlockSword()
    {
        GetComponent<MeshRenderer>().enabled = true;
        swordUnlocked = true;
        Debug.Log("Sword Unlocked");
    }

    private bool CanUseScanner()
    {
        return playerMovement.GetState().GetType() != typeof(EdgeState)
            && playerMovement.GetState().GetType() != typeof(PushState);
    }


    private bool CanStab()
    {
        return playerMovement.GetState().GetType() != typeof(EdgeState)
            && playerMovement.GetState().GetType() != typeof(CombatState)
            && playerMovement.GetState().GetType() != typeof(PushState);
    }

    public bool HoldingSword()
    {
        return transform.parent == playerHand && swordUnlocked;
    }

    public bool UsingScannerInHand()
    {
        return activeScanner && HoldingSword();
    } 
}
