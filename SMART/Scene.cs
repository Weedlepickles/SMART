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
using OpenTK.Graphics.OpenGL4;

namespace SMART
{
	class Scene
	{
		//private List<SceneObject> sceneObjects;

		private Skeleton skeleton;
		private Jitter.World world;

		private Camera camera;

		public Scene(float width, float height)
		{
			camera = new Camera(new Vector3(-15, 25, -10), new Vector3((float)Math.PI / 10, (float)Math.PI / 10, 0), width / height);

			CollisionSystemPersistentSAP system = new CollisionSystemPersistentSAP();

			world = new Jitter.World(system);
			world.Gravity = new JVector(0, 0, 0);

			world.SetDampingFactors(0.01f, 0.01f);
			world.SetInactivityThreshold(0, 0, float.MaxValue);
		}

		public void Load()
		{
			skeleton = new Skeleton("Ben", new Vector3(0, 7, -25), world, "Bug.skeleton");

			foreach (LinearMuscle muscle in skeleton.Muscles)
			{
				muscle.Strength = 0.8f;
			}
		}

		public void Update(TimeSpan deltaTime)
		{
			if (skeleton != null && skeleton.Bones != null)
			{
				foreach (LinearMuscle muscle in skeleton.Muscles)
				{
					muscle.UseMuscle();
				}

				Random random = new Random();
				foreach (Bone bone in skeleton.Bones)
				{
					JVector gravity = new JVector(0, -0.000982f, 0);
					//JVector gravity = new JVector(0, 0, 0);
					bone.RigidBody.AddForce(gravity);
					if (bone.RigidBody.Position.Y <= 0)
						bone.RigidBody.AddForce(new JVector(0, -bone.RigidBody.Force.Y, 0));
					//Debug.WriteLine(bone.Name + " is at position: " + bone.RigidBody.Position);
				}

				SatisfyConnections();
			}

			world.Step((float)deltaTime.TotalMilliseconds, false); //Runs without multithreading
		}

		private void SatisfyConnections()
		{
			Dictionary<RigidBody, List<JVector>> storage = new Dictionary<RigidBody, List<JVector>>();

			//Collect all expected positions
			foreach (Connection connection in skeleton.Connections)
			{
				JVector connectionVector = connection.GetConnectionVector();
				float currentLength = connectionVector.Length();

				if (currentLength > connection.MaxLength || currentLength < connection.MinLength)
				{
					float expectedLength;

					if (currentLength > connection.MaxLength)
						expectedLength = connection.MaxLength;
					else
						expectedLength = connection.MinLength;

					float lengthDifference = expectedLength - currentLength;

					RigidBody first = connection.First.RigidBody;
					RigidBody second = connection.Second.RigidBody;

					connectionVector.Normalize();

					JVector firstPosition;
					JVector secondPosition;

					//This is where the bodies should be
					if (first.Position.Y > 0 && second.Position.Y > 0)
					{
						firstPosition = first.Position + connectionVector * lengthDifference * 0.5f;
						secondPosition = second.Position - connectionVector * lengthDifference * 0.5f;
					}
					else if (first.Position.Y <= 0 && second.Position.Y <= 0)
					{
						firstPosition = first.Position;
						secondPosition = second.Position;
					}
					else if (first.Position.Y <= 0)
					{
						firstPosition = first.Position;
						secondPosition = second.Position - connectionVector * lengthDifference;
					}
					else// if (second.Position.Y <= 0)
					{
						firstPosition = first.Position + connectionVector * lengthDifference;
						secondPosition = second.Position;
					}

					//Add the first
					if (storage.ContainsKey(first))
					{
						storage[first].Add(firstPosition);
					}
					else
					{
						List<JVector> positionList = new List<JVector>();
						positionList.Add(firstPosition);
						storage.Add(first, positionList);
					}

					//Add the second
					if (storage.ContainsKey(second))
					{
						storage[second].Add(secondPosition);
					}
					else
					{
						List<JVector> positionList = new List<JVector>();
						positionList.Add(secondPosition);
						storage.Add(second, positionList);
					}
				}
			}

			RigidBody[] bodies = storage.Keys.ToArray<RigidBody>();

			//Put the rigidbody in the mean position
			foreach (RigidBody body in bodies)
			{
				List<JVector> list = storage[body];
				JVector vectorSum = new JVector(0);
				foreach (JVector vector in list)
				{
					vectorSum += vector;
				}
				vectorSum = vectorSum * (1f / list.Count);
				if (vectorSum.Y > 0)
					body.Position = vectorSum; //Change the position
				else
				{
					body.Position = new JVector(vectorSum.X, 0, vectorSum.Z);
				}
			}

		}

		public void Render()
		{
			if (skeleton != null)
				skeleton.Render(camera);
		}
	}
}
