using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using SMART.Engine;


//Put this class out of its misera asap
namespace SMART.EngineDeprecated
{
	class RigidBody : Component
	{
		private Shape shape;
		private Jitter.Dynamics.RigidBody rigidBody;

		public RigidBody(Shape shape)
			: base()
		{
			this.shape = shape;
			rigidBody = new Jitter.Dynamics.RigidBody(shape);
		}
	}
}
