using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Values")] 
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float joystickDeadZone;

    [Header("Edge Values")] 
    [SerializeField] private Vector3 edgeOffset;
    [SerializeField] private Vector3 edgeCompletedOffset;
    [SerializeField] private float lerpVelocity;
    [HideInInspector] public Vector3 edgePosition;
    [HideInInspector] public GameObject edgeGameObject;
    
    //Player Inputs
    private InputActions _input;
    private Vector2 movementVector;
    
    //Components
    private CharacterController _characterController;
    
    //Variables
    private bool _jump;
    private bool _onGround;
    private bool _onEdge;
    private bool _edgeAvailable;
    private bool _standing;
    private float _verticalSpeed;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _input = new InputActions();
        _input.PlayerControls.Move.performed += callbackContext => movementVector = callbackContext.ReadValue<Vector2>();
        _input.PlayerControls.Jump.started += callbackContext => _jump = true;
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
        var vector3D = RetargetVector(movementVector);
        if(!_onEdge) RotateTowardsForward(vector3D);

        vector3D *= Mathf.Lerp(minSpeed, maxSpeed, movementVector.magnitude);
        #region Jump

        if (_onGround && _jump)
        {
            _verticalSpeed = jumpSpeed;
            _jump = false;
            if (_edgeAvailable)
            {
                _onEdge = true;
                _verticalSpeed = 0;
            }
        }

        if (!_onEdge)
        {
            _verticalSpeed += Physics.gravity.y * Time.deltaTime * gravityMultiplier;
            vector3D.y = _verticalSpeed;
            var collisionFlags = _characterController.Move(vector3D * Time.deltaTime);

            if ((collisionFlags & CollisionFlags.Below) != 0)
            {
                _onGround = true;
                _verticalSpeed = 0;
            }
            else
            {
                _onGround = false;
            }

            if ((collisionFlags & CollisionFlags.Above) != 0 && _verticalSpeed > 0) _verticalSpeed = 0;
        }
        else
        {
            var position = transform.position;
            Vector3 moveVector = Vector3.zero;
            
            Vector3 lVector = Vector3.Lerp(position, edgePosition + edgeOffset, lerpVelocity);
            
            moveVector = new Vector3(0, (lVector - position).y, 0);
            
            RotateTowardsForward(-edgeGameObject.transform.forward);
            
            _characterController.Move(moveVector);
        }
        

        #endregion

        #region Edge

        if (_onEdge)
        {
            if (StandEdge() && !_standing) StartCoroutine(Co_StandEdge(edgePosition + edgeCompletedOffset));
            if (FallEdge() && !_standing) _onEdge = false;
        }
        
        #endregion
    }

    private Vector3 RetargetVector(Vector2 input)
    {
        if(input.magnitude <= joystickDeadZone) return Vector3.zero;
        
        Vector3 outputVector;

        outputVector.x = -input.y;
        outputVector.y = 0;
        outputVector.z = input.x;

        return outputVector;
    }

    private void RotateTowardsForward(Vector3 forward)
    {
        transform.LookAt(transform.position + forward);
    }

    public void OnEdge()
    {
        _edgeAvailable = true;
    }

    public void OffEdge()
    {
        _edgeAvailable = false;
    }

    private bool StandEdge()
    {
        Vector2 edgeForward = new Vector2(edgeGameObject.transform.forward.x, edgeGameObject.transform.forward.z);
        Vector2 input = new Vector2(Mathf.RoundToInt(movementVector.y), Mathf.RoundToInt(movementVector.x));
        if (edgeForward.normalized == input) return true;
        return false;
    }

    private bool FallEdge()
    {
        Vector2 edgeForward = new Vector2(edgeGameObject.transform.forward.x, edgeGameObject.transform.forward.z);
        Vector2 input = new Vector2(Mathf.RoundToInt(movementVector.y), Mathf.RoundToInt(movementVector.x));
        if (edgeForward.normalized == -input) return true;
        return false;
    }

    private IEnumerator Co_StandEdge(Vector3 finalPos)
    {
        _characterController.enabled = false;
        _standing = true;
        
        //Movement in Y
        while (Math.Abs(finalPos.y - transform.position.y) > _characterController.height)
        {
            var lerpVector = Vector3.Lerp(transform.position, finalPos, lerpVelocity);
            var moveVector = new Vector3(0, (lerpVector - transform.position).y, 0);
            transform.Translate(moveVector);
            
            yield return null;
        }
        
        //Movement in Z
        while (Math.Abs(transform.position.z - finalPos.z) > 0.1f)
        {
            var lerpVector = Vector3.Lerp(transform.position, finalPos, lerpVelocity);
            var moveVector = new Vector3((lerpVector - transform.position).z, 0, 0);

            transform.Translate(moveVector);
            yield return null;
        }

        _standing = false;
        _onEdge = false;
        _characterController.enabled = true;
    }
}
