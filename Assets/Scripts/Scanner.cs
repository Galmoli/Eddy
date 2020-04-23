using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float detectionRadius;
    
    private bool activeScanner;

    //private List<GameObject> visibleObjects = new List<GameObject>();
    //private List<GameObject> invisibleObjects = new List<GameObject>();
    GameObject[] hiddenObjects;
    GameObject[] hideableObjects;


    void Start()
    {
        activeScanner = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!activeScanner)
            {
                ScannerOn();
                
                activeScanner = true;            
            } 
            
            for (int i = 0; i < hiddenObjects.Length; i++)
            {
                if (Vector3.Distance(gameObject.transform.position, hiddenObjects[i].transform.position) > detectionRadius)
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
                if (Vector3.Distance(gameObject.transform.position, hideableObjects[i].transform.position) < detectionRadius)
                {
                    Hide(hideableObjects[i]);
                }
                else
                {    
                    Show(hideableObjects[i]);
                }
            }
        }
        else if (activeScanner)
        {
            ScannerOff();

            activeScanner = false;
        }
    }

    private void ScannerOn()
    {
        hiddenObjects = GameObject.FindGameObjectsWithTag("Hidden");
        hideableObjects = GameObject.FindGameObjectsWithTag("Hideable");
    }
    
    private void ScannerOff()
    {
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
