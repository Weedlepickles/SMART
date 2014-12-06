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
		private Matrix4 ProjectionMatrix;
		private Matrix4 WorldMatrix;

		private Vector3 cameraPosition;
		private Shader Shader;

		private DateTime lastUpdate;

		private Scene ActiveScene;


		public Application()
			: base(400, 400)
		{
			this.RenderFrame += new EventHandler<FrameEventArgs>(OnRenderFrame);
			this.Resize += new EventHandler<EventArgs>(OnResize);
			this.Load += new EventHandler<EventArgs>(OnLoad);
			this.UpdateFrame += new EventHandler<FrameEventArgs>(OnUpdateFrame);
			ActiveScene = new Scene();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 0.1f, 1000.0f);
			WorldMatrix = new Matrix4();

			cameraPosition = new Vector3(0, 18, 0);

			string vertex_source = System.IO.File.ReadAllText("Shaders/Basic.vertex");
			string fragment_source = System.IO.File.ReadAllText("Shaders/Basic.fragment");

			Shader = new Shader(ref vertex_source, ref fragment_source);



			ActiveScene.Load();

			lastUpdate = DateTime.Now;
		}

		private void OnUpdateFrame(object sender, FrameEventArgs e)
		{
			WorldMatrix = Matrix4.CreateTranslation(-cameraPosition);

			GL.UseProgram(Shader.Program);

			int p_matrix_location = GL.GetUniformLocation(Shader.Program, "p_matrix");
			GL.UniformMatrix4(p_matrix_location, false, ref ProjectionMatrix);
			int mv_matrix_location = GL.GetUniformLocation(Shader.Program, "v_matrix");
			GL.UniformMatrix4(mv_matrix_location, false, ref WorldMatrix);

			GL.UseProgram(0);



			//long currentTime = PrevTime = DateTime.Now.Millisecond;
			//ActiveSkeleton.Update(currentTime - PrevTime);
			//PrevTime = currentTime;

			DateTime now = DateTime.Now;
			TimeSpan deltaTime = now - lastUpdate;
			lastUpdate = now;

			UpdateState(deltaTime);
		}

		private void UpdateState(TimeSpan deltaTime)
		{
			ActiveScene.Update(deltaTime);

			//UpdateAI(deltaTime);
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


			ActiveScene.Render();

			GL.UseProgram(0);
			SwapBuffers();
		}
	}
}
