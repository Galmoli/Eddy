using UnityEngine;

namespace Steerings
{
	public class KinematicState : MonoBehaviour
	{
		public float maxAcceleration;
		public float maxSpeed;
		public float maxAngularAcceleration;
		public float maxAngularSpeed;

		[HideInInspector] public Vector3 position;
		[HideInInspector] public float orientation;
		[HideInInspector] public Vector3 linearVelocity;
		[HideInInspector] public float angularSpeed;

		void Start ()
		{
			position = transform.position;
			orientation = transform.eulerAngles.y;
			linearVelocity = Vector3.zero;
			angularSpeed = 0f;
		}
	}
}