using UnityEngine;

namespace Steerings
{
	public class LinearRepulsion : SteeringBehaviour
	{
		public string idTag = "Enemy";
		public float repulsionThreshold = 20f;

		public override SteeringOutput GetSteering ()
		{
			SteeringOutput result = LinearRepulsion.GetSteering(this.ownKS, this.idTag, this.repulsionThreshold);
			if (!surrogateTarget) return null;

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

		public static SteeringOutput GetSteering (KinematicState ownKS, string tag, float repulsionThreshold)
		{
			Vector3 directionToTarget;
			float distanceToTarget;
			float repulsionStrength = 0;
			int activeTargets = 0;

			SteeringOutput result = new SteeringOutput ();

			GameObject[] targets = GameObject.FindGameObjectsWithTag (tag);

			foreach (GameObject target in targets)
			{
				if (target== ownKS.gameObject) continue;

				directionToTarget = target.transform.position - ownKS.position;
				distanceToTarget = directionToTarget.magnitude;

				if (distanceToTarget <= repulsionThreshold)
				{
					activeTargets++;
					repulsionStrength = ownKS.maxAcceleration * (repulsionThreshold - distanceToTarget) / repulsionThreshold;
					result.linearAcceleration = result.linearAcceleration - directionToTarget.normalized * repulsionStrength;
				}	
			} 

			if (activeTargets > 0)
			{
				if (result.linearAcceleration.magnitude > ownKS.maxAcceleration)
				{
					result.linearAcceleration = result.linearAcceleration.normalized * ownKS.maxAcceleration;
				}
					
				return result;
			}
			else
			{
				return nullSteering;
			}			
		}

	}
}