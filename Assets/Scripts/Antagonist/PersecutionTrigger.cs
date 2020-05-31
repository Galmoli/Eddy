using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersecutionTrigger : MonoBehaviour
{

    public AntagonistFSM antagonistFSM;
    public AntagonistFSM.States state;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            antagonistFSM.ChangeState(state);
            gameObject.SetActive(false);
        }
    }
}
