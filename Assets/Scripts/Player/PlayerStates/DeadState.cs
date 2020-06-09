using UnityEngine;

public class DeadState : State
{
    private PlayerMovementController _controller;
    private bool _hadSwordUnlocked;
    private bool _sameScene;

    public DeadState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        _controller.animator.SetTrigger("Death");
        _controller.characterController.enabled = false;
        
        if(!_controller.scannerSword.HoldingSword() && _controller.scannerSword.SwordUnlocked()) _controller.scannerSword.SwordBack();
        
        _hadSwordUnlocked = _controller.scannerSword.SwordUnlocked();
        _controller.scannerSword.LockSword();
        _sameScene = GameManager.Instance.GetCurrentScene() == GameManager.Instance.checkpointSceneIndex && GameManager.Instance.GetCurrentScene() != 11;
    }

    public override void Interact()
    {
        if(_hadSwordUnlocked) _controller.scannerSword.UnlockSword();
        _controller.transform.position = GameManager.Instance.respawnPos;
        if (_sameScene) _controller.characterController.enabled = true;
    }
}
