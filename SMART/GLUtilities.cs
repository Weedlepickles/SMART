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
	class GLUtilities
	{
		public static int LoadShader(string FileName, ShaderType ShaderType)
		{
			string shaderCode = System.IO.File.ReadAllText(FileName);
			int shader = GL.CreateShader(ShaderType);

			CompileShader(shader, shaderCode);
			
			return shader;
		}

		public static int CreateProgram(params int[] shaders)
		{
			int program = GL.CreateProgram();

			foreach (int shader in shaders)
			{
				GL.AttachShader(program, shader);
			}

			GL.LinkProgram(program);

			// Output link info log.
			string info = "";
			GL.GetProgramInfoLog(program, out info);
			Debug.WriteLine(info);

			return program;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ObjVertex
		{
			public Vector2 TexCoord;
			public Vector3 Normal;
			public Vector3 Vertex;
		}
		
		#region private

		private static void CompileShader(int shader, string source)
		{
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);

			string info;
			GL.GetShaderInfoLog(shader, out info);
			Debug.WriteLine(info);

			int compileResult;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compileResult);
			if (compileResult != 1)
			{
				Debug.WriteLine("Compile Error!");
				Debug.WriteLine(source);
				throw new Exception("Shader compile error:\nInfo:"+info+"\n" + source);
			}
		}
		#endregion
	}
}
