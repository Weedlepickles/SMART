using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Camera
	{
		private Vector3 position;
		private Vector3 rotation;
		private Matrix4 projectionMatrix;
 
		public Camera(Vector3 position, Vector3 rotation, float aspectRatio)
		{
			projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000.0f);
			this.position = position;
			this.rotation = (float)Math.PI * (rotation / 180);
		}

		public Vector3 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return (rotation * 180) / (float)Math.PI;
			}
			set
			{
				rotation = ((float)Math.PI * value) / 180;
			}
		}

		public Matrix4 ViewMatrix
		{
			get
			{
				Matrix4 cameraTransformationMatrix = Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(position).Inverted();
				return cameraTransformationMatrix * projectionMatrix;
			}
			private set
			{

			}
		}
	}
}
