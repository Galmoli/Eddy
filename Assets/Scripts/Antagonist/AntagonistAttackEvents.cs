using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistAttackEvents : MonoBehaviour
{

    public AntagonistFSM antagonistFSM;

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().Hit(30);
                break;
            case "Obstacle":

                antagonistFSM.ChangeState(AntagonistFSM.States.STUNNED);

                break;
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {

            case "End":

                other.gameObject.SetActive(false);
                antagonistFSM.ChangeState(AntagonistFSM.States.WAITTOBEHEAD);

                break;
        }
    }
}
