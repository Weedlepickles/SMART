using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using OpenTK.Input;
using System.IO;
using System.Windows.Forms;

namespace SMART
{
	public class Application : GameWindow
	{
        
        private DateTime lastUpdate;
		private KeyboardState previousKeyBoardState;
		string[] skeletonFileNames;
		int skeletonNumber = 0;

		private Scene ActiveScene;


		public Application()
			: base(800, 600)
		{
			this.RenderFrame += new EventHandler<FrameEventArgs>(OnRenderFrame);
			this.Resize += new EventHandler<EventArgs>(OnResize);
			this.Load += new EventHandler<EventArgs>(OnLoad);
			this.UpdateFrame += new EventHandler<FrameEventArgs>(OnUpdateFrame);
			previousKeyBoardState = new KeyboardState();
			ActiveScene = new Scene(Width, Height);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			GL.ClearColor(0.65f, 0.8f, 1f, 1.0f);

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			skeletonFileNames = Directory.GetFiles("Skeletons");

            ActiveScene.LoadCowSkeleton(skeletonFileNames.First(n => n.Contains("Cow")));

			lastUpdate = DateTime.Now;
		}

		private void OnUpdateFrame(object sender, FrameEventArgs e)
		{
			DateTime now = DateTime.Now;
			TimeSpan deltaTime = now - lastUpdate;
			lastUpdate = now;

			KeyHandler(deltaTime);

			deltaTime = new TimeSpan(0, 0, 0, 0, 5);

			UpdateState(deltaTime);
		}

		private void KeyHandler(TimeSpan deltaTime)
		{
			KeyboardState keyBoardState = OpenTK.Input.Keyboard.GetState();

			if (keyBoardState[Key.Enter] && !previousKeyBoardState[Key.Enter])
			{
				//Load new skeleton!
				/*skeletonNumber = (skeletonNumber + 1) % skeletonFileNames.Length;
				Console.WriteLine("Loaded " + skeletonFileNames[skeletonNumber]);
				ActiveScene.Load(skeletonFileNames[skeletonNumber]);*/
                ActiveScene.Reset();
			}
			if (keyBoardState[Key.W])
			{
				ActiveScene.Camera.Position = ActiveScene.Camera.Position - Vector3.UnitZ * 0.015f * (float)deltaTime.TotalMilliseconds;
			}
			if (keyBoardState[Key.S])
			{
				ActiveScene.Camera.Position = ActiveScene.Camera.Position + Vector3.UnitZ * 0.015f * (float)deltaTime.TotalMilliseconds;
			}
			if (keyBoardState[Key.A])
			{
				ActiveScene.Camera.Position = ActiveScene.Camera.Position - Vector3.UnitX * 0.015f * (float)deltaTime.TotalMilliseconds;
			}
			if (keyBoardState[Key.D])
			{
				ActiveScene.Camera.Position = ActiveScene.Camera.Position + Vector3.UnitX * 0.015f * (float)deltaTime.TotalMilliseconds;
			}

            if (keyBoardState[Key.Q])
            {
                ActiveScene.Camera.Rotation = ActiveScene.Camera.Rotation + Vector3.UnitY * 0.1f * (float)deltaTime.TotalMilliseconds;
            }
            if (keyBoardState[Key.E])
            {
                ActiveScene.Camera.Rotation = ActiveScene.Camera.Rotation - Vector3.UnitY * 0.1f * (float)deltaTime.TotalMilliseconds;
            }

            if (ActiveScene.cow != null && keyBoardState[Key.Space] && !previousKeyBoardState[Key.Space])
			{
                if (MessageBox.Show("Save engine state?", "Save?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ActiveScene.cow.SaveState();
                    MessageBox.Show("State saved", "info", MessageBoxButtons.OK);
                }
                
			}

            if (keyBoardState[Key.L] && !previousKeyBoardState[Key.L])
            {
                if (ActiveScene.cow != null && MessageBox.Show("Load engine state?", "Load?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ActiveScene.cow.LoadState();
                    MessageBox.Show("State loaded", "info", MessageBoxButtons.OK);
                }

            }

			previousKeyBoardState = keyBoardState;
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
