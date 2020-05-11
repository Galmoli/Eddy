public class DeadState : State
{
    private PlayerMovementController _controller;

    public DeadState(PlayerMovementController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        //Set Dead Animation
        _controller.characterController.enabled = false;
    }

    public override void Interact()
    {
        _controller.transform.position = GameManager.Instance.respawnPos;
        _controller.characterController.enabled = true;
    }
}
