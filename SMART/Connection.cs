using Jitter.LinearMath;
using OpenTK;
using SMART.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Connection
	{
		private Bone first, second;
		private float maxLength, minLength;
		private Renderer renderer;
		private Skeleton owner;

		public Connection(Skeleton owner, Bone first, Bone second, Vector4 color)
		{
			this.first = first;
			this.second = second;
			this.owner = owner;
			maxLength = GetConnectionVector().Length();
			minLength = maxLength;
			renderer = new Renderer(new ObjMesh(0.2f, 1, 10), color);
		}

		public Bone First
		{
			get
			{
				return first;
			}
			set
			{
				first = value;
			}
		}
		public Bone Second
		{
			get
			{
				return second;
			}
			set
			{
				second = value;
			}
		}
		public float MaxLength
		{
			get
			{
				return maxLength;
			}
			set
			{
				maxLength = value;
			}
		}
		public float MinLength
		{
			get
			{
				return minLength;
			}
			set
			{
				minLength = value;
			}
		}
		public JVector GetConnectionVector()
		{
			return first.RigidBody.Position - second.RigidBody.Position;
		}

		private DateTime time = DateTime.Now;
		public void Render(Camera camera)
		{
			float degree90 = (float)(Math.PI / 2);
			TimeSpan timeSpan = DateTime.Now - time;

			JVector connectionJVector = GetConnectionVector();
			Vector3 connectionVector3 = new Vector3(connectionJVector.X, connectionJVector.Y, connectionJVector.Z);

			Vector3 position = new Vector3(first.RigidBody.Position.X - 0.5f * connectionJVector.X, first.RigidBody.Position.Y - 0.5f * connectionJVector.Y, first.RigidBody.Position.Z - 0.5f * connectionJVector.Z);

			Matrix4 modelTranslation = Matrix4.CreateTranslation(position);
			Matrix4 scale = Matrix4.CreateScale(1, 1, connectionVector3.Length);

			float angleAroundY = Vector3.CalculateAngle(Vector3.UnitZ, new Vector3(connectionVector3.X, 0, connectionVector3.Z));

			Vector3 rotationVector = Vector3.Cross(connectionVector3, Vector3.UnitY);
			float YToVector = Vector3.CalculateAngle(Vector3.UnitY, connectionVector3);

			//Debug.WriteLine(second.Name + " to " + first.Name + " Angle around Y: " + Math.Round(angleAroundY * (180 / Math.PI)) + ", " + Math.Round(YToVector * (180 / Math.PI)) + ", " + (float)((Math.PI / 2) - YToVector) + ", " + connectionVector3);

			Matrix4 rotation = Matrix4.Identity;
			if (!float.IsNaN(angleAroundY))
			{
				if (connectionVector3.X >= 0 && connectionVector3.Z >= 0)
				{
					if (angleAroundY < degree90)
						rotation = Matrix4.CreateRotationY(angleAroundY);
					else
						rotation = Matrix4.CreateRotationY(-angleAroundY);
				}
				else if (connectionVector3.X >= 0 && connectionVector3.Z < 0)
				{
					if (angleAroundY > degree90)
						rotation = Matrix4.CreateRotationY(angleAroundY);
					else
						rotation = Matrix4.CreateRotationY(-angleAroundY);
				}
				else if (connectionVector3.X < 0 && connectionVector3.Z >= 0)
				{
					if (angleAroundY > degree90)
						rotation = Matrix4.CreateRotationY(angleAroundY);
					else
						rotation = Matrix4.CreateRotationY(-angleAroundY);
				}
				else
				{
					if (angleAroundY < degree90)
						rotation = Matrix4.CreateRotationY(degree90 - angleAroundY);
					else
						rotation = Matrix4.CreateRotationY(-angleAroundY);
				}
			}
			if (rotationVector != Vector3.Zero)
			{
				if (rotationVector.Y < 0)
					rotation = rotation * Matrix4.CreateFromAxisAngle(rotationVector, -YToVector);
				else
					rotation = rotation * Matrix4.CreateFromAxisAngle(rotationVector, (float)(Math.PI / 2) - YToVector);
			}
			else
				rotation = rotation * Matrix4.CreateRotationX((float)(Math.PI / 2) - YToVector);


			Matrix4 transformation = scale * rotation * modelTranslation;

			renderer.Render(camera, transformation);
		}
	}
}
