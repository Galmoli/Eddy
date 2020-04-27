using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PushPullObject : MonoBehaviour
{
    public float speedWhenMove;
    [SerializeField] private float angleToAllowMovement;
    [SerializeField] private Transform playerTransform;
    [HideInInspector] public bool canMove;
    [HideInInspector] public Vector3 moveVector; //This vector can be negative, it depends if it's pushing or pulling

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GetAngleBetweenForwardAndPlayer() <= angleToAllowMovement)
            {
                canMove = true;
                moveVector = GetDirectionVector();
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
        Vector3 l_directionVector = GetDirectionVector();
        Vector3 playerVector = (playerTransform.position - transform.position).normalized;

        return Mathf.Abs(Vector3.Angle(l_directionVector, playerVector));
    }

    private Vector3 GetDirectionVector()
    {
        Vector3 playerVector = playerTransform.position - transform.position;
        Vector3 l_directionVector = Vector3.ProjectOnPlane(playerVector, Vector3.up).normalized;

        if (Mathf.Abs(l_directionVector.x) >= Mathf.Abs(l_directionVector.z))
        {
            l_directionVector = new Vector3(Mathf.RoundToInt(l_directionVector.x), 0, 0);
        }
        else
        {
            l_directionVector = new Vector3(0, 0, Mathf.RoundToInt(l_directionVector.z));
        }

        return l_directionVector;
    }
}
