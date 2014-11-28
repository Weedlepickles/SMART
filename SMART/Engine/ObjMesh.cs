using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	public class ObjMesh
	{
        private VertexFloatBuffer MeshBuffer;
        
        public ObjMesh(string fileName)
		{
			ObjMeshLoader.LoadObj(this, fileName);
			CreateAndLoadVBO();
		}

        /// <summary>
        /// Use this constructor to create a sphere
        /// </summary>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="resolution">The number of segments of the sphere</param>
        public ObjMesh(float radius, int resolution)
        {
            ObjMeshLoader.LoadSphere(this, radius, resolution);
			CreateAndLoadVBO();
        }

		/// <summary>
		/// Use this constructor to create a cylinder
		/// </summary>
		/// <param name="radius">Radius of the cylinder</param>
		/// <param name="length">Length of the cylinder</param>
		/// <param name="resolution">The number of segments of the cylinder</param>
		public ObjMesh(float radius, float length, int resolution)
		{
			ObjMeshLoader.LoadCylinder(this, radius, length, resolution);
			CreateAndLoadVBO();
		}

		public ObjMesh()
		{
		}

		public ObjVertex[] Vertices
		{
			get { return vertices; }
			set { vertices = value; }
		}
		ObjVertex[] vertices;

		public ObjTriangle[] Triangles
		{
			get { return triangles; }
			set { triangles = value; }
		}
		ObjTriangle[] triangles;

		public ObjQuad[] Quads
		{
			get { return quads; }
			set { quads = value; }
		}
		ObjQuad[] quads;

        private void CreateAndLoadVBO()
        {
            if (triangles != null && triangles.Length > 0)
            {
                MeshBuffer = new VertexFloatBuffer(VertexFormat.XYZ_NORMAL_UV, Triangles.Length * 3);
				foreach (ObjTriangle t in Triangles)
                {
                    AddVertexToMeshBuffer(Vertices[t.Index0]);
                    AddVertexToMeshBuffer(Vertices[t.Index1]);
                    AddVertexToMeshBuffer(Vertices[t.Index2]);
                }
            }
            else
            {
                MeshBuffer = new VertexFloatBuffer(VertexFormat.XYZ_NORMAL_UV, Vertices.Length);
                foreach (ObjVertex v in Vertices)
                {
                    AddVertexToMeshBuffer(v);
                }
            }
            
            MeshBuffer.IndexFromLength();
            MeshBuffer.Load();
        }

        public void Render(Shader shader)
        {
			MeshBuffer.Bind(shader);
        }

        private void AddVertexToMeshBuffer(ObjVertex v)
        {
            MeshBuffer.AddVertex(v.Vertex[0], v.Vertex[1], v.Vertex[2], v.Normal[0], v.Normal[1], v.Normal[2], v.TexCoord[0], v.TexCoord[1]);
        }

		[StructLayout(LayoutKind.Sequential)]
		public struct ObjVertex
		{
			public Vector2 TexCoord;
			public Vector3 Normal;
			public Vector3 Vertex;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ObjTriangle
		{
			public int Index0;
			public int Index1;
			public int Index2;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ObjQuad
		{
			public int Index0;
			public int Index1;
			public int Index2;
			public int Index3;
		}
	}
}
