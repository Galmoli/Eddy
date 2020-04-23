using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Editable Values")] 
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float joystickDeadZone;
    
    //Player Inputs
    private InputActions _input;
    private Vector2 movementVector;
    
    //Components
    private CharacterController _characterController;
    
    //Variables
    private bool _jump;
    private bool _onGround;
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
        RotateTowardsForward(vector3D);

        vector3D *= Mathf.Lerp(minSpeed, maxSpeed, movementVector.magnitude);
        
        #region Jump

        if (_onGround && _jump)
        {
            _verticalSpeed = jumpSpeed;
            _jump = false;
        }

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
}
