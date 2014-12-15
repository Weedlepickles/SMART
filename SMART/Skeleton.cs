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
using Jitter;
using Jitter.Dynamics.Constraints;
using Jitter.LinearMath;
using Jitter.Dynamics;

namespace SMART
{
	class Skeleton
	{
		private Vector3 position;

		private List<Bone> bones = new List<Bone>();
		private List<Connection> connections = new List<Connection>();
		private List<LinearMuscle> muscles = new List<LinearMuscle>();

        private SMARTWorld world;

        public Skeleton(string name, Vector3 position, SMARTWorld world, string fileName)
		{
            FileName = fileName;
            this.position = position;
			this.world = world;
			LoadSkeleton(fileName);
		}

        #region Members

        public List<Bone> Bones
		{
			get
			{
				return bones;
			}
			private set
			{
				bones = value;
			}
		}
		public List<Connection> Connections
		{
			get
			{
				return connections;
			}
			private set
			{
				connections = value;
			}
		}
		public List<LinearMuscle> Muscles
		{
			get
			{
				return muscles;
			}
			private set
			{
				muscles = value;
			}
		}
		public Vector3 Position
		{
			get
			{
				return position;
			}
			private set
			{
				position = value;
			}
		}
        public string FileName;

		enum SkeletonParserState { StartState, CreationState, LinkState, MuscleState };

        #endregion

        public virtual void Update(TimeSpan deltaTime)
        {
            if (Bones != null)
            {
                foreach (LinearMuscle muscle in Muscles)
                {
                    muscle.UseMuscle();
                }

                Random random = new Random();
                foreach (Bone bone in Bones)
                {
                    bone.RigidBody.AddForce(world.Gravity);
                    if (world.CollidesWithGround(bone.RigidBody.Position))
                        bone.RigidBody.AddForce(new JVector(0, Math.Abs(bone.RigidBody.Force.Y), 0));
                }

                SatisfyConnections();
            }
        }

        public virtual void Render(Camera camera)
        {
            foreach (Connection connection in connections)
            {
                connection.Render(camera);
            }
            foreach (Bone bone in bones)
            {
                bone.Render(camera);
            }
        }

        #region private

        public virtual void Reset()
        {
            LoadSkeleton(FileName);
        }

