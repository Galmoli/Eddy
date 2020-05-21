using UnityEngine;
using System.Collections;

namespace Steerings
{
	public class Arrive : SteeringBehaviour
	{
		public float closeEnoughRadius = 5f;
		public float slowDownRadius = 20f;
		public float timeToDesiredSpeed = 0.1f; 

		public GameObject target;

		public override  SteeringOutput GetSteering ()
		{
			SteeringOutput result = Arrive.GetSteering (ownKS, target, closeEnoughRadius, slowDownRadius, timeToDesiredSpeed);

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

		public static SteeringOutput GetSteering (KinematicState ownKS, GameObject target, float targetRadius, float slowDownRadius, float timeToDesiredSpeed)
		{
			SteeringOutput steering = new SteeringOutput ();
			Vector3 directionToTarget;
			float distanceToTarget;
			float desiredSpeed;
			Vector3 desiredVelocity;
			Vector3 requiredAcceleration;

			directionToTarget = target.transform.position - ownKS.position;
			distanceToTarget = directionToTarget.magnitude;

			if (distanceToTarget < targetRadius)
			{
				return nullSteering;
			}

			if (distanceToTarget > slowDownRadius)
			{
				return Seek.GetSteering(ownKS, target);
			}
				
			desiredSpeed = ownKS.maxSpeed * (distanceToTarget / slowDownRadius);

			desiredVelocity = directionToTarget.normalized * desiredSpeed;

			requiredAcceleration = (desiredVelocity - ownKS.linearVelocity) / timeToDesiredSpeed;

			if (requiredAcceleration.magnitude > ownKS.maxAcceleration)
			{
				requiredAcceleration = requiredAcceleration.normalized * ownKS.maxAcceleration;
			}

			steering.linearAcceleration = requiredAcceleration;

			return steering;
		}
	}
}