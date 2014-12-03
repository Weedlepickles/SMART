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
	class Bone : SceneObject
	{
		public Bone(string name, Transform transform, Material material)
			: base(name, transform)
		{
			Mesh mesh = new Mesh(new ObjMesh(.30f, 16));
			MeshRenderer meshRenderer = new MeshRenderer(material);
			this.Add(mesh);
			this.Add(meshRenderer);
		}
	}
}
