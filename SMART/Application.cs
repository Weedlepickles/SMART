using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using SMART.Engine;

namespace SMART
{
	public class Application : GameWindow
	{
		private Skeleton ActiveSkeleton;

		private Matrix4 ProjectionMatrix;
		private Matrix4 WorldMatrix;

		private Vector3 CameraPosition;
		private Shader Shader;
		private long PrevTime;

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

			CameraPosition = new Vector3(0, 0, 0);

			string vertex_source = System.IO.File.ReadAllText("Shaders/Basic.vertex");
			string fragment_source = System.IO.File.ReadAllText("Shaders/Basic.fragment");

			Shader = new Shader(ref vertex_source, ref fragment_source);

			PrevTime = DateTime.Now.Millisecond;

			//Laddar en obj mesh
			//pants = Skeleton.CreatePants();
			//pants.SetPosition(new Vector3(0, 0, -10));

			//ActiveSkeleton = new Skeleton("ToePants.skeleton");
			//ActiveSkeleton.SetPosition(new Vector3(0, 2, -10));

			Material standardMaterial = new Material("Basic.vertex", "Basic.fragment");

			SceneObject sphere = new SceneObject("Sphere", new Transform(new Vector3(0, 0, -10), Vector3.Zero, new Vector3(1, 1, 1)));
			SceneObject cylinder = new SceneObject("Cylinder", new Transform(new Vector3(2, 2, -10), new Vector3(1, 0, 0), new Vector3(1, 1, 1)));

			Mesh sphereMesh = new Mesh(new ObjMesh(.30f, 16));
			Mesh cylinderMesh = new Mesh(new ObjMesh(.30f, 1, 16));

			MeshRenderer meshRenderer1 = new MeshRenderer(standardMaterial);
			MeshRenderer meshRenderer2 = new MeshRenderer(standardMaterial);

			sphere.Add(sphereMesh);
			sphere.Add(meshRenderer1);

			cylinder.Add(cylinderMesh);
			cylinder.Add(meshRenderer2);

			//ActiveScene.Add(sphere);
			//ActiveScene.Add(cylinder);


			Transform skeletonTransform = new Transform(new Vector3(0, 2, -10), Vector3.Zero, new Vector3(1, 1, 1));
			Skeleton skeleton = new Skeleton("Ben", skeletonTransform, standardMaterial, "ToePants.skeleton");

			ActiveScene.Add(skeleton);

			ActiveScene.Load();

			//Laddar en sfär med radie 4, upplösning 32
			//Sphere = new ObjMesh(4.0f, 32);

			//Prepare meshes here

		}

		private void OnUpdateFrame(object sender, FrameEventArgs e)
		{
			WorldMatrix = Matrix4.CreateTranslation(-CameraPosition);


			//Matrix4 MVP_Matrix = ModelviewMatrix * WorldMatrix * ProjectionMatrix;

			GL.UseProgram(Shader.Program);

			int p_matrix_location = GL.GetUniformLocation(Shader.Program, "p_matrix");
			GL.UniformMatrix4(p_matrix_location, false, ref ProjectionMatrix);
			int mv_matrix_location = GL.GetUniformLocation(Shader.Program, "v_matrix");
			GL.UniformMatrix4(mv_matrix_location, false, ref WorldMatrix);

			GL.UseProgram(0);
			//long currentTime = PrevTime = DateTime.Now.Millisecond;
			//ActiveSkeleton.Update(currentTime - PrevTime);
			//PrevTime = currentTime;

			UpdateState(TimeSpan.Zero);
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

		//float rot = 0;
		private void OnRenderFrame(object sender, FrameEventArgs e)
		{
			//ModelviewMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, -20.0f);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(Shader.Program);

			//Render meshes here
			//pants.SetRotation(new Vector3(0, rot * (float)Math.PI * 2, 0));
			//pants.GetRootBone().SetRotation(new Vector3(0, 0, rot * (float)Math.PI));
			//pants.GetRootBone().GetChildren()[0].SetRotation(new Vector3(rot * (float)Math.PI * 4, 0, 0));
			//rot += 0.01f;
			//if (rot > 2)
			//{
			//	rot = 0;
			//}
			//pants.Render(Shader);

			//ActiveSkeleton.Render(Shader);
			ActiveScene.Render();

			GL.UseProgram(0);
			SwapBuffers();
		}
	}
}
