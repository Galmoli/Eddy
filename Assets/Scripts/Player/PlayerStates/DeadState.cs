public class DeadState : State
{
    private PlayerMovementController _controller;

    public DeadState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        _controller.animator.SetTrigger("Death");
        _controller.characterController.enabled = false;
        if(!_controller.scannerSword.HoldingSword() && _controller.scannerSword.SwordUnlocked()) _controller.scannerSword.SwordBack();
    }

    public override void Interact()
    {
        _controller.transform.position = GameManager.Instance.respawnPos;
    }
}
