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

    public static bool CanInteractWithEdge(Vector3 pf, Vector3 ef, float a)
    {
        return Vector3.Angle(pf, -ef) <= a;
    }

    public static Vector3 GetEdgeOffsetOnLocalSapce(GameObject edge, Vector3 offset)
    {
        var result = edge.transform.right * offset.x;
        result += edge.transform.up * offset.y;
        result += edge.transform.forward * offset.z;
        return result;
    }
    
    public static Collider[] GetFloorColliders(PlayerMovementController _controller, Vector3 position)
    {
        if (_controller.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return Physics.OverlapSphere(position, 0.1f, _controller.layersToCheckFloorOutsideScanner);
        }
        return Physics.OverlapSphere(position, 0.1f, _controller.layersToCheckFloorInsideScanner);
    }
}