/* **************** VERSION 2 ****************** */

using UnityEngine;


namespace Steerings
{
	[RequireComponent(typeof(KinematicState))]

	public class SteeringBehaviour : MonoBehaviour 
	{
		protected KinematicState ownKS;

		protected static GameObject SURROGATE_TARGET = null; // all behaviours requiring a surrogate target will use this one
		protected static SteeringOutput NULL_STEERING;

		// BEWARE: made vitual in order to allow redefinitions
		protected virtual void Start ()
		{
			// get a reference to the kinematic state and hold it
			ownKS = GetComponent<KinematicState>();

			if (SURROGATE_TARGET == null) {
				SURROGATE_TARGET = new GameObject ("surrogate target");
				SURROGATE_TARGET.AddComponent<KinematicState> ();
			}

			if (NULL_STEERING == null) {
				NULL_STEERING = new SteeringOutput ();
				NULL_STEERING.linearActive = false;
			}
		}
	
		void Update ()
		{
			SteeringOutput steering = GetSteering ();

            #region Movement
            if (steering.linearActive)
			{
				ownKS.linearVelocity = ownKS.linearVelocity + steering.linearAcceleration * Time.deltaTime;

				if (ownKS.linearVelocity.magnitude > ownKS.maxSpeed)
				{
					ownKS.linearVelocity = ownKS.linearVelocity.normalized * ownKS.maxSpeed;
				}

				ownKS.linearVelocity.y = 0f;

				ownKS.position = ownKS.position + ownKS.linearVelocity * Time.deltaTime + 0.5f * steering.linearAcceleration * Time.deltaTime * Time.deltaTime;

				transform.position = new Vector3(ownKS.position.x, transform.position.y, ownKS.position.z);
			}
			else
			{
				ownKS.linearVelocity = Vector3.zero;
			}
            #endregion

            #region Rotation
            if (steering.angularActive)
			{
				ownKS.angularSpeed = ownKS.angularSpeed + steering.angularAcceleration * Time.deltaTime;

				if (Mathf.Abs(ownKS.angularSpeed) > ownKS.maxAngularSpeed)
				{
					ownKS.angularSpeed = ownKS.maxAngularSpeed * Mathf.Sign(ownKS.angularSpeed);
				}
					
				ownKS.orientation = ownKS.orientation + ownKS.angularSpeed * Time.deltaTime + 0.5f * steering.angularAcceleration * Time.deltaTime * Time.deltaTime;
				
				transform.rotation = Quaternion.Euler(0, 0, ownKS.orientation);
			}
			else
			{
				ownKS.angularSpeed = 0f;
			}
            #endregion
        }

        public virtual SteeringOutput GetSteering ()
		{
			return null;
		}

		public static Vector3 OrientationToVector(float alpha)
		{
			alpha = alpha * Mathf.Deg2Rad;

			float cos = Mathf.Cos(alpha);
			float sin = Mathf.Sin(alpha);

			return new Vector3(sin, 0, cos);
		}

		public static float VectorToOrientation(Vector3 vector)
		{
			Vector3 direction = vector.normalized;

			float sin = direction.x;
			float cos = direction.z;

			float tan = sin / cos;

			float orientation = Mathf.Atan(tan) * Mathf.Rad2Deg;

			if (cos < 0)
				orientation = orientation + 180;

			return orientation;
		}
	}
}
