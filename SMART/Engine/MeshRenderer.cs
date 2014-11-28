using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	class MeshRenderer : Component
	{
		public Material Material;

		public MeshRenderer()
			: base()
		{
			Type = ComponentType.MeshRenderer;
		}
		public MeshRenderer(Material material)
			: base()
		{
			Type = ComponentType.MeshRenderer;
			Material = material;
		}

		public void Render()
		{
			if (Material != null)
			{
				List<Component> components = Owner.GetComponents(ComponentType.Mesh);
				foreach (Component component in components)
				{
					RenderMesh((Mesh)component);
				}
			}
		}

		private void RenderMesh(Mesh mesh)
		{
			//Set the right matrix
			Matrix4 modelMatrix = Owner.Transform.TransformationMatrix;// Make sure to add supoort for parent movement (*parentTranslation);
			int modelMatrixGPULocation = GL.GetUniformLocation(Material.Shader.Program, "m_matrix");
			GL.UniformMatrix4(modelMatrixGPULocation, false, ref modelMatrix);

			//Set the right color
			int colorGPULocation = GL.GetUniformLocation(Material.Shader.Program, "in_color");
			Vector3 baseColor = new Vector3(Material.Color.X, Material.Color.Y, Material.Color.Z);
			GL.Uniform3(colorGPULocation, ref baseColor);

			//Render the mesh
			mesh.ObjMesh.Render(Material.Shader);

		}
	}
}
