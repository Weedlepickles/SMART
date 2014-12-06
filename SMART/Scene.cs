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

namespace SMART
{
	class Scene
	{
		//private List<SceneObject> sceneObjects;

		private Skeleton skeleton;
		private Jitter.World world;

		public Scene()
		{
			//We run a CollisionSystem that is fast for up to 30 objects. O(n^2).
			world = new Jitter.World(new Jitter.Collision.CollisionSystemBrute());
			world.Gravity = new JVector(0, -0.0002f, 0);

			//float[,] heights = new float[1,1];
			Shape box = new BoxShape(100, 1, 100);
			RigidBody ground = new RigidBody(box);
			ground.IsStatic = true;
			ground.Position = new JVector(-50, -1, -50);

			//Let there be ground, to separate the void from the bones!
			world.AddBody(ground);
		}

		public void Load()
		{
			skeleton = new Skeleton("Ben", new Vector3(0, 12, -20), world, "EasyBones.skeleton");
			//Load this Scene, maybe nothing needs to be done here?
		}

		public void Update(TimeSpan deltaTime)
		{
			world.Step((float)deltaTime.TotalMilliseconds, false); //Runs without multithreading
		}

		public void Render()
		{
			if (skeleton != null)
				skeleton.Render();
		}
	}
}
