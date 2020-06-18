using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDoor : MonoBehaviour
{
    public AntagonistFSM antagonistFSM;
    public float force;
    Rigidbody rb;

    bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!end && Time.timeScale != 0)
            rb.AddForce(force * Vector3.down, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Guillotine")
        {
            antagonistFSM.ChangeState(AntagonistFSM.States.BEHEADED);
        }

        if (other.tag == "GuillotineEnd")
        {
            end = true;
        }
    }
}
