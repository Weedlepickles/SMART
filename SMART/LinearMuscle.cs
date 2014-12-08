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

		public LinearMuscle (Bone first, Bone second, float maxForce)
		{
			this.first = first;
			this.second = second;

			length = (first.RigidBody.Position - second.RigidBody.Position).Length();

			this.maxForce = maxForce;
		}

		public float Strength
		{
			get
			{
				return strength;
			}
			set
			{
				if (value <= 1)
					strength = value;
				else
					strength = 1;
			}
		}

		public void UseMuscle()
		{
			JVector forceDirection = first.RigidBody.Position - second.RigidBody.Position;
			float currentLength = forceDirection.Length();
			forceDirection.Normalize();

			//Let the muscle have less power near the edges
			float extensionFactor = currentLength / length;
			if (extensionFactor > 0.8f)
			{
				extensionFactor = (1 - extensionFactor) * 5;
			}
			else if (extensionFactor >= 0.2f)
			{
				extensionFactor = 1;
			}
			else
			{
				extensionFactor = extensionFactor * 5;
			}

			first.RigidBody.AddForce(forceDirection * strength * maxForce * extensionFactor);
			second.RigidBody.AddForce(forceDirection * -strength * maxForce * extensionFactor);
		}
	}
}
