using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected PlayerState state;

    public void SetState(PlayerState state)
    {
        this.state = state;
        this.state.Enter();
    }
}
