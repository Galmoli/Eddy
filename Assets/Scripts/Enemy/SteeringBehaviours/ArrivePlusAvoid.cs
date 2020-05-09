using UnityEngine;

namespace Steerings
{
	public class ArrivePlusAvoid : SteeringBehaviour
	{
		public GameObject target;

		public float closeEnoughRadius = 5f;
		public float slowDownRadius = 20f;
		public float timeToDesiredSpeed = 0.1f; 

		public float lookAheadLength = 10f;
		public float avoidDistance = 10f;
		public float secondaryWhiskerAngle = 30f;
		public float secondaryWhiskerRatio = 0.7f;
		public LayerMask avoidLayers;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = ArrivePlusAvoid.GetSteering (ownKS, target, closeEnoughRadius, slowDownRadius, timeToDesiredSpeed, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, avoidLayers);

			if (ownKS.linearVelocity.magnitude > 0.001f)
			{
				transform.rotation = Quaternion.Euler(0, VectorToOrientation(ownKS.linearVelocity), 0);
				ownKS.orientation = transform.rotation.eulerAngles.z;
			}
			result.angularActive = false;

			return result;
		}

		public static SteeringOutput GetSteering (KinematicState ownKS, GameObject target, float closeEnoughRadius, float slowDownRadius, float timeToDesiredSpeed, float lookAheadLength, float avoidDistance, float secondaryWhiskerAngle, float secondaryWhiskerRatio, LayerMask avoidLayers)
		{
			SteeringOutput steeringOutput = ObstacleAvoidance.GetSteering(ownKS, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, avoidLayers);

			if (steeringOutput == NULL_STEERING)
			{
				return Arrive.GetSteering (ownKS, target, closeEnoughRadius, slowDownRadius, timeToDesiredSpeed);
			}

			return steeringOutput;
		}
    }
}