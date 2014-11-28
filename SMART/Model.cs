using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Engine;

namespace SMART
{
	class Model
	{
		ObjMesh ModelMesh;
		Vector3 Translation;
		Vector3 Rotation;

		#region Constructors
		public Model()
		{
			Translation = Vector3.Zero;
			Rotation = Vector3.Zero;
		}
		public Model(Vector3 translation)
		{
			Translation = translation;
			Rotation = Vector3.Zero;
		}
		public Model(Vector3 translation, Vector3 rotation)
		{
			Translation = translation;
			Rotation = rotation;
		}
		public Model(Vector3 translation, Vector3 rotation, ObjMesh modelMesh)
		{
			Translation = translation;
			Rotation = rotation;
			ModelMesh = modelMesh;
		}
		#endregion

		#region Getters and Setters
		public void SetTranslation(Vector3 translation)
		{
			Translation = translation;
		}
		public void SetRotation(Vector3 rotation)
		{
			Rotation = rotation;
		}
		public void SetModelMesh(ObjMesh modelMesh)
		{
			ModelMesh = modelMesh;
		}
		#endregion

		public void Render(Shader shader)
		{

		}
	}
}
