using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvisionalPlayerController : MonoBehaviour
{
    CharacterController characterController;

    Vector3 movement;

    private float speed = 1.5f;
    private float verticalSpeed;
    private float jumpSpeed = 4f;
    private bool onGround;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // movement depends on camera forward
        
        movement = Vector3.zero;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraForward.Normalize();
        cameraRight.y = 0f;
        cameraRight.Normalize();


        if (Input.GetKey(KeyCode.W))
            movement = cameraForward;
        else if (Input.GetKey(KeyCode.S))
            movement = -cameraForward;

        if (Input.GetKey(KeyCode.D))
            movement += cameraRight;
        else if (Input.GetKey(KeyCode.A))
            movement -= cameraRight;

        movement.Normalize();

        bool l_hasMovement = movement.sqrMagnitude > 0.01f;
        if (l_hasMovement)
            transform.forward = movement;

        movement *= speed * Time.deltaTime;

        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y = verticalSpeed * Time.deltaTime;

        CollisionFlags collisionFlags;

        collisionFlags = characterController.Move(movement);
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            onGround = true;
            verticalSpeed = 0.0f;
            
        }
        else
        {
            onGround = false;
        }

        if ((collisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0.0f)
            verticalSpeed = 0.0f;

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            verticalSpeed = jumpSpeed;
        }
    }
}
