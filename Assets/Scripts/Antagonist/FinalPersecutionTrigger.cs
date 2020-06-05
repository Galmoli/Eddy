using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPersecutionTrigger : MonoBehaviour
{
    public AntagonistEndingFSM antagonistEndingFSM;
    public AntagonistEndingFSM.States state;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            antagonistEndingFSM.ChangeState(state);
            gameObject.SetActive(false);
        }
    }
}
