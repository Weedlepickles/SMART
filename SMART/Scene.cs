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

		private List<Skeleton> mSkeletons;
        private SMARTWorld mWorld;
		private Camera camera;
        public CowSkeleton cow;

		public Scene(float width, float height)
		{
			camera = new Camera(new Vector3(0, 6, 30), new Vector3(0, 0, 0), width / height);
            mSkeletons = new List<Skeleton>();
            mWorld = new SMARTWorld(new CollisionSystemPersistentSAP());
		}

		public Camera Camera
		{
			get
			{
				return camera;
			}
			private set
			{
				camera = value;
			}
		}

		public void LoadSkeleton(string skeletonFileName)
		{
			mSkeletons.Add(new Skeleton("Ben", new Vector3(0, 8, 0), mWorld, skeletonFileName));
		}

        public void LoadCowSkeleton(string skeletonFileName)
        {
            cow = new CowSkeleton("Cow", new Vector3(0, 8, 0), mWorld, skeletonFileName);
            mSkeletons.Add(cow);
        }

        public void Reset()
        {
            foreach (Skeleton s in mSkeletons)
            {
                s.Reset();
            }
        }

		public void Update(TimeSpan deltaTime)
		{
            foreach (Skeleton s in mSkeletons)
            {
                s.Update(deltaTime);
            }

            mWorld.Update(deltaTime);
		}

		public void Render()
		{
            mWorld.Render(camera);

            foreach (Skeleton s in mSkeletons)
            {
                s.Render(camera);
            }
		}
	}
}
