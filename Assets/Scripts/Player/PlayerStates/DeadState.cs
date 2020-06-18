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
        _controller.animator.gameObject.SetActive(false);
        _controller.rag = GameObject.Instantiate(_controller.ragdollPrefab, _controller.gameObject.transform.position, Quaternion.identity);
        CameraController cameraController = GameObject.FindObjectOfType<CameraController>();
        cameraController.enabled = true;
        HeadLookAt cameraLook = cameraController.gameObject.GetComponent<HeadLookAt>();
        cameraLook.lookAtObj = _controller.rag.transform.GetChild(0).transform.GetChild(0).transform;
        cameraLook.enabled = true;
        FovController fovController = GameObject.FindObjectOfType<FovController>();
        fovController.activated = true;
        fovController.isGoingToMin = true;

        _controller.characterController.enabled = false;

        _hadSwordUnlocked = _controller.scannerSword.SwordUnlocked();
        _sameScene = GameManager.Instance.GetCurrentScene() == GameManager.Instance.checkpointSceneIndex && GameManager.Instance.GetCurrentScene() != 11;
        UIManager.Instance.DeathFade();
    }

    public override void Interact()
    {
        if(_hadSwordUnlocked) _controller.scannerSword.UnlockSword();
        _controller.transform.position = GameManager.Instance.respawnPos;
        if (_sameScene) _controller.characterController.enabled = true;
    }
}
