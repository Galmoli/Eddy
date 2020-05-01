using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PushPullObject : MonoBehaviour
{
    public float speedWhenMove;
    public float angleToAllowMovement;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask _layersToDetectCollision;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canPush;
    [HideInInspector] public bool canPull;
    [HideInInspector] public Vector3 moveVector; //This vector can be negative, it depends if it's pushing or pulling
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GetAngleBetweenForwardAndPlayer() <= angleToAllowMovement && transform.GetChild(0).gameObject.activeSelf)
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
            canMove = false;
        }
    }

    public void Pull()
    {
        transform.Translate(moveVector * (speedWhenMove * Time.deltaTime), Space.World);
    }

    public void Push()
    {
        transform.Translate(-moveVector * (speedWhenMove * Time.deltaTime), Space.World);
    }

    private float GetAngleBetweenForwardAndPlayer()
    {
        Vector3 l_directionVector = GetClosestVector();
        Vector3 playerForward = Vector3.ProjectOnPlane(playerTransform.position - transform.position, Vector3.up).normalized;

        return Mathf.Abs(Vector3.Angle(l_directionVector, playerForward));
    }

    private Vector3 GetDirectionVector()
    {
        Vector3 playerVector = playerTransform.position - transform.position;
        Vector3 l_directionVector = Vector3.ProjectOnPlane(playerVector, Vector3.up).normalized;

        return l_directionVector;
    }

    private bool PushCollision()
    {
        return Physics.Raycast(transform.position, -GetDirectionVector(), GetColliderSize(), _layersToDetectCollision);
    }

    private bool PullCollision()
    {
        return Physics.Raycast(playerTransform.position, GetDirectionVector(), 1, _layersToDetectCollision);
    }

    private Vector3 GetClosestVector()
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

    private Vector3[] Generate3DVectors()
    {
        Vector3[] vectors = new Vector3[4];
        
        vectors[0] = transform.forward;
        vectors[1] = -transform.forward;
        vectors[2] = transform.right;
        vectors[3] = -transform.right;

        return vectors;
    }

    private float GetColliderSize()
    {
        if (GetClosestVector() == transform.forward || GetClosestVector() == -transform.forward)
        {
            return _boxCollider.size.z / 2 + 0.01f;
        }
        return _boxCollider.size.x / 2 + 0.01f;
    }
}
