using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Engine
{
	class SceneObject
	{
		public string Name;
		public Transform Transform;

		private List<SceneObject> contains;
		private List<Component> components;

		public void Add(SceneObject sceneObject)
		{
			sceneObject.Transform.Parent = this.Transform;
			contains.Add(sceneObject);
		}

		public void Add(Component component)
		{
			component.Owner = this;
			components.Add(component);
		}

		public List<Component> GetComponents(ComponentType type)
		{
			List<Component> output = new List<Component>();
			foreach (Component component in components)
			{
				if (component.Type == type)
					output.Add(component);
			}
			return output;
		}

		public SceneObject()
		{
			Name = "SceneObject";
			Transform = new Transform(Vector3.Zero, Vector3.Zero, Vector3.Zero);
			contains = new List<SceneObject>();
			components = new List<Component>();
		}
		public SceneObject(string name)
		{
			Name = name;
			Transform = new Transform(Vector3.Zero, Vector3.Zero, Vector3.Zero);
			contains = new List<SceneObject>();
			components = new List<Component>();
		}
		public SceneObject(string name, Transform transform)
		{
			Name = name;
			Transform = transform;
			contains = new List<SceneObject>();
			components = new List<Component>();
		}

		public void CallProcess(ProcessType processType, object data)
		{
			switch (processType)
			{
				case ProcessType.Render:
					Render();
					goto default;
				case ProcessType.UpdateState:
					//To be implemented
					goto default;
				default:
					foreach (SceneObject sceneObject in contains)
					{
						CallProcess(processType, data);
					}
					break;
			}
		}

		private void Render()
		{
			foreach (Component component in components)
			{
				if (component.Type == ComponentType.MeshRenderer)
				{
					((MeshRenderer)component).Render();
				}
			}
		}
	}
}
