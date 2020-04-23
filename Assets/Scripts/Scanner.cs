using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float detectionRadius;

    public GameObject sword;
    public Transform hand;
    public Transform floorDetectionPoint;
    public float hitObjectDistance;
    private GameObject swordHolder;
    

    [Header("Layers")] 
    public int hiddenObjectsLayer;
    public int hideableObjectsLayer;
    
    private bool activeScanner;

    private GameObject[] hiddenObjects;
    private GameObject[] hideableObjects;


    void Start()
    {
        activeScanner = false;
    }

    void Update()
    {
        //Sword
        if (Input.GetMouseButtonDown(0))
        {
            if(sword.transform.parent == hand)
            {
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position, transform.forward, out hit, hitObjectDistance))
                {
                    sword.transform.parent = hit.collider.gameObject.transform;
                    swordHolder = hit.collider.gameObject;
                }
                else if(Physics.Raycast(floorDetectionPoint.position, -transform.up, out hit, 0.2f))
                {
                    sword.transform.parent = hit.collider.gameObject.transform;
                    swordHolder = hit.collider.gameObject;

                    //hardcoding
                    sword.transform.eulerAngles = new Vector3(0, 90, 0);
                    sword.transform.position -= new Vector3(0, 0.25f, 0);
                }
            }
            else
            {
                sword.transform.position = hand.position;
                sword.transform.rotation = hand.rotation;
                sword.transform.parent = hand;

                if(swordHolder.layer == hiddenObjectsLayer)
                {
                    Hide(swordHolder);
                }
                else if (swordHolder.layer == hideableObjectsLayer)
                {
                    Show(swordHolder);
                }
            }       
        }

        //Scanner
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!activeScanner)
            {
                hiddenObjects = FindObjectsInLayer(hiddenObjectsLayer);
                hideableObjects = FindObjectsInLayer(hideableObjectsLayer);

                activeScanner = true;            
            } 
            
            for (int i = 0; i < hiddenObjects.Length; i++)
            {
                if (Vector3.Distance(sword.transform.position, hiddenObjects[i].transform.position) > detectionRadius)
                {
                    Hide(hiddenObjects[i]);
                }
                else
                {
                    Show(hiddenObjects[i]);
                }
            }
            
            for (int i = 0; i < hideableObjects.Length; i++)
            {
                if (Vector3.Distance(sword.transform.position, hideableObjects[i].transform.position) < detectionRadius)
                {
                    Hide(hideableObjects[i]);
                }
                else
                {    
                    Show(hideableObjects[i]);
                }
            }
        }
        else if (activeScanner && sword.transform.parent == hand)
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
            if(objects[i].layer == layerIdx)
            {
                objectsInLayer.Add(objects[i]);
            }
        }

        return objectsInLayer.ToArray();
    }

    private void ScannerOff()
    {
        for (int i = 0; i < hiddenObjects.Length; i++)
        {
            if (hiddenObjects[i].transform != sword.transform.parent)
            {
                Hide(hiddenObjects[i]);
            }
        }

        for (int i = 0; i < hideableObjects.Length; i++)
        {
            Show(hideableObjects[i]);
        }
        
    }
    
    private void Show(GameObject go)
    {
        if(go.GetComponent<MeshRenderer>().enabled == false)
        {
            go.GetComponent<MeshRenderer>().enabled = true;
            go.GetComponent<Collider>().enabled = true;
        }     
    }

    private void Hide(GameObject go)
    {
        if(go.GetComponent<MeshRenderer>().enabled == true)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
            go.GetComponent<Collider>().enabled = false;
        }    
    }
}
