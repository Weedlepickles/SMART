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
	class Application : GameWindow
	{
		public Application() : base (400, 400)
		{
			this.RenderFrame += new EventHandler<FrameEventArgs>(OnRenderFrame);
			this.Resize += new EventHandler<EventArgs>(OnResize);
			this.Load += new EventHandler<EventArgs>(OnLoad);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			this.VSync = VSyncMode.On;
		}

		private void OnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, this.Width, this.Height);

			OnRenderFrame(null, null);
		}

		private void OnRenderFrame(object sender, EventArgs e)
		{
			// render graphics
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.ClearColor(Color4.Beige);
			this.SwapBuffers();
		}
	}
}
