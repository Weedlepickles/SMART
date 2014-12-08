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
		private float forceFactor = 0.05f;
		private JVector connectionVector;
		private float length;

		public Connection(Bone parent, Bone child)
		{
			this.parent = parent;
			this.child = child;
			connectionVector = child.RigidBody.Position - parent.RigidBody.Position;
			length = connectionVector.Length();

		}

		public void ForceConnection()
		{
			JVector connection = child.RigidBody.Position - parent.RigidBody.Position;
			float currentLength = connection.Length();
			float lengthDifference = length - currentLength;
			connection.Normalize();
			JVector force;

			force = connection * forceFactor * lengthDifference;

			child.RigidBody.ApplyImpulse(force);
			force.Negate();
			parent.RigidBody.ApplyImpulse(force);
		}
	}
}
