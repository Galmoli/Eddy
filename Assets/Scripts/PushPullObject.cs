using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PushPullObject : MonoBehaviour
{
    public float speedWhenMove;
    public float angleToAllowMovement;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform _triggerTransform;
    [SerializeField] private LayerMask _layersToDetectCollision;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canPush;
    [HideInInspector] public bool canPull;
    [HideInInspector] public bool swordStabbed;
    [HideInInspector] public bool moving;
    [HideInInspector] public Vector3 moveVector; //This vector can be negative, it depends if it's pushing or pulling
    private BoxCollider _boxCollider;
    private Rigidbody _rb;
    private InputActions _input;
    private Vector3 meshForward;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.Sword.started += ctx => SwordInput();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        meshForward = transform.forward;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GetAngleBetweenForwardAndPlayer() <= angleToAllowMovement)
            {

                if (gameObject.layer == LayerMask.NameToLayer("Appear"))
                {
                    if (GetComponentInChildren<PlayerSwordScanner>())
                        UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Drag, transform.position + Vector3.up * 2);
                    else
                        UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.NailSword, transform.position + Vector3.up * 2);
                }
                else
                    UIHelperController.Instance.EnableHelper(UIHelperController.HelperAction.Drag, transform.position + Vector3.up * 2);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GetAngleBetweenForwardAndPlayer() <= angleToAllowMovement)
            {
                canMove = true;
                moveVector = GetClosestVector();
                
                canPush = !PushCollision();
                canPull = !PullCollision();
            }
            else canMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //UI Helper
            UIHelperController.Instance.DisableHelper();
            canMove = false;
        }
    }

    private void FixedUpdate()
    {
        _triggerTransform.up = Vector3.up;
        _triggerTransform.forward = meshForward;
    }
    
    //Moves this GameObject when the player pulls it.
    public void Pull()
    {
        _rb.velocity = moveVector.normalized * 1.14f * speedWhenMove;
    }
    
    //Moves this GameObject when the player pushes it.
    public void Push()
    {
        _rb.velocity = -moveVector.normalized * 1.05f * speedWhenMove;
    }

    //Gets the angle between the closest vector and the director vector
    //This is used to check if the player is in the safe face of the cube as the movement axis.
    private float GetAngleBetweenForwardAndPlayer()
    {
        Vector3 l_directionVector = GetClosestVector();
        Vector3 playerForward = GetDirectionVector();

        return Mathf.Abs(Vector3.Angle(l_directionVector, playerForward));
    }
    
    //Gets the Projected vector between the player and this object.
    private Vector3 GetDirectionVector()
    {
        Vector3 playerVector = playerTransform.position - transform.position;
        Vector3 l_directionVector = Vector3.ProjectOnPlane(playerVector, Vector3.up).normalized;

        return l_directionVector;
    }
    
    //Checks if the object is colliding when pushing
    private bool PushCollision()
    {
        return Physics.Raycast(transform.position, -GetDirectionVector(), GetColliderSize(), _layersToDetectCollision);
    }
    
    //Checks if the player is colliding when pulling
    private bool PullCollision()
    {
        return Physics.Raycast(playerTransform.position, GetDirectionVector(), 1, _layersToDetectCollision);
    }
    
    //Returns the closest vector to the player
    //This is used to set the axis of movement
    public Vector3 GetClosestVector()
    {
        Vector3[] vectors = Generate3DVectors();
        Vector3 closestVector = vectors[0];
        float closestVectorAngle = 360;

        foreach (var v in vectors)
        {
            var angle = Mathf.Abs(Vector3.Angle(v, GetDirectionVector()));
            if (angle < closestVectorAngle)
            {
                closestVector = v;
                closestVectorAngle = angle;
            }
        }

        return closestVector;
    }

    //Generates the 4 vectors of movement of the cube. 
    //Its used because the cube can be rotated in any angle.
    private Vector3[] Generate3DVectors()
    {
        Vector3[] vectors = new Vector3[6];
        
        vectors[0] = transform.forward;
        vectors[1] = -transform.forward;
        vectors[2] = transform.right;
        vectors[3] = -transform.right;
        vectors[4] = transform.up;
        vectors[5] = -transform.up;
        

        return vectors;
    }

    //Gets the collider size from the center. This is used by the Raycast to get the distance of detection
    private float GetColliderSize()
    {
        if (GetClosestVector() == transform.forward || GetClosestVector() == -transform.forward)
        {
            return _boxCollider.size.z / 2 + 0.01f;
        }
        return _boxCollider.size.x / 2 + 0.01f;
    }

    private void SwordInput()
    {
        if (swordStabbed) swordStabbed = false;
    }

    public void LockAllConstraints()
    {
        if (_rb.constraints == RigidbodyConstraints.FreezeRotation || _rb.constraints == RigidbodyConstraints.None)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void UnlockPosConstraints()
    {
        if (_rb.constraints == RigidbodyConstraints.FreezeAll || _rb.constraints == RigidbodyConstraints.None)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void UnlockAllConstrains()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }

    public bool HasFloor()
    {
        var pos = new Vector3(transform.position.x, transform.position.y - _boxCollider.size.y / 2, transform.position.z);
        var colliders = Physics.OverlapSphere(pos, 0.1f);
        return colliders.Any(c => !c.CompareTag("MoveObject"));
    }
}
