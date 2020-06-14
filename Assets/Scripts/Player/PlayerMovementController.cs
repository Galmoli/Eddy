using System;
using System.Collections;
using UnityEngine;
using FMOD.Studio;

public class PlayerMovementController : StateMachine
{
    [Header("Movement Values")] 
    public float minSpeed;
    public float maxSpeed;
    public float jumpSpeed;
    public  float speedMultiplierWhenJump;
    public float gravityMultiplier; 
    public float joystickDeadZone;
    public Transform feetOverlap;
    public LayerMask layersToCheckFloorOutsideScanner;
    public LayerMask layersToCheckFloorInsideScanner;

    [Header("Edge Values")] 
    public Vector3 edgeOffset;
    public Vector3 edgeCompletedOffset;
    public float angleToAllowClimbEdge;
    public float edgeOffsetToKnee;
    public float edgeOffsetToWaist;
    public float lerpVelocity;
    [HideInInspector] public Vector3 edgePosition;
    [HideInInspector] public GameObject edgeGameObject;
    
    //Player Inputs
    private InputActions _input;
    [HideInInspector] public Vector2 movementVector;

    //Objects
    [HideInInspector] public PlayerSwordScanner scannerSword;
    [HideInInspector] public PlayerCombatController combatController;
    [HideInInspector] public SphereCollider scannerCollider;

    //Components
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public PlayerSounds playerSounds;

    //Variables
    [HideInInspector] public bool inputMoveObject;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool onEdge;
    [HideInInspector] public bool edgeAvailable;
    [HideInInspector] public bool standing;
    [HideInInspector] public bool inputToStand;
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public Transform cameraTransform;
    [HideInInspector] public PushPullObject moveObject;
    [HideInInspector] public ScannerIntersectionManager scannerIntersect;
    [HideInInspector] public Rigidbody standRb;

    private EventInstance dragSoundEvent;


    [Header("Animation")]
    public Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        combatController = GetComponent<PlayerCombatController>();
        playerSounds = GetComponent<PlayerSounds>();
        cameraTransform = Camera.main.gameObject.transform;
        _input = new InputActions();
        scannerSword = FindObjectOfType<PlayerSwordScanner>();
        scannerCollider = scannerSword.GetComponent<SphereCollider>();
        scannerIntersect = FindObjectOfType<ScannerIntersectionManager>();

        _input.PlayerControls.Move.performed += callbackContext => movementVector = callbackContext.ReadValue<Vector2>();
        _input.PlayerControls.Jump.started += callbackContext => JumpInput();
        _input.PlayerControls.MoveObject.started += callbackContext => inputMoveObject = true;
        _input.PlayerControls.MoveObject.canceled += callbackContext => inputMoveObject = false;
    }

    private void Start()
    {
        SetState(new MoveState(this));
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        state.Update();
    }

    public void OnEdge()
    {
        edgeAvailable = true;
    }

    public void OffEdge()
    {
        edgeAvailable = false;
    }

    private void JumpInput()
    {
        if (!onEdge && state.GetType() == typeof(MoveState) && !UIManager.Instance.paused)
        {
            jump = true;
            animator.SetTrigger("Jump");
            JumpSound();
        }
        if (!standing && onEdge) inputToStand = true;
    }
    
    public void RotateTowardsForward(Vector3 forward)
    {
        if (UIManager.Instance.paused) return;
        transform.LookAt(transform.position + forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoveObject") && other.transform.parent != null)
        {
            moveObject = other.transform.parent.GetComponent<PushPullObject>();
        }
    }

    public void StandEdge()
    {
        StartCoroutine(Co_StandEdge());
    }

    private IEnumerator Co_StandEdge()
    {
        Vector3 finalPos = GetProjectedVector() + edgePosition + PlayerUtils.GetEdgeOffsetOnLocalSpace(edgeGameObject, edgeCompletedOffset);
        transform.position = finalPos;
        yield return new WaitForEndOfFrame();

        standing = false;
        onEdge = false;
        Destroy(standRb);
        characterController.enabled = true;
        SetState(new MoveState(this));
    }

    public void StandDeactivatePlayer()
    {
        if (standRb == null)
        {
            characterController.enabled = false;
            standing = true;
            inputToStand = false;
            standRb = gameObject.AddComponent<Rigidbody>();
            standRb.isKinematic = true;
        }
    }

    public void WaistStand()
    {
        StartCoroutine(Co_WaistStand());
    }
    private IEnumerator Co_WaistStand()
    {
        StandDeactivatePlayer();
        Vector3 finalPos = GetProjectedVector() + edgePosition + PlayerUtils.GetEdgeOffsetOnLocalSpace(edgeGameObject, edgeCompletedOffset);

        while (Vector3.Distance(transform.position, finalPos) > 0.2f)
        {
            Debug.Log(Vector3.Distance(transform.position, finalPos));
            Vector3 lerpVector = Vector3.Lerp(transform.position, finalPos, lerpVelocity*0.2f);
            transform.position = lerpVector;
            yield return null;
        }

        standing = false;
        onEdge = false;
        Destroy(standRb);
        characterController.enabled = true;
        SetState(new MoveState(this));
    }

    public Vector3 GetProjectedVector()
    {
        var projectedVector = Vector3.ProjectOnPlane(transform.position - edgePosition, edgeGameObject.transform.forward);
        return Vector3.ProjectOnPlane(projectedVector, transform.up);
    }

    public void Spawn()
    {
        state.Interact();
    }

    public void CheckCollisions()
    {
        scannerIntersect.CheckIntersections();
    }

    public State GetState()
    {
        return state;
    }

    #region Sounds
    public void JumpSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.jumpSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.jumpSoundPath, transform);
        }  
    }

    public void LandingSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.landSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.landSoundPath, transform);
        }
    }

    public void DragSound()
    {
        if (!AudioManager.Instance.isPlaying(dragSoundEvent))
        {
            if (AudioManager.Instance.ValidEvent(playerSounds.draggableObjectSoundPath))
            {
                dragSoundEvent = AudioManager.Instance.PlayEvent(playerSounds.draggableObjectSoundPath, transform);
            }
        }
    }

    public void StopDragSound()
    {
        dragSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    
    public void StepSound()
    {
        if (AudioManager.Instance.ValidEvent(playerSounds.woodStepSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(playerSounds.woodStepSoundPath, transform);
        }

        /*if (AudioManager.Instance.ValidEvent(stoneStepSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(stoneStepSoundPath, transform);
        }*/
    }
    #endregion
}
