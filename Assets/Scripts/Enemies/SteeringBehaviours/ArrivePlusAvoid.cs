using UnityEngine;

namespace Steerings
{
	public class ArrivePlusAvoid : SteeringBehaviour
	{
		[HideInInspector] public GameObject target;

		[HideInInspector] public float closeEnoughRadius;
		[HideInInspector] public float slowDownRadius;
		private float timeToDesiredSpeed = 0.1f;

		[HideInInspector] public float lookAheadLength;
		[HideInInspector] public float avoidDistance;
		[HideInInspector] public float secondaryWhiskerAngle;
		[HideInInspector] public float secondaryWhiskerRatio;
		[HideInInspector] public LayerMask avoidLayers;
		[HideInInspector] public SphereCollider scanner;

		[HideInInspector] public string repulsionTag;
		[HideInInspector] public float repulsionThreshold;
		[HideInInspector] public float arriveWeight;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = ArrivePlusAvoid.GetSteering (ownKS, target, closeEnoughRadius, slowDownRadius, timeToDesiredSpeed, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, avoidLayers, scanner, repulsionTag, repulsionThreshold, arriveWeight);
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

		public static SteeringOutput GetSteering (KinematicState ownKS, GameObject target, float closeEnoughRadius, float slowDownRadius, float timeToDesiredSpeed, float lookAheadLength, float avoidDistance, float secondaryWhiskerAngle, float secondaryWhiskerRatio, LayerMask avoidLayers, SphereCollider scanner, string repulsionTag, float repulsionThreshold, float arriveWeight)
		{
			SteeringOutput steeringOutput = ObstacleAvoidance.GetSteering(ownKS, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, avoidLayers, scanner);

			if (steeringOutput == nullSteering)
			{
				SteeringOutput arrive = Arrive.GetSteering(ownKS, target, closeEnoughRadius, slowDownRadius, timeToDesiredSpeed);
				SteeringOutput linearRepulsion = LinearRepulsion.GetSteering(ownKS, repulsionTag, repulsionThreshold);
				arrive.linearAcceleration = arrive.linearAcceleration * arriveWeight + linearRepulsion.linearAcceleration * (1 - arriveWeight);
				return arrive;
			}

			return steeringOutput;
		}
    }
}