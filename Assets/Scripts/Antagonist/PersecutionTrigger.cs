using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersecutionTrigger : MonoBehaviour
{

    public AntagonistPersecutionFSM antagonistPersecutionFSM;
    public AntagonistPersecutionFSM.States state;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            antagonistPersecutionFSM.ChangeState(state);
            gameObject.SetActive(false);
        }
    }
}
