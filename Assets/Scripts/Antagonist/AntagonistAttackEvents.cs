using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistAttackEvents : MonoBehaviour
{

    public AntagonistPersecutionFSM antagonistPersecutionFSM;
    public AntagonistBossFSM antagonistBossFSM;
    public AntagonistEndingFSM antagonistEndingFSM;

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                break;
            case "Obstacle":

                if (antagonistPersecutionFSM.enabled)
                    antagonistPersecutionFSM.ChangeState(AntagonistPersecutionFSM.States.STUNNED);
                else if (antagonistBossFSM.enabled)
                    antagonistBossFSM.ChangeState(AntagonistBossFSM.States.STUNNED);
                else
                    antagonistEndingFSM.ChangeState(AntagonistEndingFSM.States.STUNNED);

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
                antagonistPersecutionFSM.enabled = false;
                antagonistBossFSM.enabled = true;
                break;
        }
    }
}
