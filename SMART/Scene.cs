using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.DataStructures;
using OpenTK;
using Jitter.LinearMath;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using System.Diagnostics;
using Jitter.Dynamics.Constraints;
using Jitter.Collision;

namespace SMART
{
	class Scene
	{
		//private List<SceneObject> sceneObjects;

		private Skeleton skeleton;
		private Jitter.World world;

		public Scene()
		{
			CollisionSystemPersistentSAP system = new CollisionSystemPersistentSAP();


			world = new Jitter.World(system);
			world.Gravity = new JVector(0, 0, 0);

			//float[,] heights = new float[1,1];
			Shape box = new BoxShape(100, 1, 100);
			RigidBody ground = new RigidBody(box);
			ground.IsStatic = true;
			ground.Position = new JVector(0, -1, 0);

			//Let there be ground, to separate the void from the bones!
			//world.AddBody(ground);

			world.SetDampingFactors(0.1f, 0.1f);
			world.SetInactivityThreshold(0, 0, float.MaxValue);
		}

		public void Load()
		{
			skeleton = new Skeleton("Ben", new Vector3(0, 7, -20), world, "Bug.skeleton");

			foreach (LinearMuscle muscle in skeleton.Muscles)
			{
				muscle.Strength = 0;
			}
			//Load this Scene, maybe nothing needs to be done here?
		}

		public void Update(TimeSpan deltaTime)
		{
			if (skeleton != null && skeleton.Bones != null)
			{

				foreach (Connection connection in skeleton.Connections)
				{
					connection.ForceConnection();
				}

				foreach (LinearMuscle muscle in skeleton.Muscles)
				{
					//muscle.UseMuscle();
				}

				Random random = new Random();
				foreach (Bone bone in skeleton.Bones)
				{
					bone.RigidBody.IsActive = true;
					bone.RigidBody.IsStatic = false;

					/*
					float temp1 = 1;
					float temp2 = 1;
					float temp3 = 1;
					if (random.NextDouble() > 0.5d)
						temp1 *= -1;
					if (random.NextDouble() > 0.5d)
						temp2 *= -1;
					if (random.NextDouble() > 0.5d)
						temp3 *= -1;

					JVector gravity = new JVector(0.001f * temp1 * (float)random.NextDouble(), 0.001f * temp2 * (float)random.NextDouble(), 0.001f * temp3 * (float)random.NextDouble());

					 */
					JVector gravity = new JVector(0, -0.000982f, 0);

					if (bone.RigidBody.Position.Y > 0.0001f)
					{
						bone.RigidBody.AddForce(gravity);
					}
					else
					{
						bone.RigidBody.AddForce(-0.005f * gravity);
					}

					//else if (bone.RigidBody.Position.Y <= 0.25f)
					//{
					//	bone.RigidBody.Force = new JVector(bone.RigidBody.Force.X * 0.5f, bone.RigidBody.Force.Y * 0.5f, bone.RigidBody.Force.Z * 0.5f);
					//	if (bone.RigidBody.LinearVelocity.Y < 0)
					//		bone.RigidBody.LinearVelocity = new JVector(bone.RigidBody.LinearVelocity.X, bone.RigidBody.LinearVelocity.Y * -0.5f, bone.RigidBody.LinearVelocity.Z);
					//}
				}

				foreach (Bone bone in skeleton.Bones)
				{
					if (bone.RigidBody.Position.Y <= 0)
					{
						bone.RigidBody.Position = new JVector(bone.RigidBody.Position.X, 0, bone.RigidBody.Position.Z);
						bone.RigidBody.Force = new JVector(bone.RigidBody.Force.X, 0, bone.RigidBody.Force.Z);
						if (bone.RigidBody.LinearVelocity.Y < 0)
							bone.RigidBody.LinearVelocity = new JVector(bone.RigidBody.LinearVelocity.X, bone.RigidBody.LinearVelocity.Y * -0.5f, bone.RigidBody.LinearVelocity.Z);
					}
				}
			}

			world.Step((float)deltaTime.TotalMilliseconds, false); //Runs without multithreading
		}

		public void Render()
		{
			if (skeleton != null)
				skeleton.Render();
		}
	}
}
