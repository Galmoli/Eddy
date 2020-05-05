using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtils
{
    public static Vector3 RetargetVector(Vector2 input, Transform cameraTransform, float joystickDeadZone)
    {
        if(input.magnitude <= joystickDeadZone) return Vector3.zero;
        
        Vector3 outputVector = Vector3.zero;

        outputVector += Vector3.ProjectOnPlane(cameraTransform.forward * input.y, Vector3.up);
        outputVector += Vector3.ProjectOnPlane(cameraTransform.right * input.x, Vector3.up);

        return outputVector;
    }
    public static bool InputEqualVector(Vector3 _vector, Transform cameraTransform, Vector3 movementVector)
    {
        Vector2 l_directorVector = new Vector2(Mathf.RoundToInt(_vector.x), Mathf.RoundToInt(_vector.z));
        Vector3 outputVector = Vector3.zero;

        outputVector += Vector3.ProjectOnPlane(cameraTransform.forward * movementVector.y, Vector3.up);
        outputVector += Vector3.ProjectOnPlane(cameraTransform.right * movementVector.x, Vector3.up);
        
        Vector2 input = new Vector2(Mathf.RoundToInt(outputVector.x), Mathf.RoundToInt(outputVector.z));
        return l_directorVector == input;
    }
    
    public static bool InputDirectionTolerance(Vector3 _vector ,float angleTolerance, Transform _cameraTransform, Vector3 movementVector)
    {
        Vector2 l_directorVector = new Vector2(Mathf.RoundToInt(_vector.x), Mathf.RoundToInt(_vector.z));
        Vector3 outputVector = Vector3.zero;

        outputVector += Vector3.ProjectOnPlane(_cameraTransform.forward * movementVector.y, Vector3.up);
        outputVector += Vector3.ProjectOnPlane(_cameraTransform.right * movementVector.x, Vector3.up);
        
        Vector2 input = new Vector2(outputVector.x, outputVector.z);
        float angle = Mathf.Abs(Vector2.Angle(l_directorVector, input));
        
        return angle <= angleTolerance;
    }
}