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
		private DateTime lastUpdate;

		private Scene ActiveScene;


		public Application()
			: base(500, 400)
		{
			this.RenderFrame += new EventHandler<FrameEventArgs>(OnRenderFrame);
			this.Resize += new EventHandler<EventArgs>(OnResize);
			this.Load += new EventHandler<EventArgs>(OnLoad);
			this.UpdateFrame += new EventHandler<FrameEventArgs>(OnUpdateFrame);
			ActiveScene = new Scene(Width, Height);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			GL.ClearColor(0.10f, 0.795f, 0.10f, 1.0f);

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			ActiveScene.Load();

			lastUpdate = DateTime.Now;
		}

		private void OnUpdateFrame(object sender, FrameEventArgs e)
		{

			DateTime now = DateTime.Now;
			TimeSpan deltaTime = now - lastUpdate;
			lastUpdate = now;

			deltaTime = new TimeSpan(0, 0, 0, 0, 5);

			UpdateState(deltaTime);
		}

		private void UpdateState(TimeSpan deltaTime)
		{
			ActiveScene.Update(deltaTime);

			//UpdateAI(deltaTime);
		}


		private void OnResize(object sender, EventArgs e)
		{
			OnRenderFrame(this, null);
		}

		private void OnRenderFrame(object sender, FrameEventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			ActiveScene.Render();

			SwapBuffers();
		}
	}
}
