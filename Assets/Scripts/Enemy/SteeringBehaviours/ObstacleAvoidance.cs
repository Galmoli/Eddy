using UnityEngine;

namespace Steerings
{
	public class ObstacleAvoidance : SteeringBehaviour
	{
		public float lookAheadLength = 10f;
		public float avoidDistance = 10f;
		public float secondaryWhiskerAngle = 30f;
		public float secondaryWhiskerRatio = 0.7f;
		public LayerMask avoidLayers;
		public SphereCollider scanner;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = ObstacleAvoidance.GetSteering (ownKS, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio, avoidLayers, scanner);

			if (ownKS.linearVelocity.magnitude > 0.001f)
			{
				transform.rotation = Quaternion.Euler(0, VectorToOrientation(ownKS.linearVelocity), 0);
				ownKS.orientation = transform.rotation.eulerAngles.z;
			}
			result.angularActive = false;

			return result;
		}

		public static SteeringOutput GetSteering (KinematicState ownKS, float lookAheadLength, float avoidDistance, float secondaryWhiskerAngle, float secondaryWhiskerRatio, LayerMask avoidLayers, SphereCollider scanner)
		{
			Vector3 mainDirection;
			
			if (ownKS.linearVelocity.magnitude < 0.0001f)
			{
				mainDirection = OrientationToVector(ownKS.orientation);
			}
			else
			{
				mainDirection = ownKS.linearVelocity.normalized;
			}

			Collider collider = ownKS.gameObject.GetComponent<Collider>();
			bool before = false;
			if (collider != null)
			{
				before = collider.enabled;
				collider.enabled = false;
			}

			Vector3 centralWhiskerDirection = mainDirection;
			Vector3 rightWhiskerDirection = OrientationToVector (VectorToOrientation (mainDirection) + secondaryWhiskerAngle);
			Vector3 leftWhiskerDirection = OrientationToVector (VectorToOrientation (mainDirection) - secondaryWhiskerAngle);

			Debug.DrawRay (ownKS.position, centralWhiskerDirection * lookAheadLength);
			Debug.DrawRay (ownKS.position, rightWhiskerDirection * lookAheadLength * secondaryWhiskerRatio);
			Debug.DrawRay (ownKS.position, leftWhiskerDirection * lookAheadLength * secondaryWhiskerRatio);

			RaycastHit hit;

			#region Central Whisker
			if (Physics.Raycast(ownKS.position, centralWhiskerDirection, out hit, lookAheadLength, avoidLayers)) 
			{
				if(ValidObstacle(hit, scanner))
				{
					SURROGATE_TARGET.transform.position = hit.point + hit.normal * avoidDistance;

					if (collider != null)
					{
						collider.enabled = before;
					}

					Debug.DrawRay(ownKS.position, centralWhiskerDirection * lookAheadLength, Color.red);

					return Seek.GetSteering(ownKS, SURROGATE_TARGET);
				}	
			}
            #endregion

            #region Right Whisker
            if (Physics.Raycast(ownKS.position, rightWhiskerDirection, out hit, lookAheadLength * secondaryWhiskerRatio, avoidLayers))
			{
				if (ValidObstacle(hit, scanner))
				{
					SURROGATE_TARGET.transform.position = hit.point + hit.normal * avoidDistance;

					if (collider != null)
					{
						collider.enabled = before;
					}

					Debug.DrawRay(ownKS.position, rightWhiskerDirection * lookAheadLength * secondaryWhiskerRatio, Color.red);

					return Seek.GetSteering(ownKS, SURROGATE_TARGET);
				}	
			}
            #endregion

            #region Left Whisker
            if (Physics.Raycast(ownKS.position, leftWhiskerDirection, out hit, lookAheadLength * secondaryWhiskerRatio, avoidLayers))
			{
				if (ValidObstacle(hit, scanner))
				{
					SURROGATE_TARGET.transform.position = hit.point + hit.normal * avoidDistance;

					if (collider != null)
					{
						collider.enabled = before;
					}

					Debug.DrawRay(ownKS.position, leftWhiskerDirection * lookAheadLength * secondaryWhiskerRatio, Color.red);

					return Seek.GetSteering(ownKS, SURROGATE_TARGET);
				}	
			}
            #endregion

            if (collider != null)
			{
				collider.enabled = before;
			}
			
			return NULL_STEERING;
		}

		private static bool ValidObstacle(RaycastHit hit, SphereCollider scanner)
		{
			return HideLayer(hit, scanner) || AppearLayer(hit, scanner) || OtherLayer(hit.collider.gameObject);
		}

		private static bool HideLayer(RaycastHit hit, SphereCollider scanner)
		{
			return hit.collider.gameObject.layer == LayerMask.NameToLayer("Hide") && !scanner.bounds.Contains(hit.point);
		}

		private static bool AppearLayer(RaycastHit hit, SphereCollider scanner)
		{
			return hit.collider.gameObject.layer == LayerMask.NameToLayer("Appear") && scanner.bounds.Contains(hit.point);
		}

		private static bool OtherLayer(GameObject go)
		{
			return go.layer != LayerMask.NameToLayer("Hide") && go.layer != LayerMask.NameToLayer("Appear");
		}
	}
}