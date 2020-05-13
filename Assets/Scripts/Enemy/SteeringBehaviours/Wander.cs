using UnityEngine;

namespace Steerings
{
    public class Wander : SteeringBehaviour
    {
        public float wanderRate = 30f;
        public float wanderRadius = 10f;
        public float wanderOffset = 20f;

        protected float targetOrientation = 0f;

        public override SteeringOutput GetSteering()
        {
            SteeringOutput result = Wander.GetSteering(ownKS, ref targetOrientation, wanderRate, wanderRadius, wanderOffset);

            if (ownKS.linearVelocity.magnitude > 0.001f)
            {
                transform.rotation = Quaternion.Euler(0, VectorToOrientation(ownKS.linearVelocity), 0);
                ownKS.orientation = transform.rotation.eulerAngles.y;
            }
            result.angularActive = false;

            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, ref float targetOrientation, float wanderRate, float wanderRadius, float wanderOffset)
        {
            targetOrientation += wanderRate * (Random.value - Random.value);

            surrogateTarget.transform.position = OrientationToVector(targetOrientation) * wanderRadius;

            surrogateTarget.transform.position += ownKS.position + OrientationToVector(ownKS.orientation) * wanderOffset;

            return Seek.GetSteering(ownKS, surrogateTarget);
        }
    }
}
