using UnityEngine;
using System;

namespace Steerings 
{
	public class SteeringOutput
	{
		public Vector3 linearAcceleration = Vector3.zero;
		public float angularAcceleration = 0f;

		public bool linearActive = true;
		public bool angularActive = false;
	}
}

