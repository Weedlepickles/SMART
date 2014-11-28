using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	class Mesh : Component
	{
		public ObjMesh ObjMesh;

		public Mesh()
			: base()
		{
			Type = ComponentType.Mesh;
		}

		public Mesh(ObjMesh mesh)
			: base()
		{
			Type = ComponentType.Mesh;
			ObjMesh = mesh;
		}
	}
}
