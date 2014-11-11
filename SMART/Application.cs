using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace SMART
{
	public class Application : GameWindow
	{
		private ObjMesh Sphere;

        private Matrix4 ProjectionMatrix;
        private Matrix4 WorldMatrix;
        private Matrix4 ModelviewMatrix;

        private Vector3 CameraPosition;
        private Shader Shader;

		public Application()
			: base(400, 400)
		{
			this.RenderFrame += new EventHandler<FrameEventArgs>(OnRenderFrame);
            this.Resize += new EventHandler<EventArgs>(OnResize);
			this.Load += new EventHandler<EventArgs>(OnLoad);
            this.UpdateFrame += new EventHandler<FrameEventArgs>(OnUpdateFrame);
			new Scene();
		}

		private void OnLoad(object sender, EventArgs e)
		{
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 0.1f, 1000.0f);
            WorldMatrix = new Matrix4();
            ModelviewMatrix = new Matrix4();

            CameraPosition = new Vector3(0, 0, 0);

            string vertex_source = System.IO.File.ReadAllText("Shaders/Basic.vertex");
            string fragment_source = System.IO.File.ReadAllText("Shaders/Basic.fragment");

            Shader = new Shader(ref vertex_source, ref fragment_source);

            //Sphere = new ObjMesh("Models/tetrahedron.obj");
            Sphere = new ObjMesh(0.7f, 32);

            //Prepare meshes here
            Sphere.Prepare();

		}

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            WorldMatrix = Matrix4.CreateTranslation(-CameraPosition);
            ModelviewMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, -4.0f);

            //Matrix4 MVP_Matrix = ModelviewMatrix * WorldMatrix * ProjectionMatrix;
            Matrix4 MV_Matrix = ModelviewMatrix * WorldMatrix;


            GL.UseProgram(Shader.Program);

            int mv_matrix_location = GL.GetUniformLocation(Shader.Program, "mv_matrix");
            GL.UniformMatrix4(mv_matrix_location, false, ref MV_Matrix);
            int p_matrix_location = GL.GetUniformLocation(Shader.Program, "p_matrix");
            GL.UniformMatrix4(p_matrix_location, false, ref ProjectionMatrix);

            GL.UseProgram(0);
        }

        private void OnResize(object sender, EventArgs e)
		{
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 0.1f, 1000.0f);
			OnRenderFrame(this, null);
		}

		private void OnRenderFrame(object sender, FrameEventArgs e)
		{
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(Shader.Program);

            //Render meshes here
            Sphere.Render(Shader);
            
            GL.UseProgram(0);
            SwapBuffers();
		}
	}
}
