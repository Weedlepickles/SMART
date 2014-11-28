using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	class Transform
	{
		public Transform()
		{
			Position = Vector3.Zero;
			Rotation = Vector3.Zero;
			Scale = Vector3.One;
			UpdateNumber = 1;
		}

		public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
			UpdateNumber = 1;
		}

		private Vector3 position;
		private Vector3 rotation;
		private Vector3 scale;
		private Transform parent;
		private bool isModified = true;
		private int updateNumber = 0; //Zero reserved
		private int parentUpdateNumber = 0;
		private Matrix4 transformationMatrix;

		public int UpdateNumber
		{
			get
			{
				return updateNumber;
			}
			private set
			{
				updateNumber = value;
			}
		}

		private int ParentUpdateNumber
		{
			get
			{
				return parentUpdateNumber;
			}
			set
			{
				parentUpdateNumber = value;
			}
		}

		public Transform Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
				parentUpdateNumber = 0;
				if (value != null)
					parentUpdateNumber = parent.UpdateNumber;
				isModified = true;
			}
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
				updateNumber++;
				isModified = true;
			}
		}
		public Vector3 Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
				updateNumber++;
				isModified = true;
			}
		}
		public Vector3 Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
				updateNumber++;
				isModified = true;
			}
		}

		public Matrix4 TransformationMatrix
		{
			get
			{
				if (isModified || (parent != null && ParentUpdateNumber != parent.UpdateNumber))
				{
					transformationMatrix = CalculateTransformationMatrix();
					if(parent != null)
						ParentUpdateNumber = parent.UpdateNumber;
					updateNumber++;
					isModified = false;
				}
				return transformationMatrix;
			}
			private set
			{
				transformationMatrix = value;
			}
		}

		private Matrix4 CalculateTransformationMatrix()
		{
			Matrix4 output = Matrix4.CreateScale(scale) * Matrix4.CreateRotationZ(rotation[2]) * Matrix4.CreateRotationY(rotation[1]) * Matrix4.CreateRotationX(rotation[0]) * Matrix4.CreateTranslation(position);
			if (parent != null)
			{
				output = output * parent.TransformationMatrix;
			}
			return output;
		}
	}
}
