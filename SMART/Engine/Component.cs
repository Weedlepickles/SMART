using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	enum ComponentType { Mesh, MeshRenderer, RigidBody };

	abstract class Component
	{
		public ComponentType Type;
		private SceneObject owner;

		public SceneObject Owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
			}
		}

		public Component()
		{
		}
	}
}
