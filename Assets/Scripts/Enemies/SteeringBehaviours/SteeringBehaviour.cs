using UnityEngine;

namespace Steerings
{
	[RequireComponent(typeof(KinematicState))]
    [RequireComponent(typeof(Rigidbody))]

    public class SteeringBehaviour : MonoBehaviour 
	{
		[HideInInspector] public KinematicState ownKS;
        [HideInInspector] public Rigidbody ownRB;

        protected static GameObject surrogateTarget = null;
		protected static SteeringOutput nullSteering;

		protected virtual void Start ()
		{
			ownKS = GetComponent<KinematicState>();
            ownRB = GetComponent<Rigidbody>();

			if (surrogateTarget == null)
			{
				surrogateTarget = new GameObject ("Enemy Surrogate Target");
				surrogateTarget.AddComponent<KinematicState> ();
			}

			if (nullSteering == null)
			{
				nullSteering = new SteeringOutput ();
				nullSteering.linearActive = false;
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

                //ownRB.AddForce(steering.linearAcceleration, ForceMode.Acceleration);
                ownKS.position = ownKS.position + ownKS.linearVelocity * Time.deltaTime + 0.5f * steering.linearAcceleration * Time.deltaTime * Time.deltaTime;
                transform.position = new Vector3(ownKS.position.x, transform.position.y, ownKS.position.z);

				ownKS.position.y = transform.position.y;
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
				
				transform.rotation = Quaternion.Euler(0, ownKS.orientation, 0);
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
			{
				orientation = orientation + 180;
			}

			return orientation;
		}

		private void OnEnable()
		{
			if (ownKS != null) ownKS.position = transform.position;
		}
	}
}
