using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.DataStructures;

namespace SMART
{
	class Scene
	{
		Jitter.Collision.Shapes.SphereShape my_sphere;
		Jitter.Dynamics.RigidBody my_rigidbody;
		float mass = 1;

		public Scene()
		{
			// ######   #     #   ####      #            #
            // #        ##   ##   #   #    # #     ####   #
            // ####     # # # #   ####    #   #            #
            // #        #  #  #   #       #####    ####   #
            // ######   #     #   #      #     #         #

            my_sphere = new Jitter.Collision.Shapes.SphereShape(1);
			my_rigidbody = new Jitter.Dynamics.RigidBody(my_sphere);

			my_rigidbody.AddForce(Jitter.LinearMath.JVector.Down * 5);

			Jitter.Collision.CollisionSystemBrute csb = new Jitter.Collision.CollisionSystemBrute();

			//csb.AddEntity(my_rigidbody);

			Jitter.World my_world = new Jitter.World(csb);

			my_world.AddBody(my_rigidbody);

			my_world.Step(1, false);

		}
	}
}
