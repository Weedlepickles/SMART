using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Connection
	{
		private Bone parent, child;
		private float impulseFactor = 0.01f;
		private JVector connectionVector;
		private float length;
		private float stabilizer = 1;

		public Connection(Bone parent, Bone child, float impulseFactor)
		{
			this.parent = parent;
			this.child = child;
			connectionVector = child.RigidBody.Position - parent.RigidBody.Position;
			length = connectionVector.Length();
			this.impulseFactor = impulseFactor;
		}

		public void ForceConnection()
		{
			JVector connection = child.RigidBody.Position - parent.RigidBody.Position;
			float currentLength = connection.Length();
			float lengthDifference = length - currentLength;
			connection.Normalize();
			JVector force;
			if (lengthDifference> 0)
			{
				force= connection * impulseFactor * lengthDifference * lengthDifference;
			}
			else
			{
				force = connection * impulseFactor * -lengthDifference * lengthDifference;
			}
			child.RigidBody.AddForce(force);
			force.Negate();
			parent.RigidBody.AddForce(force);
		}
	}
}
