using UnityEngine;

namespace Steerings
{
	public class Align : SteeringBehaviour
	{

		public float closeEnoughAngle = 2f;
		public float slowDownAngle = 10f;
		public float timeToDesiredAngularSpeed = 0.1f;

		public GameObject target;

		public override SteeringOutput GetSteering ()
		{
			return Align.GetSteering (this.ownKS, this.target, this.closeEnoughAngle, this.slowDownAngle, this.timeToDesiredAngularSpeed);
		}

		public static SteeringOutput GetSteering (KinematicState ownKS, GameObject target, float targetAngularRadius = 2f, float slowDownAngularRadius = 10f, float timeToDesiredAngularSpeed = 0.1f)
		{
			SteeringOutput result = new SteeringOutput();
			if (!target) return null;

			result.linearActive = false;
			result.angularActive = true;

			float requiredAngularSpeed;
			float targetOrientation = target.transform.eulerAngles.z;

			float requiredRotation = targetOrientation - ownKS.orientation;

			if (requiredRotation < 0)
			{
				requiredRotation = 360 + requiredRotation;
			}			

			if (requiredRotation > 180)
			{
				requiredRotation = -(360 - requiredRotation);
			}
				
			float rotationSize = Mathf.Abs (requiredRotation); 

			if (rotationSize <= targetAngularRadius)
			{
				return nullSteering;
			}
				
			if (rotationSize > slowDownAngularRadius)
			{
				requiredAngularSpeed = ownKS.maxAngularSpeed;
			}
			else
			{
				requiredAngularSpeed = ownKS.maxAngularSpeed * (rotationSize / slowDownAngularRadius);
			}
			
			requiredAngularSpeed = requiredAngularSpeed * Mathf.Sign (requiredRotation);

			result.angularAcceleration = (requiredAngularSpeed - ownKS.angularSpeed)/timeToDesiredAngularSpeed;

			if (Mathf.Abs (result.angularAcceleration) > ownKS.maxAngularAcceleration)
			{
				result.angularAcceleration = ownKS.maxAngularAcceleration * Mathf.Sign(result.angularAcceleration);
			}			

			return result;
		}
	
	}
}