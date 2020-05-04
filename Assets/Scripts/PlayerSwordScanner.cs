using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementController>();
        
        hand = transform.parent;
        
        activeScanner = false;
        scannerInput = false;

        transform.GetChild(0).localScale *= scannerRadius * 2f;
        GetComponent<SphereCollider>().radius = scannerRadius;

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
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SwordProgressiveColliders>().EnableSword();
    }

    private void ScannerOff()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<SwordProgressiveColliders>().DisableSword();
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
            transform.parent = swordHolder.transform;
        }

        if (swordHolder.GetComponent<Switchable>() != null)
        {
            swordHolder.GetComponent<Switchable>().SwitchOn();
        }      
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
