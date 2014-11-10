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
		private int mShaderProgram;
		private Matrix4 mViewMatrix, mProjectionMatrix, mModelMatrix;
		private Vector3 objectLocation = Vector3.UnitX;
		private ObjMesh sphere;
		ObjMesh myTriangle;

		public Application()
			: base(400, 400)
		{
			this.RenderFrame += new EventHandler<FrameEventArgs>(OnRenderFrame);
			this.Resize += new EventHandler<EventArgs>(OnResize);
			this.Load += new EventHandler<EventArgs>(OnLoad);

			new Scene();
		}

		private void OnLoad(object sender, EventArgs e)
		{

			myTriangle = ObjMesh.CreateTriangle();

			this.VSync = VSyncMode.On;

			string vertexShaderFileName = "Shaders/Basic.vertex";
			string fragmentShaderFileName = "Shaders/Basic.fragment";

			int vertexShader = GLUtilities.LoadShader(vertexShaderFileName, ShaderType.VertexShader);
			int fragmentShader = GLUtilities.LoadShader(fragmentShaderFileName, ShaderType.FragmentShader);

			// Set clearcolor.
			GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			// create a shader object.
			mShaderProgram = GLUtilities.CreateProgram(vertexShader, fragmentShader);
			GL.UseProgram(mShaderProgram);

			mProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 2), 1.0f, 0.1f, 1000f);
			mViewMatrix = Matrix4.LookAt(Vector3.Zero, objectLocation, Vector3.UnitY);

			mModelMatrix = Matrix4.Identity;
			mModelMatrix.Column3 = new Vector4(0, 0, 10, 1); //Place the oject at (0,0,10)

			sphere = new ObjMesh("Models/sphere.obj");
			//ALL DONE!
		}

		private void OnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, this.Width, this.Height);

			OnRenderFrame(null, null);
		}

		private void OnRenderFrame(object sender, EventArgs e)
		{

			Matrix4 modelView = mViewMatrix * mModelMatrix;

			// render graphics
			GL.ClearColor(new Color4(123, 123, 13, 255));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(mShaderProgram);
			GL.UniformMatrix4(GL.GetUniformLocation(mShaderProgram, "projection"), true, ref mProjectionMatrix);
			GL.UniformMatrix4(GL.GetUniformLocation(mShaderProgram, "view"), true, ref modelView);

			//Draw object here
			//GL.UniformMatrix4(GL.GetUniformLocation(mShaderProgram, "model"), true, ref mModelMatrix);
			//sphere.Render(mShaderProgram, "in_Position", "in_Normal", null);
			myTriangle.Render2(mShaderProgram, "in_Position", "in_Normal", null);

			SwapBuffers();
		}
	}
}
