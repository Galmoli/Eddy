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

		public static SteeringOutput GetSteering (KinematicState ownKS, float lookAheadLength, float avoidDistance, float secondaryWhiskerAngle, float secondaryWhiskerRatio, LayerMask avoidLayers, SphereCollider scanner)
		{
			Vector3 centralDirection;
			
			if (ownKS.linearVelocity.magnitude < 0.0001f)
			{
				centralDirection = OrientationToVector(ownKS.orientation);
			}
			else
			{
				centralDirection = ownKS.linearVelocity.normalized;
			}

			RaycastHit hit;

			#region Central Whisker

			Vector3 centralWhiskerDirection = centralDirection;

			Debug.DrawRay(ownKS.position, centralWhiskerDirection * lookAheadLength);

			if (Physics.Raycast(ownKS.position, centralWhiskerDirection, out hit, lookAheadLength, avoidLayers)) 
			{
				if(ValidObstacle(hit, scanner))
				{
					surrogateTarget.transform.position = hit.point + hit.normal * avoidDistance;

					Debug.DrawRay(ownKS.position, centralWhiskerDirection * lookAheadLength, Color.red);

					return Seek.GetSteering(ownKS, surrogateTarget);
				}	
			}
			#endregion

			#region Right Whisker

			Vector3 rightWhiskerDirection = OrientationToVector(VectorToOrientation(centralDirection) + secondaryWhiskerAngle);

			Debug.DrawRay(ownKS.position, rightWhiskerDirection * lookAheadLength * secondaryWhiskerRatio);

			if (Physics.Raycast(ownKS.position, rightWhiskerDirection, out hit, lookAheadLength * secondaryWhiskerRatio, avoidLayers))
			{
				if (ValidObstacle(hit, scanner))
				{
					surrogateTarget.transform.position = hit.point + hit.normal * avoidDistance;

					Debug.DrawRay(ownKS.position, rightWhiskerDirection * lookAheadLength * secondaryWhiskerRatio, Color.red);

					return Seek.GetSteering(ownKS, surrogateTarget);
				}	
			}
			#endregion

			#region Left Whisker

			Vector3 leftWhiskerDirection = OrientationToVector(VectorToOrientation(centralDirection) - secondaryWhiskerAngle);

			Debug.DrawRay(ownKS.position, leftWhiskerDirection * lookAheadLength * secondaryWhiskerRatio);

			if (Physics.Raycast(ownKS.position, leftWhiskerDirection, out hit, lookAheadLength * secondaryWhiskerRatio, avoidLayers))
			{
				if (ValidObstacle(hit, scanner))
				{
					surrogateTarget.transform.position = hit.point + hit.normal * avoidDistance;

					Debug.DrawRay(ownKS.position, leftWhiskerDirection * lookAheadLength * secondaryWhiskerRatio, Color.red);

					return Seek.GetSteering(ownKS, surrogateTarget);
				}	
			}
            #endregion
			
			return nullSteering;
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