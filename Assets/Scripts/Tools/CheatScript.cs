using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{

    public Transform[] transforms;
    public CharacterController charController;

    int currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            charController.enabled = false;
            charController.transform.position = transforms[currentWaypoint].transform.position;
            currentWaypoint++;
            charController.enabled = true;
        }
    }
}
