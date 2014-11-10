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
		public ObjMesh(string fileName)
		{
			ObjMeshLoader.Load(this, fileName);
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

		int verticesBufferId;
		int VertexStride;
		int VertexStartOffset;
		int NormalStride;
		int NormalStartOffset;
		int TexStride;
		int TexStartOffset;
		int trianglesBufferId;
		int quadsBufferId;

		public void Prepare()
		{
			if (verticesBufferId == 0)
			{
				GL.GenBuffers(1, out verticesBufferId);
				GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(ObjVertex))), vertices, BufferUsageHint.StaticDraw);
				
				VertexStride = Marshal.SizeOf(typeof(Vector2)) + Marshal.SizeOf(typeof(Vector3));
				VertexStartOffset = Marshal.SizeOf(typeof(Vector2)) + Marshal.SizeOf(typeof(Vector3));
				NormalStride = Marshal.SizeOf(typeof(Vector2)) + Marshal.SizeOf(typeof(Vector3));
				NormalStartOffset = Marshal.SizeOf(typeof(Vector2));
				TexStride = 2 * Marshal.SizeOf(typeof(Vector3));
				TexStartOffset = 0;

				Debug.WriteLine("Vertex Stride: " + VertexStride + "  ObjVertex: " + Marshal.SizeOf(typeof(ObjVertex)));
			}

			if (trianglesBufferId == 0)
			{
				GL.GenBuffers(1, out trianglesBufferId);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
				GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(ObjTriangle))), triangles, BufferUsageHint.StaticDraw);
			}

			if (quadsBufferId == 0)
			{
				GL.GenBuffers(1, out quadsBufferId);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
				GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(ObjQuad))), quads, BufferUsageHint.StaticDraw);
			}
		}

		public void Render(int program, string vertexVariableName, string normalVariableName, string texCoordVariableName)
		{
			Prepare();

			int locationOfAttribute = GL.GetAttribLocation(program, vertexVariableName);
			if (locationOfAttribute >= 0)
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
				GL.VertexAttribPointer(locationOfAttribute, 3, VertexAttribPointerType.Float, false, VertexStride, VertexStartOffset);
				GL.EnableVertexAttribArray(locationOfAttribute);
			}
			else
				Debug.WriteLine("DrawModel warning: " + vertexVariableName + " not found in shader!\n");

			if (normalVariableName != null)
			{
				locationOfAttribute = GL.GetAttribLocation(program, normalVariableName);
				if (locationOfAttribute >= 0)
				{
					GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
					GL.VertexAttribPointer(locationOfAttribute, 3, VertexAttribPointerType.Float, false, NormalStride, NormalStartOffset);
					GL.EnableVertexAttribArray(locationOfAttribute);

				}
				else
					Debug.WriteLine("DrawModel warning: " + normalVariableName + " not found in shader!\n");
			}

			// VBO for texture coordinate data NEW for 5b
			if (texCoordVariableName != null)
			{
				locationOfAttribute = GL.GetAttribLocation(program, texCoordVariableName);
				if (locationOfAttribute >= 0)
				{
					GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
					GL.VertexAttribPointer(locationOfAttribute, 2, VertexAttribPointerType.Float, false, TexStride, TexStartOffset);
					GL.EnableVertexAttribArray(locationOfAttribute);
				}
				else
					Debug.WriteLine("DrawModel warning: " + texCoordVariableName + " not found in shader!\n");
			}

			GL.DrawElements(BeginMode.Triangles, triangles.Length, DrawElementsType.UnsignedInt, 0);
		}

		int vao, vb, ib, nb, tb;
		Vector3[] positionArray, normalArray;
		Vector2[] textureCoordinateArray;
		int[] indexArray;

		public static ObjMesh CreateTriangle()
		{
			ObjMesh myTriangle = new ObjMesh();
			myTriangle.Vertices = new ObjMesh.ObjVertex[3];
			myTriangle.Vertices[0] = new ObjMesh.ObjVertex()
			{
				Vertex = new Vector3(-1, 0, 0),
				Normal = new Vector3(0, 0, -1),
				TexCoord = new Vector2(0, 1)
			};

			myTriangle.Vertices[1] = new ObjMesh.ObjVertex()
			{
				Vertex = new Vector3(0, 1, 0),
				Normal = new Vector3(0, 0, -1),
				TexCoord = new Vector2(0.5f, 0)
			};
			myTriangle.Vertices[2] = new ObjMesh.ObjVertex()
			{
				Vertex = new Vector3(1, 0, 0),
				Normal = new Vector3(0, 0, -1),
				TexCoord = new Vector2(1, 1)
			};
			myTriangle.Triangles = new ObjMesh.ObjTriangle[1];
			myTriangle.Triangles[0] = new ObjMesh.ObjTriangle() { Index0 = 0, Index1 = 1, Index2 = 2 };

			myTriangle.Quads = new ObjMesh.ObjQuad[0];

			myTriangle.indexArray = new int[3];
			myTriangle.indexArray[0] = 0;
			myTriangle.indexArray[1] = 1;
			myTriangle.indexArray[2] = 2;
			myTriangle.positionArray = new Vector3[3];
			myTriangle.positionArray[0] = new Vector3(-1, 0, 0);
			myTriangle.positionArray[1] = new Vector3(0, 1, 0);
			myTriangle.positionArray[2] = new Vector3(1, 0, 0);
			myTriangle.normalArray = new Vector3[3];
			myTriangle.normalArray[0] = new Vector3(0, 0, -1);
			myTriangle.normalArray[1] = new Vector3(0, 0, -1);
			myTriangle.normalArray[2] = new Vector3(0, 0, -1);
			myTriangle.textureCoordinateArray = new Vector2[3];
			myTriangle.textureCoordinateArray[0] = new Vector2(0,1);
			myTriangle.textureCoordinateArray[1] = new Vector2(0.5f, 0);
			myTriangle.textureCoordinateArray[2] = new Vector2(1,1);

			myTriangle.BuildTriangle();

			return myTriangle;
		}

		public void BuildTriangle()
		{
			GL.GenVertexArrays(1, out vao);
			GL.GenBuffers(1, out vb);
			GL.GenBuffers(1, out ib);
			GL.GenBuffers(1, out nb);
			if(textureCoordinateArray != null)
				GL.GenBuffers(1, out tb);

			GL.BindVertexArray(vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(positionArray.Length * Marshal.SizeOf(typeof(Vector3))), positionArray, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, nb);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalArray.Length * Marshal.SizeOf(typeof(Vector3))), normalArray, BufferUsageHint.StaticDraw);

			if (textureCoordinateArray != null)
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, tb);
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(textureCoordinateArray.Length * Marshal.SizeOf(typeof(Vector2))), textureCoordinateArray, BufferUsageHint.StaticDraw);

			}
			GL.BindBuffer(BufferTarget.ArrayBuffer, ib);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(indexArray.Length * sizeof(int)), normalArray, BufferUsageHint.StaticDraw);
		}

		public void Render2(int program, string vertexVariableName, string normalVariableName, string textureCoordinateVariableName)
		{
			int loc;

			GL.BindVertexArray(vao);	// Select VAO

			GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
			loc = GL.GetAttribLocation(program, vertexVariableName);
			if (loc >= 0)
			{
				GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, 0, 0);
				GL.EnableVertexAttribArray(loc);
			}
			else
				Debug.WriteLine("DrawModel warning: " + vertexVariableName + " not found in shader!");

			if (normalVariableName != null)
			{
				loc = GL.GetAttribLocation(program, normalVariableName);
				if (loc >= 0)
				{
					GL.BindBuffer(BufferTarget.ArrayBuffer, nb);
					GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, 0, 0);
					GL.EnableVertexAttribArray(loc);
				}
				else
					Debug.WriteLine("DrawModel warning: "+ normalVariableName + " not found in shader!");
			}

			// VBO for texture coordinate data NEW for 5b
			if ((textureCoordinateArray != null) && (textureCoordinateVariableName != null))
			{
				loc = GL.GetAttribLocation(program, textureCoordinateVariableName);
				if (loc >= 0)
				{
					GL.BindBuffer(BufferTarget.ArrayBuffer, tb);
					GL.VertexAttribPointer(loc, 2, VertexAttribPointerType.Float, false, 0, 0);
					GL.EnableVertexAttribArray(loc);
				}
				else
					Debug.WriteLine("DrawModel warning: " + textureCoordinateVariableName + " not found in shader!");
			}

			GL.DrawElements(BeginMode.Triangles, triangles.Length, DrawElementsType.UnsignedInt, 0);
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
