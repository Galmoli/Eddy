using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scannerRadius;

    public GameObject sword;
    public Transform hand;
    public Transform floorDetectionPoint;
    public float hitObjectDistance;

    private GameObject swordHolder;
    
    private bool activeScanner;

    private GameObject[] hiddenObjects;
    private GameObject[] hideableObjects;

    void Start()
    {
        activeScanner = false;

        sword.transform.GetChild(0).localScale *= scannerRadius * 2f;
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
        if (Input.GetKey(KeyCode.LeftShift) && sword.transform.parent == hand)
        {
            if (!activeScanner)
            {
                hiddenObjects = FindObjectsInLayer(LayerMask.NameToLayer("HiddenObjects"));
                hideableObjects = FindObjectsInLayer(LayerMask.NameToLayer("HideableObjects"));

                activeScanner = true;

                sword.transform.GetChild(0).gameObject.SetActive(true);
            }          
        }
        else if (activeScanner && sword.transform.parent == hand)
        {
            ScannerOff();

            activeScanner = false;
        }

        if (activeScanner)
        {   
            for (int i = 0; i < hiddenObjects.Length; i++)
            {
                if (Vector3.Distance(sword.transform.position, hiddenObjects[i].transform.position) > scannerRadius)
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
                if (Vector3.Distance(sword.transform.position, hideableObjects[i].transform.position) < scannerRadius)
                {
                    Hide(hideableObjects[i]);
                }
                else
                {
                    Show(hideableObjects[i]);
                }
            }
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
        sword.transform.GetChild(0).gameObject.SetActive(false);

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

    private void Stab(GameObject obj, bool vertical)
    {
        swordHolder = obj;

        sword.transform.parent = null;

        if (vertical)
        {
            //hardcoding        
            sword.transform.eulerAngles = new Vector3(0, 90, 0);
            sword.transform.position -= new Vector3(0, 0.25f, 0);
            //
        }

        //if(swordHolder == objecteArrossegable) sword.transform.parent = swordHolder.transform;

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

        sword.transform.parent = null;

        //hardcoding
        sword.transform.rotation = hand.rotation;
        sword.transform.position = hand.position;
        //

        sword.transform.parent = hand;

        //if (swordHolder.layer == hiddenObjectsLayer)
        //{
            //Hide(swordHolder);
        //}
        //else if (swordHolder.layer == hideableObjectsLayer)
        //{
            //Show(swordHolder);
        //}
    }
}
