using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistAttackEvents : MonoBehaviour
{

    public AntagonistPersecutionFSM antagonistPersecutionFSM;

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                break;
            case "Obstacle":

                antagonistPersecutionFSM.ChangeState(AntagonistPersecutionFSM.States.STUNNED);

                break;
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Pipe":

                other.gameObject.SetActive(false);
                antagonistPersecutionFSM.ChangeState(AntagonistPersecutionFSM.States.WAITINGFORPIPE);

                break;
            case "End":

                other.gameObject.SetActive(false);
                antagonistPersecutionFSM.ChangeState(AntagonistPersecutionFSM.States.WAITINGFORPLAYER);

                break;
        }
    }
}
