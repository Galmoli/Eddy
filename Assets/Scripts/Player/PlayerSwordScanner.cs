using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordScanner : MonoBehaviour
{
    private InputActions input;
    private PlayerMovementController playerMovement;

    public float scannerRadius;
    
    public Transform floorDetectionPoint;
    public float hitObjectDistance;
    public LayerMask stabSwordLayers;

    private Transform hand;
    private GameObject swordHolder;
    
    [HideInInspector] public bool activeScanner;

    private bool scannerInput;
    private ScannerIntersectionManager _scannerIntersectionManager;
    private SwordProgressiveColliders _swordProgressiveColliders;
    private PlayerInsideVolume _playerInsideVolume;
    private SphereCollider _sphereCollider;

    private Vector3 swordInitPos;
    private Quaternion swordInitRot;

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
        
        hand = transform.parent;
        
        activeScanner = false;
        scannerInput = false;

        transform.GetChild(0).localScale *= scannerRadius * 2f;
        _sphereCollider.radius = scannerRadius;

        swordInitPos = transform.localPosition;
        swordInitRot = transform.localRotation;

        input = new InputActions();
        input.Enable();

        input.PlayerControls.Scanner.started += ctx => ScannerOnInput();
        input.PlayerControls.Scanner.canceled += ctx => ScannerOffInput();
    }

    void Update()
    {
        //Sword
        if (input.PlayerControls.Sword.triggered && !playerMovement.inputMoveObject)
        {
            if(transform.parent == hand)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.root.position, transform.root.forward, out hit, hitObjectDistance, stabSwordLayers))
                {   
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Hide"))
                    {
                        if (!GetComponent<SphereCollider>().bounds.Contains(hit.point))
                        {
                            Stab(hit.collider.gameObject, false);
                            return;
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Appear"))
                    {
                        if (GetComponent<SphereCollider>().bounds.Contains(hit.point))
                        {
                            Stab(hit.collider.gameObject, false);
                            return;
                        }
                    }
                    else
                    {
                        Stab(hit.collider.gameObject, false);
                        return;
                    }                   
                }

                if(Physics.Raycast(floorDetectionPoint.position, -transform.root.up, out hit, 1.5f, stabSwordLayers))
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
    }

    private void ScannerOnInput()
    {
        scannerInput = true;
        if (transform.parent == hand && CanUseScanner())
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
        if (activeScanner && transform.parent == hand && !playerMovement.inputMoveObject)
        {
            ScannerOff();    
        }
    }

    private bool CanUseScanner()
    {
        return playerMovement.GetState().GetType() != typeof(EdgeState)
            && playerMovement.GetState().GetType() != typeof(CombatState)
            && playerMovement.GetState().GetType() != typeof(PushState);
    }

    public void ScannerOn()
    {
        if (!_playerInsideVolume.CanActivateScanner()) return;
        activeScanner = true;
        
        transform.GetChild(0).gameObject.SetActive(true);
        _sphereCollider.enabled = true;
        _swordProgressiveColliders.EnableSword();
    }

    public void ScannerOff()
    {
        if (!_playerInsideVolume.CanDisableScanner()) return;
        activeScanner = false;

        transform.GetChild(0).gameObject.SetActive(false);
        _sphereCollider.enabled = false;
        _swordProgressiveColliders.DisableSword();
        _scannerIntersectionManager.DeleteIntersections();
    }

    private void Stab(GameObject obj, bool vertical)
    {
        swordHolder = obj;
        transform.parent = null;

        //Harcoding
        if (vertical)
        {       
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (swordHolder.CompareTag("MoveObject"))
        {
            var moveObject = swordHolder.GetComponent<PushPullObject>();
            if(moveObject) moveObject.swordStabbed = true;
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
        
        if(activeScanner) _scannerIntersectionManager.CheckIntersections();
    }

    private void SwordBack()
    {
        if (!_playerInsideVolume.CanDisableScanner()) return;
        
        if (swordHolder.GetComponent<Switchable>() != null)
        {
            swordHolder.GetComponent<Switchable>().SwitchOff();
        }
        
        if(!scannerInput) ScannerOff();
        
        transform.parent = null;

        transform.parent = hand;
        transform.parent = hand;

        transform.localPosition = swordInitPos;
        transform.localRotation = swordInitRot;
    }

    public bool HoldingSword()
    {
        return transform.parent == hand;
    }

    public bool UsingScannerInHand()
    {
        return activeScanner && HoldingSword();
    } 
}
