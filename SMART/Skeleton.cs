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
using SMART.Engine;

namespace SMART
{
	class Skeleton : SceneObject
	{
		private Material skeletonMaterial;

		List<Muscle> Muscles = new List<Muscle>();

		public Skeleton(string name, Transform transform, Material material, string fileName)
			: base(name, transform)
		{
			skeletonMaterial = material;
			LoadSkeleton(fileName);
		}

		enum SkeletonParserState { StartState, CreationState, LinkState, FreedomState };
		private void LoadSkeleton(string fileName)
		{
			SkeletonParserState state = SkeletonParserState.StartState;
			string line;
			int lineNumber = 0;
			int rootBoneCounter = 0;
			Dictionary<string, Bone> allBones = new Dictionary<string, Bone>();
			//ObjMesh boneMesh = new ObjMesh(.30f, 16);
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
						if (segments[0].Equals("Bone"))
						{
							string boneName = segments[1];
							float x = float.Parse(segments[2], culture);
							float y = float.Parse(segments[3], culture);
							float z = float.Parse(segments[4], culture);
							Bone bone = new Bone(boneName, new Transform(new Vector3(x, y, z), Vector3.Zero, Vector3.One), skeletonMaterial);

							allBones.Add(boneName, bone);

							if (boneName.Equals("Root"))
							{
								this.Add(bone);
								rootBoneCounter++;
							}

							//Give the bone a random color
							//Color color = Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
							//bone.SetColor(color);

							//allBones.Add(nodeName, bone);
						}
						else if (segments[0].Equals("LinkState"))
						{
							if (rootBoneCounter == 1)
							{
								state = SkeletonParserState.LinkState;
							}
							else
							{
								throw new Exception("Wrong amout of bones with the name Root. We found " + rootBoneCounter + " bones with the name Root.");
							}
						}
						else
						{
							throw new Exception("Error on line " + lineNumber + ". The words Node or LinkState expected.");
						}
					}
					else if (state == SkeletonParserState.LinkState)
					{
						string[] segments = line.Split(separators);
						if (segments[0].Equals("Bone") && segments[2].Equals("Children"))
						{
							string boneName = segments[1];
							for (int i = 3; i < segments.Length; i++)
							{
								//Set 
								allBones[boneName].Add(allBones[segments[i]]);
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
		}
	}
}
