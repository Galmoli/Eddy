using UnityEngine;

namespace Steerings
{
	public class WanderPlusAvoid : SteeringBehaviour
	{
		[HideInInspector] public float wanderRate = 30f;
		[HideInInspector] public float wanderRadius = 10f;
		[HideInInspector] public float wanderOffset = 20f;
		private float targetOrientation = 0f;

		[HideInInspector] public float lookAheadLength = 10f;
		[HideInInspector] public float avoidDistance = 10f;
		[HideInInspector] public float secondaryWhiskerAngle = 30f;
		[HideInInspector] public float secondaryWhiskerRatio = 0.7f;
		[HideInInspector] public LayerMask avoidLayers;
		[HideInInspector] public SphereCollider scanner;

		private bool avoidActive = false;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = WanderPlusAvoid.GetSteering (ownKS, wanderRate, wanderRate, wanderOffset, ref targetOrientation, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, ref avoidActive, avoidLayers, scanner);

			if (ownKS.linearVelocity.magnitude > 0.001f)
			{
				transform.rotation = Quaternion.Euler(0, VectorToOrientation(ownKS.linearVelocity), 0);
				ownKS.orientation = transform.rotation.eulerAngles.y;
			}
			result.angularActive = false;

			return result;
		}

		public static SteeringOutput GetSteering (KinematicState ownKS, float WanderRate, float wanderRadius, float wanderOffset, ref float targetOrientation, float lookAheadLength, float avoidDistance, float secondaryWhiskerAngle, float secondaryWhiskerRatio, ref bool avoidActive, LayerMask avoidLayers, SphereCollider scanner)
		{
			SteeringOutput so = ObstacleAvoidance.GetSteering(ownKS, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, avoidLayers, scanner);

			if (so == nullSteering)
			{
				if (avoidActive)
				{
					targetOrientation = ownKS.orientation;
				}

				avoidActive = false;

				return Wander.GetSteering (ownKS, ref targetOrientation, WanderRate, wanderRadius, wanderOffset);
			}
			else
			{
				avoidActive = true;
				return so;
			}
		}
	}
}
