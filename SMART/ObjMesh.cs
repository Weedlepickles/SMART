using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	public class ObjMesh
	{
        public enum MeshShape
        {
            Sphere
        };
        
        private VertexFloatBuffer mMeshBuffer;
        
        public ObjMesh(string fileName)
		{
			ObjMeshLoader.LoadObj(this, fileName);
		}

        /// <summary>
        /// Use this constructor to create a sphere
        /// </summary>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="resolution">The number of segments of the sphere</param>
        public ObjMesh(float radius, int resolution)
        {
            ObjMeshLoader.LoadSphere(this, radius, resolution);
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

        public void Prepare()
        {
            mMeshBuffer = new VertexFloatBuffer(VertexFormat.XYZ_NORMAL_UV, Vertices.Length);
            foreach (ObjVertex v in Vertices)
            {
                mMeshBuffer.AddVertex(v.Vertex[0], v.Vertex[1], v.Vertex[2], v.Normal[0], v.Normal[1], v.Normal[2], v.TexCoord[0], v.TexCoord[1]);
            }
            mMeshBuffer.IndexFromLength();
            mMeshBuffer.Load();
        }

        public void Render(Shader shader)
        {
            mMeshBuffer.Bind(shader);
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
