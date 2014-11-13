using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Skeleton
	{
		Bone RootBone;
		Vector3 Position = Vector3.Zero;
		Vector3 Rotation = Vector3.Zero;

		List<Muscle> Muscles = new List<Muscle>();

		public static Skeleton CreatePants()
		{
			ObjMesh mesh = new ObjMesh(.45f, 8);
			Bone rootBone = new Bone(new Vector3(0, 1, 0), mesh);
			rootBone.SetColor(Color.DarkGoldenrod);

			Bone bone2 = new Bone(Vector3.One, mesh);
			bone2.SetColor(Color.HotPink);

			Vector3 temp = new Vector3(0, 1, 0);
			Bone bone3 = new Bone(temp, mesh);
			bone3.SetColor(Color.LawnGreen);


			rootBone.AddChildBone(bone2);
			bone2.AddChildBone(bone3);

			Skeleton skeleton = new Skeleton(rootBone);
			return skeleton;
		}

		public Skeleton(Bone rootBone)
		{
			RootBone = rootBone;
		}

		public void SetPosition(Vector3 position)
		{
			Position = position;
		}

		public void SetRotation(Vector3 rotation)
		{
			Rotation = rotation;
		}

		public Bone GetRootBone()
		{
			return RootBone;
		}

		public void Render(Shader shader)
		{
			Matrix4 ModelMatrix = Matrix4.CreateRotationZ(Rotation[2]) * Matrix4.CreateRotationX(Rotation[0]) * Matrix4.CreateRotationY(Rotation[1]) * Matrix4.CreateTranslation(Position);

			RootBone.Render(shader, ModelMatrix);
		}

		public void Update(long dt)
		{
			RootBone.Update(dt);
		}
	}
}
