using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Skeleton
	{
		Bone RootBone;
		Vector3 Position = Vector3.Zero;
		Vector3 Rotation = Vector3.Zero;

		List<Muscle> Muscles = new List<Muscle>();

		public Skeleton(string fileName)
		{
			RootBone = LoadSkeleton(fileName);
		}

		enum SkeletonParserState { StartState, CreationState, LinkState, FreedomState };
		private Bone LoadSkeleton(string fileName)
		{
			SkeletonParserState state = SkeletonParserState.StartState;
			string line;
			int lineNumber = 0;
			Dictionary<string, Bone> allBones = new Dictionary<string, Bone>();
			ObjMesh boneMesh = new ObjMesh(.30f, 16);
			CultureInfo culture = new CultureInfo("en-US");
			char[] separators = { ',', ' ' };
			Random random = new Random();

			using (StreamReader reader = new StreamReader("Skeletons/" + fileName))
			{
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine();
					lineNumber++;
					if (line.Length == 0 || line[0] == 47 || line[0] == 32) //ignore if slash or space
					{
						//Do nothing, it's a comment or an empty line
					}
					else if (state == SkeletonParserState.StartState)
					{
						if (line.Equals("CreationState"))
						{
							state = SkeletonParserState.CreationState;
						}
						else
						{
							throw new Exception("Error on line " + lineNumber + ". The word CreationState expected.");
						}
					}
					else if (state == SkeletonParserState.CreationState)
					{
						string[] segments = line.Split(separators);
						if (segments[0].Equals("Node"))
						{
							string nodeName = segments[1];
							float x = float.Parse(segments[2], culture);
							float y = float.Parse(segments[3], culture);
							float z = float.Parse(segments[4], culture);
							Bone bone = new Bone(new Vector3(x, y, z), boneMesh);
							
							//Give the bone a random color
							Color color = Color.FromArgb(random.Next(0,256),random.Next(0,256),random.Next(0,256));
							bone.SetColor(color);
							
							allBones.Add(nodeName, bone);
						}
						else if (segments[0].Equals("LinkState"))
						{
							state = SkeletonParserState.LinkState;
						}
						else
						{
							throw new Exception("Error on line " + lineNumber + ". The words Node or LinkState expected.");
						}
					}
					else if (state == SkeletonParserState.LinkState)
					{
						string[] segments = line.Split(separators);
						if (segments[0].Equals("Node") && segments[2].Equals("Children"))
						{
							string nodeName = segments[1];
							for (int i = 3; i < segments.Length; i++)
							{
								allBones[nodeName].AddChildBone(allBones[segments[i]]);
							}
						}
						else if (segments[0].Equals("FreedomState"))
						{
							state = SkeletonParserState.FreedomState;
						}
						else
						{
							throw new Exception("Error on line " + lineNumber + ". The words Node or Children or FreedomState expected.");
						}
					}
					else if (state == SkeletonParserState.FreedomState)
					{
						//Not implemented (yet)
					}
				}
			}
			return allBones["Root"];
		}

		public static Skeleton CreatePants()
		{
			ObjMesh mesh = new ObjMesh(.45f, 8);
			//ObjMesh mesh = new ObjMesh(0.3f, 0.8f, 16);
			Bone rootBone = new Bone(new Vector3(0, 1, 0), mesh);
			rootBone.SetColor(Color.DarkGoldenrod);

			Bone bone2 = new Bone(Vector3.One, mesh);
			bone2.SetColor(Color.HotPink);

			Vector3 temp = new Vector3(0, 1, 0);
			Bone bone3 = new Bone(temp, mesh);
			bone3.SetColor(Color.LawnGreen);


			rootBone.AddChildBone(bone2);
			bone2.AddChildBone(bone3);

			Skeleton skeleton = new Skeleton(rootBone);
			return skeleton;
		}

		public Skeleton(Bone rootBone)
		{
			RootBone = rootBone;
		}

		public void SetPosition(Vector3 position)
		{
			Position = position;
		}

		public void SetRotation(Vector3 rotation)
		{
			Rotation = rotation;
		}

		public Bone GetRootBone()
		{
			return RootBone;
		}

		public void Render(Shader shader)
		{
			Matrix4 ModelMatrix = Matrix4.CreateRotationZ(Rotation[2]) * Matrix4.CreateRotationX(Rotation[0]) * Matrix4.CreateRotationY(Rotation[1]) * Matrix4.CreateTranslation(Position);

			RootBone.Render(shader, ModelMatrix);
		}

		public void Update(long dt)
		{
			RootBone.Update(dt);
		}
	}
}
