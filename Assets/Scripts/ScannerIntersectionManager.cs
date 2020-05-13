using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScannerIntersectionManager : MonoBehaviour
{
    [SerializeField] private BoxCollider checker;
    [SerializeField] private SphereCollider swordRadius;
    private float checkerRadius;
    private int circumferencesToCheck;
    private InputActions _input;

    private void Awake()
    {
        _input = new InputActions();
        _input.Enable();
        _input.PlayerControls.MoveObject.canceled += ctx => CheckIntersections();
    }

    public void CheckIntersections()
    {
        DeleteIntersections();
        checkerRadius = checker.size.x/2;
        circumferencesToCheck = (int)(2 * Mathf.PI * swordRadius.radius / (checkerRadius * 2));
        float angleY = 0, angleX = 0;

        for (int i = 0; i < circumferencesToCheck/2; i++)
        {
            for (int r = 0; r < circumferencesToCheck; r++)
            {
                Vector3 centerPointToOverlap = swordRadius.transform.position + Quaternion.Euler(angleX, angleY, 0) * Vector3.forward * swordRadius.radius;

                Collider[] overlapCols = Physics.OverlapSphere(centerPointToOverlap, checker.size.x / 8);
      
                for (int c = 0; c < overlapCols.Length; c++)
                {
                    if ((overlapCols[c].gameObject.layer == 13 || overlapCols[c].gameObject.layer == 14) && overlapCols[c].isTrigger == false)
                    {
                        GameObject g = ObjectPooler.SharedInstance.GetPooledObject();
                        g.transform.position = centerPointToOverlap;
                        g.transform.forward = swordRadius.transform.position - g.transform.position;
                        if (overlapCols[c].gameObject.layer == 13) g.layer = LayerMask.NameToLayer("inScanner");
                        else  g.layer = LayerMask.NameToLayer("Normal");
                        g.SetActive(true);
                        break;
                    }
                }
                angleX += 360.0f / circumferencesToCheck;
            }
            angleY += 360.0f / circumferencesToCheck;
            angleX = 0;    
        }
    }
    
    public void CheckIntersections(Collider exception)
    {
        DeleteIntersections();
        checkerRadius = checker.size.x/2;
        circumferencesToCheck = (int)(2 * Mathf.PI * swordRadius.radius / (checkerRadius * 2));
        float angleY = 0, angleX = 0;

        for (int i = 0; i < circumferencesToCheck/2; i++)
        {
            for (int r = 0; r < circumferencesToCheck; r++)
            {
                Vector3 centerPointToOverlap = swordRadius.transform.position + Quaternion.Euler(angleX, angleY, 0) * Vector3.forward * swordRadius.radius;

                Collider[] overlapCols = Physics.OverlapSphere(centerPointToOverlap, checker.size.x / 8);

                for (int c = 0; c < overlapCols.Length; c++)
                {
                    if ((overlapCols[c].gameObject.layer == LayerMask.NameToLayer("Hide") || overlapCols[c].gameObject.layer == LayerMask.NameToLayer("Appear")) && overlapCols[c].isTrigger == false && overlapCols[c] != exception)
                    {
                        GameObject g = ObjectPooler.SharedInstance.GetPooledObject();
                        g.transform.position = centerPointToOverlap;
                        g.transform.forward = swordRadius.transform.position - g.transform.position;
                        if (overlapCols[c].gameObject.layer == LayerMask.NameToLayer("Hide")) g.layer = LayerMask.NameToLayer("inScanner");
                        else  g.layer = LayerMask.NameToLayer("Normal");
                        g.SetActive(true);
                        break;
                    }
                }
                angleX += 360.0f / circumferencesToCheck;
            }
            angleY += 360.0f / circumferencesToCheck;
            angleX = 0;    
        }
    }

    public void DeleteIntersections()
    {
        ObjectPooler.SharedInstance.DisableAllObjects();
    }
}
