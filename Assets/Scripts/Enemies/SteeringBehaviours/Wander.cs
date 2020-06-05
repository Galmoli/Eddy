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
            if (!surrogateTarget) return null;
            
            if (ownKS.linearVelocity.magnitude > 0.001f)
            {
                surrogateTarget.transform.rotation = Quaternion.Euler(0, 0, VectorToOrientation(ownKS.linearVelocity));
                SteeringOutput st = Align.GetSteering(ownKS, surrogateTarget);
                result.angularAcceleration = st.angularAcceleration;
                result.angularActive = st.angularActive;
            }
            else
            {
                result.angularActive = false;
            }

            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, ref float targetOrientation, float wanderRate, float wanderRadius, float wanderOffset)
        {
            targetOrientation += wanderRate * (Random.value - Random.value);

            if(surrogateTarget) surrogateTarget.transform.position = OrientationToVector(targetOrientation) * wanderRadius;

            if(surrogateTarget) surrogateTarget.transform.position += ownKS.position + OrientationToVector(ownKS.orientation) * wanderOffset;

            return Seek.GetSteering(ownKS, surrogateTarget);
        }
    }
}
