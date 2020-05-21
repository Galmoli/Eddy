using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected State state;

    public void SetState(State state)
    {
        this.state = state;
        this.state.Enter();
    }
}
