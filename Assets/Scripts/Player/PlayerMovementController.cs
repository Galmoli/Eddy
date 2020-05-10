using System;
using System.Collections;
using UnityEngine;

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
    [HideInInspector] public Vector3 edgeOffset;
    [HideInInspector] public Vector3 edgeCompletedOffset;
    public float lerpVelocity;
    [HideInInspector] public Vector3 edgePosition;
    [HideInInspector] public GameObject edgeGameObject;
    
    //Player Inputs
    private InputActions _input;
    [HideInInspector] public Vector2 movementVector;

    //Objects
    [HideInInspector] public PlayerSwordScanner scannerSword;

    //Components
    [HideInInspector] public CharacterController characterController;

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


    [Header("Animation")]
    public Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.gameObject.transform;
        _input = new InputActions();
        scannerSword = FindObjectOfType<PlayerSwordScanner>();
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
        if (!onEdge && state.GetType() == typeof(MoveState)) jump = true;
        if (!standing && onEdge) inputToStand = true;
    }
    
    public void RotateTowardsForward(Vector3 forward)
    {
        transform.LookAt(transform.position + forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoveObject") && other.transform.parent != null)
        {
            moveObject = other.transform.parent.GetComponent<PushPullObject>();
        }
    }

    public void StandEdge(Vector3 finalPos)
    {
        StartCoroutine(Co_StandEdge(finalPos));
    }
    
    private IEnumerator Co_StandEdge(Vector3 finalPos)
    {
        characterController.enabled = false;
        standing = true;
        inputToStand = false;
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        
        //Vertical Movement
        while (Math.Abs(finalPos.y - transform.position.y) > characterController.height)
        {
            var lerpVector = Vector3.Lerp(transform.position, finalPos, lerpVelocity);
            var moveVector = new Vector3(0, (lerpVector - transform.position).y, 0);
           transform.Translate(moveVector);
            
            yield return null;
        }
        
        //Horizontal Movement
        while ((transform.position - finalPos).magnitude > characterController.height)
        {
            var moveVector = new Vector3(finalPos.x, transform.position.y, finalPos.z);
            transform.position = Vector3.MoveTowards(transform.position, moveVector, lerpVelocity);
            yield return null;
        }

        standing = false;
        onEdge = false;
        Destroy(rb);
        characterController.enabled = true;
        SetState(new MoveState(this));
    }

    public void Spawn()
    {
        state.Interact();
    }

    public State GetState()
    {
        return state;
    }
}
