using UnityEngine;

public abstract class PlayerState
{
    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void ExitState() { }
}
