using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Engine;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace SMART
{
	class Bone
	{
		private string name;
		private Skeleton owner;
		private Renderer renderer;
		private RigidBody rigidBody;
		private bool frozen = false;

		public Bone(string name, Vector3 position, Skeleton owner)
		{
			this.name = name;
			this.owner = owner;
			renderer = new Renderer(new Vector4(0, 1, 0, 1));
			rigidBody = new RigidBody(new Jitter.Collision.Shapes.SphereShape(1));
			rigidBody.Position = new JVector(position.X, position.Y, position.Z);
			rigidBody.Material.StaticFriction = 0;
			rigidBody.Material.KineticFriction = 0;
			rigidBody.Mass = 1;
		}

		public void Render(Camera camera)
		{
			Vector3 position = new Vector3(rigidBody.Position.X, rigidBody.Position.Y, rigidBody.Position.Z);
			Matrix4 transformation = Matrix4.CreateTranslation(position) * Matrix4.CreateTranslation(owner.Position);
			renderer.Render(camera, transformation);
		}

		public string Name
		{
			get
			{
				return name;
			}
			private set
			{
				name = value;
			}
		}
		public bool isFrozen
		{
			get
			{
				return frozen;
			}
			set
			{
				frozen = value;
			}
		}

		public RigidBody RigidBody
		{
			get
			{
				return rigidBody;
			}
			private set
			{
				rigidBody = value;
			}
		}
	}
}
