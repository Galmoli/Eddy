/* **************** VERSION 2 ****************** */

using UnityEngine;
using System;

namespace Steerings 
{
	public class SteeringOutput
	{
		public Vector3 linearAcceleration = Vector3.zero;
		public float angularAcceleration = 0f; // in degs per second squared

		// used in order to know which(s) acceleration(s) apply
		public bool linearActive = true;
		public bool angularActive = false; // !!!
	}
}

