using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	class Material
	{
		private Vector4 color = new Vector4(1); //Opaque white as base-color
		public Shader Shader;

		public Material(Shader shader)
		{
			Shader = shader;
		}

		public Material(string vertexShaderFileName, string fragmentShaderFileName)
		{
			string vertex_source = System.IO.File.ReadAllText("Shaders/" + vertexShaderFileName);
			string fragment_source = System.IO.File.ReadAllText("Shaders/" + fragmentShaderFileName);

			Shader = new Shader(ref vertex_source, ref fragment_source);
		}

		public Vector4 Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}
	}
}
