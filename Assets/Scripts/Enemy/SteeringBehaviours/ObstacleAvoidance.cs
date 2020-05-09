using UnityEngine;

namespace Steerings
{
	public class ObstacleAvoidance : SteeringBehaviour
	{
		public bool showWhisker = true;
		public float lookAheadLength = 10f;
		public float avoidDistance = 10f;
		public float secondaryWhiskerAngle = 30f;
		public float secondaryWhiskerRatio = 0.7f;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = ObstacleAvoidance.GetSteering (this.ownKS, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio);

			if (ownKS.linearVelocity.magnitude > 0.001f)
			{
				transform.rotation = Quaternion.Euler(0, VectorToOrientation(ownKS.linearVelocity), 0);
				ownKS.orientation = transform.rotation.eulerAngles.z;
			}
			result.angularActive = false;

			return result;
		}

		public static SteeringOutput GetSteering (KinematicState ownKS, float lookAheadLength, float avoidDistance, float secondaryWhiskerAngle, float secondaryWhiskerRatio)
		{
			Vector3 mainDirection;
			Vector3 whisker1Direction, whisker2Direction, whisker3Direction;
			RaycastHit hit;

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

			whisker1Direction = mainDirection;
			whisker2Direction = OrientationToVector (VectorToOrientation (mainDirection) + secondaryWhiskerAngle);
			whisker3Direction = OrientationToVector (VectorToOrientation (mainDirection) - secondaryWhiskerAngle);

			Debug.DrawRay (ownKS.position, whisker1Direction * lookAheadLength);
			Debug.DrawRay (ownKS.position, whisker2Direction * lookAheadLength * secondaryWhiskerRatio);
			Debug.DrawRay (ownKS.position, whisker3Direction * lookAheadLength * secondaryWhiskerRatio);

            #region Whisker 1
            if (Physics.Raycast(ownKS.position, whisker1Direction, out hit, lookAheadLength)) 
			{
				SURROGATE_TARGET.transform.position = hit.point + hit.normal * avoidDistance;
				
				if (collider != null)
				{
					collider.enabled = before;
				}
				
				Debug.DrawRay (ownKS.position, whisker1Direction * lookAheadLength, Color.red);
				
				return Seek.GetSteering(ownKS, SURROGATE_TARGET);
			}
            #endregion

            #region Whisker 2
            if (Physics.Raycast(ownKS.position, whisker2Direction, out hit, lookAheadLength * secondaryWhiskerRatio))
			{
				SURROGATE_TARGET.transform.position = hit.point + hit.normal * avoidDistance;

				if (collider != null)
				{
					collider.enabled = before;
				}

				Debug.DrawRay (ownKS.position, whisker2Direction * lookAheadLength*secondaryWhiskerRatio, Color.red);
				
				return Seek.GetSteering (ownKS, SURROGATE_TARGET);
			}
            #endregion

            #region Whisker 3
            if (Physics.Raycast(ownKS.position, whisker3Direction, out hit, lookAheadLength * secondaryWhiskerRatio)) {

				SURROGATE_TARGET.transform.position = hit.point + hit.normal * avoidDistance;

				if (collider != null) {
					collider.enabled = before;
				}

				Debug.DrawRay (ownKS.position, whisker3Direction * lookAheadLength*secondaryWhiskerRatio, Color.red);

				return Seek.GetSteering (ownKS, SURROGATE_TARGET);
			}
            #endregion

            if (collider != null)
			{
				collider.enabled = before;
			}
			
			return NULL_STEERING;
		}
	}
}