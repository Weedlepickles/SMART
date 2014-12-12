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

		public Renderer(Vector4 color)
		{
			shader = CreateShader("Basic.vertex", "Basic.fragment");
			this.color = color;
			meshes = new List<ObjMesh>();
			meshes.Add(new ObjMesh(1, 20));
		}
		public Renderer(ObjMesh mesh, Vector4 color)
		{
			shader = CreateShader("Basic.vertex", "Basic.fragment");
			this.color = color;
			meshes = new List<ObjMesh>();
			meshes.Add(mesh);
		}

		public void Render(Camera camera, Matrix4 transformation)
		{
			//Use this specific shader to render this model
			GL.UseProgram(shader.Program);

			Matrix4 viewMatrix = camera.ViewMatrix;

			//Tell the GPU where it is we are looking
			int view_matrix_location = GL.GetUniformLocation(shader.Program, "view_matrix");
			GL.UniformMatrix4(view_matrix_location, false, ref viewMatrix);

			foreach (ObjMesh mesh in meshes)
			{
				RenderMesh(mesh, transformation);
			}

			//Stop using the shader
			GL.UseProgram(0);
		}

		private Shader CreateShader(string vertexShaderFileName, string fragmentShaderFileName)
		{
			string vertex_source = System.IO.File.ReadAllText("Shaders/" + vertexShaderFileName);
			string fragment_source = System.IO.File.ReadAllText("Shaders/" + fragmentShaderFileName);

			return new Shader(ref vertex_source, ref fragment_source);
		}

		private void RenderMesh(ObjMesh mesh, Matrix4 transformation)
		{
			//Tell the GPU where the model is
			Matrix4 modelMatrix = transformation;
			int modelMatrixGPULocation = GL.GetUniformLocation(shader.Program, "model_matrix");
			GL.UniformMatrix4(modelMatrixGPULocation, false, ref modelMatrix);

			//Tel the GPU what color the model vertices have
			int colorGPULocation = GL.GetUniformLocation(shader.Program, "in_color");
			Vector3 baseColor = new Vector3(color.X, color.Y, color.Z);
			GL.Uniform3(colorGPULocation, ref baseColor);

			//Render the model
			mesh.Render(shader);

		}

	}
}
