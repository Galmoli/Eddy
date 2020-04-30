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

    private Transform hand;
    private GameObject swordHolder;
    
    [HideInInspector] public bool activeScanner;
    private bool scannerInput;

    private GameObject[] hiddenObjects;
    private GameObject[] hideableObjects;

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

        hiddenObjects = FindObjectsInLayer(LayerMask.NameToLayer("HiddenObjects"));
        hideableObjects = FindObjectsInLayer(LayerMask.NameToLayer("HideableObjects"));

        for (int i = 0; i < hiddenObjects.Length; i++)
        {
            Hide(hiddenObjects[i]);
        }
    }

    void Update()
    {
        //Sword
        if (input.PlayerControls.Sword.triggered && !playerMovement._inputMoveObject)
        {
            if(transform.parent == hand)
            {
                RaycastHit hit;
                if (Physics.Raycast(hand.transform.position, transform.forward, out hit, hitObjectDistance))
                {
                    Stab(hit.collider.gameObject, false);
                }
                else if(Physics.Raycast(floorDetectionPoint.position, -transform.up, out hit, 0.2f))
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
                hiddenObjects = FindObjectsInLayer(LayerMask.NameToLayer("HiddenObjects"));
                hideableObjects = FindObjectsInLayer(LayerMask.NameToLayer("HideableObjects"));

                activeScanner = true;

                transform.GetChild(0).gameObject.SetActive(true);
                GetComponent<SphereCollider>().enabled = true;
            }          
        }
        else if (activeScanner && transform.parent == hand && !playerMovement._inputMoveObject)
        {
            ScannerOff();

            activeScanner = false;
        }
    }

    private GameObject[] FindObjectsInLayer(int layerIdx)
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();

        List<GameObject> objectsInLayer = new List<GameObject>();

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].layer.Equals(layerIdx))
            {
                objectsInLayer.Add(objects[i]);
            }
        }

        return objectsInLayer.ToArray();
    }

    private void ScannerOff()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;

        for (int i = 0; i < hiddenObjects.Length; i++)
        {
                Hide(hiddenObjects[i]);
        }

        for (int i = 0; i < hideableObjects.Length; i++)
        {
            Show(hideableObjects[i]);
        }
        
    }
    
    private void Show(GameObject go)
    {
        if (go.GetComponent<MeshRenderer>() != null)
            if(go.GetComponent<MeshRenderer>().enabled == false)
                go.GetComponent<MeshRenderer>().enabled = true;

        if (go.GetComponent<Collider>() != null)
            if (go.GetComponent<Collider>().isTrigger == true)
                go.GetComponent<Collider>().isTrigger = false;  
    }

    private void Hide(GameObject go)
    {
        if(go.GetComponent<MeshRenderer>() != null)
            if(go.GetComponent<MeshRenderer>().enabled == true)
                go.GetComponent<MeshRenderer>().enabled = false;

        if (go.GetComponent<Collider>() != null)
            if (go.GetComponent<Collider>().isTrigger == false)
                go.GetComponent<Collider>().isTrigger = true;
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
            //
        }

        if (swordHolder.tag == "MoveObject")
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
        //

        transform.parent = hand;
        transform.parent = hand;
    }

    void OnTriggerEnter(Collider other)
    {
        if (scannerInput)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("HiddenObjects"))
            {
                Show(other.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("HideableObjects"))
            {
                Hide(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (scannerInput)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("HiddenObjects"))
            {
                Hide(other.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("HideableObjects"))
            {
                Show(other.gameObject);
            }
        }
    }
}
