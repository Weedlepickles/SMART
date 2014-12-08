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

		public Bone(string name, Vector3 position, Skeleton owner)
		{
			this.name = name;
			this.owner = owner;
			renderer = new Renderer();
			rigidBody = new RigidBody(new Jitter.Collision.Shapes.SphereShape(1));
			rigidBody.Position = new JVector(position.X, position.Y, position.Z);
			rigidBody.Material.StaticFriction = 0;
			rigidBody.Material.KineticFriction = 0;
			rigidBody.IsParticle = true;
			rigidBody.Mass = 1;

			//TEST REMOVE!!!
			//rigidBody.LinearVelocity = new JVector(0.1f, 0.01f, 0.01f);
			//rigidBody.Force = new JVector(0, -0.00002f, 0);
			//rigidBody.ApplyImpulse( new JVector(0, 4.2f, 0));
			//rigidBody.Damping = Jitter.Dynamics.RigidBody.DampingType.None;
		}

		public void Render()
		{
			Vector3 position = new Vector3(rigidBody.Position.X, rigidBody.Position.Y, rigidBody.Position.Z);
			Matrix4 transformation = Matrix4.CreateTranslation(position) * Matrix4.CreateTranslation(owner.Position);
			renderer.Render(transformation);
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
