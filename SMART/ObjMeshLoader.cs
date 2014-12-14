using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	public class ObjMeshLoader
	{
		public static bool LoadObj(ObjMesh mesh, string fileName)
		{
			try
			{
				using (StreamReader streamReader = new StreamReader(fileName))
				{
					Load(mesh, streamReader);
					streamReader.Close();
					return true;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				return false;
			}
		}

		public static bool LoadSphere(ObjMesh mesh, float radius, int resolution)
		{
			if (mesh == null || resolution < 3) return false;
			int rings = (byte)Math.Ceiling(resolution / 2.0);
			if (rings % 2 == 0) rings++;
			mesh.Vertices = CalculateSphereVertices(radius, radius, (byte)resolution, (byte)rings);
			return true;
		}

		public static bool LoadCylinder(ObjMesh mesh, float radius, float length, int resolution)
		{
			if (mesh == null || resolution < 3) return false;
			mesh.Vertices = CalculateCylinderVertices(radius, length, resolution);
			return true;
		}

		public static bool LoadSqare(ObjMesh mesh, float length, float height)
		{
			if (mesh == null)
				return false;
			mesh.Vertices = CalculateSquareVertices(length, height);
			return true;
		}

		private static ObjMesh.ObjVertex[] CalculateSquareVertices(float length, float height)
		{
			ObjMesh.ObjVertex[] vertices = new ObjMesh.ObjVertex[6];

			vertices[0] = new ObjMesh.ObjVertex();
			vertices[1] = new ObjMesh.ObjVertex();
			vertices[2] = new ObjMesh.ObjVertex();
			vertices[3] = new ObjMesh.ObjVertex();

			vertices[0].Vertex = Vector3.Zero;
			vertices[0].Normal = Vector3.UnitY;
			vertices[0].TexCoord = Vector2.Zero;

			vertices[1].Vertex = new Vector3(0, 0, length);
			vertices[1].Normal = Vector3.UnitY;
			vertices[1].TexCoord = Vector2.Zero;

			vertices[2].Vertex = new Vector3(height, 0, length);
			vertices[2].Normal = Vector3.UnitY;
			vertices[2].TexCoord = Vector2.Zero;

			vertices[3] = vertices[0];

			vertices[4] = vertices[2];

			vertices[5].Vertex = new Vector3(height, 0, 0);
			vertices[5].Normal = Vector3.UnitY;
			vertices[5].TexCoord = Vector2.Zero;

			return vertices;
		}

		private static ObjMesh.ObjVertex[] CalculateCylinderVertices(float radius, float length, int segments)
		{
			List<List<ObjMesh.ObjVertex>> vertices = new List<List<ObjMesh.ObjVertex>>();

			ObjMesh.ObjVertex[] baseCircleVertices = new ObjMesh.ObjVertex[segments];
			ObjMesh.ObjVertex[] topCircleVertices = new ObjMesh.ObjVertex[segments];

			//Center vertex for the base side
			ObjMesh.ObjVertex baseVertex = new ObjMesh.ObjVertex();
			baseVertex.Vertex = new Vector3(0, 0, length / 2);
			baseVertex.Normal = new Vector3(0, 0, 1);
			baseVertex.TexCoord = new Vector2(1); //Don't know what this should be

			//Center vertex for the top side
			ObjMesh.ObjVertex topVertex = new ObjMesh.ObjVertex();
			topVertex.Vertex = new Vector3(0, 0, -length / 2);
			topVertex.Normal = new Vector3(0, 0, -1);
			topVertex.TexCoord = new Vector2(1); //Don't know what this should be

			double angleIncrement = 2 * Math.PI / segments;

			double x = 0;
			double y = 0;
			double z = length / 2;
			for (int i = 0; i < segments; i++)
			{
				ObjMesh.ObjVertex baseCircleVertex = new ObjMesh.ObjVertex();
				ObjMesh.ObjVertex topCircleVertex = new ObjMesh.ObjVertex();

				x = radius * Math.Cos(i * angleIncrement);
				y = radius * Math.Sin(i * angleIncrement);

				baseCircleVertex.Vertex = new Vector3((float)x, (float)y, (float)z);
				topCircleVertex.Vertex = new Vector3((float)x, (float)y, (float)-z); //Facing the other way (Z)

				baseCircleVertex.Normal = Vector3.Normalize(new Vector3(baseCircleVertex.Vertex.X, baseCircleVertex.Vertex.Y, 0));
				topCircleVertex.Normal = Vector3.Normalize(new Vector3(topCircleVertex.Vertex.X, topCircleVertex.Vertex.Y, 0));

				baseCircleVertex.TexCoord = new Vector2(1);
				topCircleVertex.TexCoord = new Vector2(1);

				baseCircleVertices[i] = baseCircleVertex;
				topCircleVertices[i] = topCircleVertex;
			}

			List<ObjMesh.ObjVertex> output = new List<ObjMesh.ObjVertex>();

			//Base
			for (int i = 0; i < segments; i++)
			{
				output.Add(baseVertex);
				output.Add(baseCircleVertices[i]);
				output.Add(baseCircleVertices[(i + 1) % segments]); //This should be 0 the last round
			}
			//Top
			for (int i = 0; i < segments; i++)
			{
				output.Add(topCircleVertices[i]);
				output.Add(topVertex);
				output.Add(topCircleVertices[(i + 1) % segments]); //This should be 0 the last round
			}
			//Side
			for (int i = 0; i < segments; i++)
			{
				output.Add(baseCircleVertices[i]);
				output.Add(topCircleVertices[i]);
				output.Add(topCircleVertices[(i + 1) % segments]);

				output.Add(baseCircleVertices[(i + 1) % segments]);
				output.Add(baseCircleVertices[i]);
				output.Add(topCircleVertices[(i + 1) % segments]);
			}

			return output.ToArray();

			//for (double y = 0; y < rings; y++)
			//{
			//	List<ObjMesh.ObjVertex> ring = new List<ObjMesh.ObjVertex>();
			//	double phi = (y / (rings - 1)) * Math.PI; //was /2 
			//	for (double x = 0; x < segments; x++)
			//	{
			//		double theta = (x / (segments - 1)) * 2 * Math.PI;

			//		Vector3 v = new Vector3()
			//		{
			//			X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
			//			Y = (float)(height * Math.Cos(phi)),
			//			Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
			//		};
			//		Vector3 n = Vector3.Normalize(v);
			//		Vector2 uv = new Vector2()
			//		{
			//			X = (float)(x / (segments - 1)),
			//			Y = (float)(y / (rings - 1))
			//		};
			//		// Using data[i++] causes i to be incremented multiple times in Mono 2.2 (bug #479506).
			//		ring.Add(new ObjMesh.ObjVertex() { Vertex = v, Normal = n, TexCoord = uv });
			//	}
			//	vertices.Add(ring);
			//}

			//List<ObjMesh.ObjVertex> data = new List<ObjMesh.ObjVertex>();
			//for (int ringId = 1; ringId < vertices.Count; ringId++)
			//{
			//	for (int segId = 0; segId < vertices[ringId].Count - 1; segId++)
			//	{
			//		bool isTop = (ringId == 1);
			//		bool isBottom = (ringId == vertices.Count - 1);

			//		if (!isBottom)
			//		{
			//			data.Add(vertices[ringId][segId]);
			//			data.Add(vertices[ringId - 1][segId]);
			//			data.Add(vertices[ringId][segId + 1]);
			//		}

			//		if (!isTop)
			//		{
			//			data.Add(vertices[ringId - 1][segId]);
			//			data.Add(vertices[ringId - 1][segId + 1]);
			//			data.Add(vertices[ringId][segId + 1]);
			//		}
			//	}
			//}

			//return data.ToArray(); ;
		}

		#region Private

		private static char[] splitCharacters = new char[] { ' ' };
		private static char[] faceParamaterSplitter = new char[] { '/' };

		private static List<Vector3> vertices;
		private static List<Vector3> normals;
		private static List<Vector2> texCoords;
		private static Dictionary<ObjMesh.ObjVertex, int> objVerticesIndexDictionary;
		private static List<ObjMesh.ObjVertex> objVertices;
		private static List<ObjMesh.ObjTriangle> objTriangles;
		private static List<ObjMesh.ObjQuad> objQuads;

		private static void Load(ObjMesh mesh, TextReader textReader)
		{
			vertices = new List<Vector3>();
			normals = new List<Vector3>();
			texCoords = new List<Vector2>();
			objVerticesIndexDictionary = new Dictionary<ObjMesh.ObjVertex, int>();
			objVertices = new List<ObjMesh.ObjVertex>();
			objTriangles = new List<ObjMesh.ObjTriangle>();
			objQuads = new List<ObjMesh.ObjQuad>();

			string line;
			while ((line = textReader.ReadLine()) != null)
			{
				line = line.Trim(splitCharacters);
				line = line.Replace("  ", " ");

				string[] parameters = line.Split(splitCharacters);
				CultureInfo enUSCulture = CultureInfo.CreateSpecificCulture("en-US");
				switch (parameters[0])
				{
					case "p": // Point
						break;

					case "v": // Vertex
						float x = float.Parse(parameters[1], enUSCulture);
						float y = float.Parse(parameters[2], enUSCulture);
						float z = float.Parse(parameters[3], enUSCulture);
						vertices.Add(new Vector3(x, y, z));
						break;

					case "vt": // TexCoord
						float u = float.Parse(parameters[1], enUSCulture);
						float v = float.Parse(parameters[2], enUSCulture);
						texCoords.Add(new Vector2(u, v));
						break;

					case "vn": // Normal
						float nx = float.Parse(parameters[1], enUSCulture);
						float ny = float.Parse(parameters[2], enUSCulture);
						float nz = float.Parse(parameters[3], enUSCulture);
						normals.Add(new Vector3(nx, ny, nz));
						break;

					case "f":
						switch (parameters.Length)
						{
							case 4:
								ObjMesh.ObjTriangle objTriangle = new ObjMesh.ObjTriangle();
								objTriangle.Index0 = ParseFaceParameter(parameters[1]);
								objTriangle.Index1 = ParseFaceParameter(parameters[2]);
								objTriangle.Index2 = ParseFaceParameter(parameters[3]);
								objTriangles.Add(objTriangle);
								break;

							case 5:
								ObjMesh.ObjQuad objQuad = new ObjMesh.ObjQuad();
								objQuad.Index0 = ParseFaceParameter(parameters[1]);
								objQuad.Index1 = ParseFaceParameter(parameters[2]);
								objQuad.Index2 = ParseFaceParameter(parameters[3]);
								objQuad.Index3 = ParseFaceParameter(parameters[4]);
								objQuads.Add(objQuad);
								break;
						}
						break;
				}
			}

			mesh.Vertices = objVertices.ToArray();
			mesh.Triangles = objTriangles.ToArray();
			mesh.Quads = objQuads.ToArray();

			objVerticesIndexDictionary = null;
			vertices = null;
			normals = null;
			texCoords = null;
			objVertices = null;
			objTriangles = null;
			objQuads = null;
		}

		private static int ParseFaceParameter(string faceParameter)
		{
			Vector3 vertex = new Vector3();
			Vector2 texCoord = new Vector2();
			Vector3 normal = new Vector3();

			string[] parameters = faceParameter.Split(faceParamaterSplitter);

			int vertexIndex = int.Parse(parameters[0]);
			if (vertexIndex < 0) vertexIndex = vertices.Count + vertexIndex;
			else vertexIndex = vertexIndex - 1;
			vertex = vertices[vertexIndex];

			if (parameters.Length > 1)
			{
				int texCoordIndex = int.Parse(parameters[1]);
				if (texCoordIndex < 0) texCoordIndex = texCoords.Count + texCoordIndex;
				else texCoordIndex = texCoordIndex - 1;
				texCoord = texCoords[texCoordIndex];
			}

			if (parameters.Length > 2)
			{
				int normalIndex = int.Parse(parameters[2]);
				if (normalIndex < 0) normalIndex = normals.Count + normalIndex;
				else normalIndex = normalIndex - 1;
				normal = normals[normalIndex];
			}

			return FindOrAddObjVertex(ref vertex, ref texCoord, ref normal);
		}

		private static int FindOrAddObjVertex(ref Vector3 vertex, ref Vector2 texCoord, ref Vector3 normal)
		{
			ObjMesh.ObjVertex newObjVertex = new ObjMesh.ObjVertex();
			newObjVertex.Vertex = vertex;
			newObjVertex.TexCoord = texCoord;
			newObjVertex.Normal = normal;

			int index;
			if (objVerticesIndexDictionary.TryGetValue(newObjVertex, out index))
			{
				return index;
			}
			else
			{
				objVertices.Add(newObjVertex);
				objVerticesIndexDictionary[newObjVertex] = objVertices.Count - 1;
				return objVertices.Count - 1;
			}
		}

		private static ObjMesh.ObjVertex[] CalculateSphereVertices(float radius, float height, byte segments, byte rings)
		{
			List<List<ObjMesh.ObjVertex>> vertices = new List<List<ObjMesh.ObjVertex>>();

			for (double y = 0; y < rings; y++)
			{
				List<ObjMesh.ObjVertex> ring = new List<ObjMesh.ObjVertex>();
				double phi = (y / (rings - 1)) * Math.PI; //was /2 
				for (double x = 0; x < segments; x++)
				{
					double theta = (x / (segments - 1)) * 2 * Math.PI;

					Vector3 v = new Vector3()
					{
						X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
						Y = (float)(height * Math.Cos(phi)),
						Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
					};
					Vector3 n = Vector3.Normalize(v);
					Vector2 uv = new Vector2()
					{
						X = (float)(x / (segments - 1)),
						Y = (float)(y / (rings - 1))
					};
					// Using data[i++] causes i to be incremented multiple times in Mono 2.2 (bug #479506).
					ring.Add(new ObjMesh.ObjVertex() { Vertex = v, Normal = n, TexCoord = uv });
				}
				vertices.Add(ring);
			}

			List<ObjMesh.ObjVertex> data = new List<ObjMesh.ObjVertex>();
			for (int ringId = 1; ringId < vertices.Count; ringId++)
			{
				for (int segId = 0; segId < vertices[ringId].Count - 1; segId++)
				{
					bool isTop = (ringId == 1);
					bool isBottom = (ringId == vertices.Count - 1);

					if (!isBottom)
					{
						data.Add(vertices[ringId][segId]);
						data.Add(vertices[ringId - 1][segId]);
						data.Add(vertices[ringId][segId + 1]);
					}

					if (!isTop)
					{
						data.Add(vertices[ringId - 1][segId]);
						data.Add(vertices[ringId - 1][segId + 1]);
						data.Add(vertices[ringId][segId + 1]);
					}
				}
			}

			return data.ToArray(); ;
		}

		#endregion

	}
}
