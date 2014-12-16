﻿using Jitter.LinearMath;
using OpenTK;
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
		private float maxForce;
		private const float forceFactor = 0.00001f;
		private Skeleton owner;

		private Connection connection;

		public LinearMuscle(Skeleton owner, Bone first, Bone second, float maxForce)
		{
			this.first = first;
			this.second = second;
			this.owner = owner;
			connection = new Connection(owner, first, second, new Vector4(0, 0, 1, 1));
			connection.MinLength = 0.80f * connection.MaxLength;

			this.maxForce = maxForce;
		}

		/// <summary>
		/// A value between 1 and -1. Positive strength contracts the muscle, negative strength expands the muscle. 
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
				float saturation1 = (float)(-strength * 0.5) + 0.5f;
				float saturation2 = (float)(strength * 0.5) + 0.5f;
				connection.Color = new Vector4(saturation1, saturation1, saturation2, 1);
			}
		}

		public Connection Connection
		{
			get
			{
				return connection;
			}
			private set
			{
				connection = value;
			}
		}

		public void UseMuscle()
		{
			JVector forceDirection = first.RigidBody.Position - second.RigidBody.Position;
			float currentLength = forceDirection.Length();
			forceDirection.Normalize();

			JVector force = JVector.Zero;
			//Contract the muscle unless it's too close to the minimum length
			if (strength > 0 && currentLength >= connection.MinLength * 1.02f)
			{
				force = forceDirection * strength * maxForce * forceFactor;
			}
			//Expand the muscle unless it's too close to the maximum length
			else if (strength < 0 && currentLength <= connection.MaxLength * 0.98f)
			{
				force = forceDirection * strength * maxForce * forceFactor;
			}

			//second is on the ground and first is not on the ground
			if (second.RigidBody.Position.Y < 0.01 && first.RigidBody.Position.Y >= 0.01)
			{
				second.RigidBody.AddForce(new JVector(0.1f * force.X, force.Y, 0.1f * force.Z));
				force.Negate();
				first.RigidBody.AddForce(new JVector(0.9f * force.X, force.Y, 0.9f * force.Z));
			}
			//second is not on the ground and first is on the ground
			else if (second.RigidBody.Position.Y >= 0.01 && first.RigidBody.Position.Y < 0.01)
			{
				second.RigidBody.AddForce(new JVector(0.9f * force.X, force.Y, 0.9f * force.Z));
				force.Negate();
				first.RigidBody.AddForce(new JVector(0.1f * force.X, force.Y, 0.1f * force.Z));
			}
			else
			{
				second.RigidBody.AddForce(force);
				force.Negate();
				first.RigidBody.AddForce(force);
			}
		}

		/// <summary>
		/// Returns a value between 0 and 1 where 0 is its most contracted state, and 1 is the opposite
		/// </summary>
		/// <returns></returns>
		public float GetState()
		{
			JVector forceDirection = first.RigidBody.Position - second.RigidBody.Position;
			float currentLength = forceDirection.Length() - connection.MinLength;
			float maxLength = connection.MaxLength - connection.MinLength;
			return currentLength / maxLength;
		}
	}
}
