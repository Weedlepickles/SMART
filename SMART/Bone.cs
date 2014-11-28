using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Engine;

namespace SMART
{
	class Bone
	{
		Bone ParentBone;
		List<Bone> ChildBones = new List<Bone>();

		private Vector3 Position = Vector3.Zero;
		private Vector3 Rotation = Vector3.Zero;

		Vector3 Color = Vector3.One;
		ObjMesh ModelMesh;
		ObjMesh BoneModel;

		public Bone(Vector3 position, ObjMesh nodeModel)
		{
			Position = position;
			ModelMesh = nodeModel;
			BoneModel = nodeModel;
		}

		public void AddChildBone(Bone childBone)
		{
			ChildBones.Add(childBone);
			childBone.SetParent(this);
		}

		protected void SetParent(Bone parentBone)
		{
			ParentBone = parentBone;
		}

		public void SetColor(Color color)
		{
			Color = new Vector3(color.R, color.G, color.B);
			Color.Normalize();
		}

		public void SetRotation(Vector3 rotation)
		{
			Rotation = rotation;
		}

		public List<Bone> GetChildren()
		{
			return ChildBones;
		}

		public void Render(Shader shader, Matrix4 translation)
		{
			Matrix4 ModelMatrix = Matrix4.CreateRotationZ(Rotation[2]) * Matrix4.CreateRotationX(Rotation[0]) * Matrix4.CreateRotationY(Rotation[1]) * Matrix4.CreateTranslation(Position) * translation;
			int mv_matrix_location = GL.GetUniformLocation(shader.Program, "m_matrix");
			GL.UniformMatrix4(mv_matrix_location, false, ref ModelMatrix);
			int color_location = GL.GetUniformLocation(shader.Program, "in_color");
			GL.Uniform3(color_location, ref Color);
			ModelMesh.Render(shader);
			foreach (Bone bone in ChildBones)
			{
				bone.Render(shader, ModelMatrix);
			}
		}

		public void Update(long dt)
		{
			// Update here!

			
			foreach (Bone bone in ChildBones)
			{
				bone.Update(dt);
			}
		}
	}
}
