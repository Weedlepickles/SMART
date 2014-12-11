using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SMART.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Renderer
	{
		private List<ObjMesh> meshes;
		private Vector4 color = new Vector4(1); //Opaque white as base-color
		private Shader shader;

		public Renderer()
		{
			shader = CreateShader("Basic.vertex", "Basic.fragment");
			//color = Set the color!!!!
			meshes = new List<ObjMesh>();
			meshes.Add(new ObjMesh(1, 20));
		}

		public void Render(Matrix4 transformation)
		{
			foreach (ObjMesh mesh in meshes)
			{
				RenderMesh(mesh, transformation);
			}
		}

		private Shader CreateShader(string vertexShaderFileName, string fragmentShaderFileName)
		{
			string vertex_source = System.IO.File.ReadAllText("Shaders/" + vertexShaderFileName);
			string fragment_source = System.IO.File.ReadAllText("Shaders/" + fragmentShaderFileName);

			return new Shader(ref vertex_source, ref fragment_source);
		}

		private void RenderMesh(ObjMesh mesh, Matrix4 transformation)
		{
			//Set the right matrix
			Matrix4 modelMatrix = transformation;
			int modelMatrixGPULocation = GL.GetUniformLocation(shader.Program, "m_matrix");
			GL.UniformMatrix4(modelMatrixGPULocation, false, ref modelMatrix);

			//Set the right color
			int colorGPULocation = GL.GetUniformLocation(shader.Program, "in_color");
			Vector3 baseColor = new Vector3(color.X, color.Y, color.Z);
			GL.Uniform3(colorGPULocation, ref baseColor);

			//Render the mesh
			mesh.Render(shader);

		}

	}
}
