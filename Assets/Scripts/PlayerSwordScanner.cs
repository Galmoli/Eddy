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
                if (Physics.Raycast(hand.transform.position, transform.forward, out hit, hitObjectDistance) && hit.collider.gameObject.GetComponent<MeshRenderer>() != null)
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

        GameObject[] hiddenObjectsInScanner = FindObjectsInLayer(LayerMask.NameToLayer("HiddenObjectsInScanner"));
        GameObject[] hideableObjectsInScanner = FindObjectsInLayer(LayerMask.NameToLayer("HideableObjectsInScanner"));

        for (int i = 0; i < hiddenObjectsInScanner.Length; i++)
        {
                Hide(hiddenObjectsInScanner[i]);
        }

        for (int i = 0; i < hideableObjectsInScanner.Length; i++)
        {
            Show(hideableObjectsInScanner[i]);
        }
        
    }
    
    private void Show(GameObject go)
    {
        if (go.GetComponent<MeshRenderer>() != null)
            if(!go.GetComponent<MeshRenderer>().enabled)
                go.GetComponent<MeshRenderer>().enabled = true;

        if (go.layer == LayerMask.NameToLayer("HiddenObjects"))
            go.layer = LayerMask.NameToLayer("HiddenObjectsInScanner");
        else if (go.layer == LayerMask.NameToLayer("HideableObjectsInScanner"))
            go.layer = LayerMask.NameToLayer("HideableObjects");

        /*if (go.GetComponent<Rigidbody>() != null)
            if (go.GetComponent<Rigidbody>().isKinematic)
                go.GetComponent<Rigidbody>().isKinematic = false;

        if (go.GetComponent<Collider>() != null)
            if (go.GetComponent<Collider>().isTrigger)
                go.GetComponent<Collider>().isTrigger = false;*/

        if (go.tag == "MoveObject")
            go.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Hide(GameObject go)
    {
        if(go.GetComponent<MeshRenderer>() != null)
            if(go.GetComponent<MeshRenderer>().enabled)
                go.GetComponent<MeshRenderer>().enabled = false;

        if (go.layer == LayerMask.NameToLayer("HiddenObjectsInScanner"))
            go.layer = LayerMask.NameToLayer("HiddenObjects");
        else if(go.layer == LayerMask.NameToLayer("HideableObjects"))
            go.layer = LayerMask.NameToLayer("HideableObjectsInScanner");

        /*if (go.GetComponent<Rigidbody>() != null)
            if (!go.GetComponent<Rigidbody>().isKinematic)
                go.GetComponent<Rigidbody>().isKinematic = true;

        if (go.GetComponent<Collider>() != null)
            if (!go.GetComponent<Collider>().isTrigger)
                go.GetComponent<Collider>().isTrigger = true;*/

        if (go.tag == "MoveObject")
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.GetComponent<PushPullObject>().canMove = false;
        }     
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
        if (activeScanner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("HiddenObjects"))
            {
                Show(other.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("HideableObjects") || other.gameObject.layer == LayerMask.NameToLayer("HiddenObjectsInScanner"))
            {
                Hide(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (activeScanner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("HiddenObjectsInScanner"))
            {
                Hide(other.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("HideableObjectsInScanner"))
            {
                Show(other.gameObject);
            }
        }
    }

    public bool UsingScannerInHand()
    {
        return activeScanner && transform.parent == hand;
    }
}
