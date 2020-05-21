using UnityEngine;

namespace Steerings
{
	public class Seek : SteeringBehaviour
	{
		public  GameObject target;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = Seek.GetSteering (this.ownKS, this.target);

			if (ownKS.linearVelocity.magnitude > 0.001f)
			{
				transform.rotation = Quaternion.Euler(0, VectorToOrientation(ownKS.linearVelocity), 0);
				ownKS.orientation = transform.rotation.eulerAngles.y;
			}
			result.angularActive = false;

			return result;
		}

		public static SteeringOutput GetSteering (KinematicState ownKS, GameObject target)
		{
			SteeringOutput steering = new SteeringOutput ();
			Vector3 directionToTarget;

			directionToTarget = target.transform.position - ownKS.position;
			directionToTarget.Normalize ();

			steering.linearAcceleration = directionToTarget * ownKS.maxAcceleration;

			return steering;
		}
	}
}