        public void LoadSkeleton(string fileName)
        {
            bones = new List<Bone>();
		    connections = new List<Connection>();
		    muscles = new List<LinearMuscle>();
            SkeletonParserState state = SkeletonParserState.StartState;
            string line;
            int lineNumber = 0;
            int rootBoneCounter = 0;
            Dictionary<string, Bone> allBones = new Dictionary<string, Bone>();
            //ObjMesh boneMesh = new ObjMesh(.30f, 16);
            CultureInfo culture = new CultureInfo("en-US");
            char[] separators = { ',', ' ' };
            Random random = new Random();

            using (StreamReader reader = new StreamReader(fileName))
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
                            Bone bone = new Bone(boneName, new Vector3(x + position.X, y + position.Y, z + position.Z), this);

                            allBones.Add(boneName, bone);
                            bones.Add(bone);

                            if (boneName.Equals("Root"))
                            {
                                rootBoneCounter++;
                            }
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
                            throw new Exception("Error on line " + lineNumber + ". The words Bone or LinkState expected.");
                        }
                    }
                    else if (state == SkeletonParserState.LinkState)
                    {
                        string[] segments = line.Split(separators);
                        if (segments[0].Equals("Bone") && segments[2].Equals("Children"))
                        {
                            string boneName = segments[1];
                            Bone parentBone = allBones[boneName];

                            for (int i = 3; i < segments.Length; i++)
                            {
                                Bone childBone = allBones[segments[i]];

                                Connection connection = new Connection(this, parentBone, childBone, new Vector4(1, 0, 0, 1));

                                connections.Add(connection);
                            }

                        }
                        else if (segments[0].Equals("Muscles"))
                        {
                            state = SkeletonParserState.MuscleState;
                        }
                        else
                        {
                            throw new Exception("Error on line " + lineNumber + ". The words Bone or Children or Muscle expected.");
                        }
                    }
                    else if (state == SkeletonParserState.MuscleState)
                    {
                        string[] segments = line.Split(separators);
                        if (segments[0].Equals("Bone"))
                        {
                            Bone bone1 = allBones[segments[1]];
                            Bone bone2 = allBones[segments[2]];
                            float maxForce = float.Parse(segments[3], culture);
                            LinearMuscle muscle = new LinearMuscle(this, bone1, bone2, maxForce);
                            connections.Add(muscle.Connection);
                            muscles.Add(muscle);
                        }
                    }
                }
            }
            AttachToWorld();
        }

        private void AttachToWorld()
        {
            //Add all the bones' RigidBody to the world
            foreach (Bone bone in bones)
            {
                world.AddBody(bone.RigidBody);
            }
        }

        private void SatisfyConnections()
        {
            Dictionary<RigidBody, List<JVector>> storage = new Dictionary<RigidBody, List<JVector>>();

            //Collect all expected positions
            foreach (Connection connection in Connections)
            {
                JVector connectionVector = connection.GetConnectionVector();
                float currentLength = connectionVector.Length();

                if (currentLength > connection.MaxLength || currentLength < connection.MinLength)
                {
                    float expectedLength;

                    if (currentLength > connection.MaxLength)
                        expectedLength = connection.MaxLength;
                    else
                        expectedLength = connection.MinLength;

                    float lengthDifference = expectedLength - currentLength;

                    RigidBody first = connection.First.RigidBody;
                    RigidBody second = connection.Second.RigidBody;

                    connectionVector.Normalize();

                    JVector firstPosition;
                    JVector secondPosition;

                    //This is where the bodies should be
                    if (first.Position.Y > 0 && second.Position.Y > 0)
                    {
                        firstPosition = first.Position + connectionVector * lengthDifference * 0.5f;
                        secondPosition = second.Position - connectionVector * lengthDifference * 0.5f;
                    }
                    else if (first.Position.Y <= 0 && second.Position.Y <= 0)
                    {
                        firstPosition = first.Position;
                        secondPosition = second.Position;
                    }
                    else if (first.Position.Y <= 0)
                    {
                        firstPosition = first.Position;
                        secondPosition = second.Position - connectionVector * lengthDifference;
                    }
                    else// if (second.Position.Y <= 0)
                    {
                        firstPosition = first.Position + connectionVector * lengthDifference;
                        secondPosition = second.Position;
                    }

                    //Add the first
                    if (storage.ContainsKey(first))
                    {
                        storage[first].Add(firstPosition);
                    }
                    else
                    {
                        List<JVector> positionList = new List<JVector>();
                        positionList.Add(firstPosition);
                        storage.Add(first, positionList);
                    }

                    //Add the second
                    if (storage.ContainsKey(second))
                    {
                        storage[second].Add(secondPosition);
                    }
                    else
                    {
                        List<JVector> positionList = new List<JVector>();
                        positionList.Add(secondPosition);
                        storage.Add(second, positionList);
                    }
                }
            }

            RigidBody[] bodies = storage.Keys.ToArray<RigidBody>();

            //Put the rigidbody in the mean position
            foreach (RigidBody body in bodies)
            {
                List<JVector> list = storage[body];
                JVector vectorSum = new JVector(0);
                foreach (JVector vector in list)
                {
                    vectorSum += vector;
                }
                vectorSum = vectorSum * (1f / list.Count);
                if (vectorSum.Y > 0)
                    body.Position = vectorSum; //Change the position
                else
                {
                    body.Position = new JVector(vectorSum.X, 0, vectorSum.Z);
                }
            }

        }

        #endregion
    }
}
