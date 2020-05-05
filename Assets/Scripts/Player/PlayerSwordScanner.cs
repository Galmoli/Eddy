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
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _swordProgressiveColliders = GetComponent<SwordProgressiveColliders>();
        _scannerIntersectionManager = GetComponent<ScannerIntersectionManager>();
    }

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementController>();
        
        hand = transform.parent;
        
        activeScanner = false;
        scannerInput = false;

        transform.GetChild(0).localScale *= scannerRadius * 2f;
        _sphereCollider.radius = scannerRadius;

        input = new InputActions();
        input.Enable();

        input.PlayerControls.Scanner.started += ctx => scannerInput = true;
        input.PlayerControls.Scanner.canceled += ctx => scannerInput = false;
    }

    void Update()
    {
        //Sword
        if (input.PlayerControls.Sword.triggered && !playerMovement._inputMoveObject)
        {
            if(transform.parent == hand)
            {
                RaycastHit hit;
                if (Physics.Raycast(hand.transform.position, transform.forward, out hit, hitObjectDistance, stabSwordLayers))
                {  
                    Stab(hit.collider.gameObject, false);
                }
                else if(Physics.Raycast(floorDetectionPoint.position, -transform.up, out hit, 0.2f, stabSwordLayers))
                {
                    Stab(hit.collider.gameObject, true);
                }
            }
            else
            {
                SwordBack();
            }       
        }

        //Scanner
        if (scannerInput && transform.parent == hand && !playerMovement._inputMoveObject)
        {
            if (!activeScanner)
            {
                ScannerOn();
            }          
        }
        else if (activeScanner && transform.parent == hand && !playerMovement._inputMoveObject)
        {
            ScannerOff();

            activeScanner = false;
        }
    }

    private void ScannerOn()
    {
        activeScanner = true;
        
        transform.GetChild(0).gameObject.SetActive(true);
        _sphereCollider.enabled = true;
        _swordProgressiveColliders.EnableSword();
    }

    private void ScannerOff()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        _sphereCollider.enabled = false;
        _swordProgressiveColliders.DisableSword();
        _scannerIntersectionManager.DeleteIntersections();
    }

    private void Stab(GameObject obj, bool vertical)
    {
        swordHolder = obj;
        transform.parent = null;

        if (vertical)
        {
            //hardcoding        
            transform.eulerAngles = new Vector3(90, 0, 0);
            transform.position -= new Vector3(0, 1f, 0);
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
        
        transform.parent = swordHolder.transform;

        if (swordHolder.GetComponent<Switchable>() != null)
        {
            swordHolder.GetComponent<Switchable>().SwitchOn();
        }
        
        if(activeScanner) _scannerIntersectionManager.CheckIntersections();
    }

    private void SwordBack()
    {
        if (swordHolder.GetComponent<Switchable>() != null)
        {
            swordHolder.GetComponent<Switchable>().SwitchOff();
        }
        
        transform.parent = null;

        //hardcoding
        transform.rotation = hand.rotation;
        transform.position = hand.position;

        transform.parent = hand;
        transform.parent = hand;
    }

    public bool UsingScannerInHand()
    {
        return activeScanner && transform.parent == hand;
    }
}
