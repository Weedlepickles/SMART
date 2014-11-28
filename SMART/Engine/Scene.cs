using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.DataStructures;

namespace SMART.Engine
{
	class Scene
	{
		public List<SceneObject> SceneObjects = new List<SceneObject>();

		public Scene()
		{
			//What to do, what to do?
		}

		public void Load()
		{
			//Load this Scene, maybe nothing needs to be done here?
		}

		public void Update(TimeSpan deltaTime)
		{

		}

		public void Render()
		{
			//Go through all SceneObjects' with a MeshRenderer Component
			foreach (SceneObject sceneObject in SceneObjects)
			{
				sceneObject.CallProcess(ProcessType.Render, null);
			}
		}

		#region Remove if we got working physics
		//Jitter.Collision.Shapes.SphereShape my_sphere;
		//Jitter.Dynamics.RigidBody my_rigidbody;
		//float mass = 1;

		//public Scene()
		//{
		//	// ######   #     #   ####      #            #
		//	// #        ##   ##   #   #    # #     ####   #
		//	// ####     # # # #   ####    #   #            #
		//	// #        #  #  #   #       #####    ####   #
		//	// ######   #     #   #      #     #         #

		//	my_sphere = new Jitter.Collision.Shapes.SphereShape(1);
		//	my_rigidbody = new Jitter.Dynamics.RigidBody(my_sphere);

		//	my_rigidbody.AddForce(Jitter.LinearMath.JVector.Down * 5);

		//	Jitter.Collision.CollisionSystemBrute csb = new Jitter.Collision.CollisionSystemBrute();

		//	//csb.AddEntity(my_rigidbody);

		//	Jitter.World my_world = new Jitter.World(csb);

		//	my_world.AddBody(my_rigidbody);

		//	my_world.Step(1, false);

		//}

		//public float GetGroundLevel(float x, float z)
		//{
		//	return 0;
		//}
		#endregion
	}
}
