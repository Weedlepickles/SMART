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
		private Matrix4 cameraTransformationMatrix;
 
		public Camera(Vector3 position, Vector3 rotation, float aspectRatio)
		{
			projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000.0f);
			this.position = position;
			this.rotation = rotation;
			cameraTransformationMatrix = Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateTranslation(position).Inverted();
		}

		public Matrix4 ViewMatrix
		{
			get
			{
				return cameraTransformationMatrix * projectionMatrix;
			}
			private set
			{

			}
		}
	}
}
