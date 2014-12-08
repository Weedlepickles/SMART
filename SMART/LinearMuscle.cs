using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class LinearMuscle
	{
		private float strength = 0;
		private Bone first, second;
		private float length;
		private float maxForce;
		private float forceFactor = 0.03f;

		public LinearMuscle(Bone first, Bone second, float maxForce)
		{
			this.first = first;
			this.second = second;

			length = (first.RigidBody.Position - second.RigidBody.Position).Length();

			this.maxForce = maxForce;
		}

		/// <summary>
		/// Negative strength contracts the muscle.
		/// </summary>
		public float Strength
		{
			get
			{
				return strength;
			}
			set
			{
				if (value <= 1 && value >= -1)
					strength = value;
				else if (strength > 1)
					strength = 1;
				else
					strength = -1;
			}
		}

		public void UseMuscle()
		{
			JVector forceDirection = first.RigidBody.Position - second.RigidBody.Position;
			float currentLength = forceDirection.Length();
			float lengthDifference = length - currentLength;
			forceDirection.Normalize();

			JVector force;
			if (lengthDifference < 0) //We have exceeded the original, allowed, length
			{
				force = forceDirection * forceFactor * lengthDifference * lengthDifference; //Force it back into place
				if (strength < 0)
					force += forceDirection * strength * maxForce; //Let the muscle power help
			}
			else 
			{
				force = forceDirection * strength * maxForce;
			}

			second.RigidBody.AddForce(force);
			force.Negate();
			first.RigidBody.AddForce(force);

		}
	}
}
