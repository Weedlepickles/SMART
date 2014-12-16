using Jitter;
using Jitter.LinearMath;
using OpenTK;
using SMART.AI;
using SMART.AI.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class CowSkeleton : Skeleton
	{
		private int DicretizationSteps = 3;
		private float Amplitude = 1;
		private float Frequency = 5f;

		QLearningEngine AIEngine;
		CowLeg mFrontRightLeg;
		CowLeg mFrontLeftLeg;
		CowLeg mBackRightLeg;
		CowLeg mBackLeftLeg;
		private int nPositions = 100;
		List<JVector> positions = new List<JVector>();
		List<int> deltaTimes = new List<int>();

		ObjMesh CowMesh;
		Renderer meshRenderer;
		float minXSpeed = float.MaxValue;

		const int FRONT_RIGHT = 0;
		const int FRONT_LEFT = 1;
		const int BACK_RIGHT = 2;
		const int BACK_LEFT = 3;

		public CowSkeleton(string name, Vector3 position, SMARTWorld world, string fileName)
			: base(name, position, world, fileName)
		{
			AIEngine = QLearningEngine.Create(4, DicretizationSteps);

			QLearningControlPanel front = new QLearningControlPanel(AIEngine);

			mFrontRightLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[FRONT_RIGHT]);
			mFrontLeftLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[FRONT_LEFT]);
			mBackRightLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[BACK_RIGHT]);
			mBackLeftLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[BACK_LEFT]);

			front.Show();
			CowMesh = new ObjMesh("Models/Cow.obj");
			meshRenderer = new Renderer(CowMesh, new Vector4(0.6f, 0.4f, 0.2f, 1.0f));
		}

		public override void Reset()
		{
			base.Reset();
			positions = new List<JVector>();
			deltaTimes = new List<int>();
			AIEngine.Reset();
            AIEngine.RepeatAction = 20;
			mFrontRightLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[FRONT_RIGHT]);
			mFrontLeftLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[FRONT_LEFT]);
			mBackRightLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[BACK_RIGHT]);
			mBackLeftLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[BACK_LEFT]);
		}

		public void SaveState()
		{
			AIEngine.SaveState("AISavedState.txt");
		}

		public void SaveState(string filename)
		{
			AIEngine.SaveState(filename);
		}

		public void LoadState()
		{
			Reset();
			AIEngine = QLearningEngine.Create("AISavedState.txt");
		}

		public override void Render(Camera camera)
		{
			base.Render(camera);
			//meshRenderer.Render(camera, Matrix4.CreateRotationY((float)Math.PI / -2.0f) * Matrix4.CreateTranslation(new Vector3(-1.5f, 0, 0)) * Matrix4.CreateScale(1.7f));
		}

		private Bone subBone;
		private Bone bmBone;
		private int GetDirectionState()
		{
			foreach (Bone bone in Bones)
			{
				if (bone.Name.Equals("Sub"))
				{
					subBone = bone;
				}
				else if (bone.Name.Equals("BM"))
				{
					bmBone = bone;
				}
			}

			JVector directionVector = bmBone.RigidBody.Position - subBone.RigidBody.Position;
			directionVector.Normalize();

			float value = directionVector * JVector.Left;

			int result;
			if (value > 0.8)
				result = 0;
			else if (directionVector.Z > 0)
				result = 1;
			else
				result = 2;
			return result;
		}

		private int GetMuscleState(LinearMuscle muscle)
		{
			float maxLength = muscle.Connection.MaxLength;
			float minLength = muscle.Connection.MinLength;
			float length = muscle.Connection.Length;
			float lengthSpan = maxLength - minLength;
			length = length - minLength;

			if (length < 0.33 * lengthSpan)
				return 0;
			else if (length < 0.66 * lengthSpan)
				return 1;
			else
				return 2;

		}

		private Bone GetBone(string name)
		{
			foreach (Bone bone in Bones)
			{
				if (bone.Name.Equals(name))
					return bone;
			}
			return null;
		}

		private long iterationNumber = 0;
		private long resetTimer = 0;
		public override void Update(TimeSpan deltaTime)
		{
			base.Update(deltaTime);
			//float reward = CalculateReward(deltaTime);
			float reward = CalculateRewardII(deltaTime);

			if (CheckIfRestartNeeded())
			{
				reward = 0;
			}

			Bone sub = GetBone("Sub");
			if (sub != null && sub.RigidBody.Position.X > 55)
			{
				reward = 1;
			}

			List<int> state2 = new List<int>();

			/*
			foreach (LinearMuscle muscle in Muscles)
			{
				state2.Add(GetMuscleState(muscle));
			}*/

			int temp1 = mFrontRightLeg.GetOmegaSteps();
			int temp2 = mFrontRightLeg.Frequency;
			if (temp1 > 2 || temp2 > 2)
				throw new Exception();
			state2.Add(temp1);
			state2.Add(temp2);

			temp1 = mFrontLeftLeg.GetOmegaSteps();
			temp2 = mFrontLeftLeg.Frequency;
			if (temp1 > 2 || temp2 > 2)
				throw new Exception();
			state2.Add(temp1);
			state2.Add(temp2);

			temp1 = mBackRightLeg.GetOmegaSteps();
			temp2 = mBackRightLeg.Frequency;
			if (temp1 > 2 || temp2 > 2)
				throw new Exception();
			state2.Add(temp1);
			state2.Add(temp2);

			temp1 = mBackLeftLeg.GetOmegaSteps();
			temp2 = mBackLeftLeg.Frequency;
			if (temp1 > 2 || temp2 > 2)
				throw new Exception();
			state2.Add(temp1);
			state2.Add(temp2);
			

			//state2.Add(GetDirectionState());

			int[] action = AIEngine.GetAction(state2, reward);

			/*
			int i = 0;
			foreach (LinearMuscle muscle in Muscles)
			{
				if (action[i] == 0)
					muscle.Strength = -1;
				else if (action[i] == 1)
					muscle.Strength = 0;
				else
					muscle.Strength = 1;
				i++;
			}*/

			mFrontRightLeg.Frequency = action[3];
			mFrontLeftLeg.Frequency = action[2];
			mBackRightLeg.Frequency = action[1];
			mBackLeftLeg.Frequency = action[0];

			mFrontRightLeg.Update(deltaTime);
			mFrontLeftLeg.Update(deltaTime);
			mBackRightLeg.Update(deltaTime);
			mBackLeftLeg.Update(deltaTime);

			if (CheckIfRestartNeeded())
			{
				Reset();
				resetTimer = iterationNumber + 6000;
				Console.WriteLine("Fallen: Next reset at " + resetTimer + " iterations.");
			}

			if (sub != null && sub.RigidBody.Position.X > 55)
			{
				Reset();
				resetTimer = iterationNumber + 6000;
				Console.WriteLine("Maxed: Next reset at " + resetTimer + " iterations.");
			}

			if (resetTimer < iterationNumber)
			{
				Reset();
				resetTimer = iterationNumber + 6000;
				Console.WriteLine("Timeout: Next reset at " + resetTimer + " iterations.");
			}

			if (iterationNumber % 1000 == 0)
				Console.WriteLine("We have now run " + iterationNumber + " iterations. We have discovered " + AIEngine.GetCombinations() + " so far.");
			iterationNumber++;

		}

		private bool CheckIfRestartNeeded()
		{
			if (Bones.Find(b => b.Name.Contains("FL")).RigidBody.Position.Y < 0.1f) return true;
			if (Bones.Find(b => b.Name.Contains("FM")).RigidBody.Position.Y < 0.1f) return true;
			if (Bones.Find(b => b.Name.Contains("FR")).RigidBody.Position.Y < 0.1f) return true;
			if (Bones.Find(b => b.Name.Contains("BL")).RigidBody.Position.Y < 0.1f) return true;
			if (Bones.Find(b => b.Name.Contains("BM")).RigidBody.Position.Y < 0.1f) return true;
			if (Bones.Find(b => b.Name.Contains("BR")).RigidBody.Position.Y < 0.1f) return true;
			return false;
		}

		private void UpdateLeg(CowLeg leg, int[] action)
		{
			leg.Frequency = action[0];
			leg.Amplitude = action[1];
		}

		private float CalculateReward(TimeSpan deltaTime)
		{
			float treshold = 0.0001f;
			float max = 0.002f;
			JVector speed = CalculateSpeed(deltaTime);
			if (speed.X < minXSpeed)
			{
				minXSpeed = speed.X;
				//Console.WriteLine("New min x speed: " + minXSpeed);
			}
			if (CalculateAveragePosition().Y < 3f)
			{
				return -0.5f;
			}
			if (speed.X < -treshold)
			{
				float ds = (speed.X * -1) - treshold;
				//Console.WriteLine("Reward!!!");
				float r = ds / max;
				return (r > 1) ? 1f : r;
			}
			return 0;
		}

		private int printCounter = 0;
		private float rewardCounter = 0;
		private JVector meanSubPosition = new JVector(0, 0, 0);
		private Dictionary<Bone, JVector> bonePositions = new Dictionary<Bone, JVector>();
		private float CalculateRewardII(TimeSpan deltaTime)
		{
			float heightFactor = 5;
			float velocityFactor = 0.3f;

			float reward = 0;

			foreach (Bone bone in Bones)
			{
				if (bone.Name.Equals("Sub"))
				{
					reward += bone.RigidBody.Position.X * 0.0001f + 0.1f;
					reward += bone.RigidBody.Position.Y * 0.000001f;

					meanSubPosition = meanSubPosition + bone.RigidBody.Position;

					/*
					//Give a reward for having progressed toward X
					float temp = (float)Math.Log10(bone.RigidBody.Position.X) * 20;
					if (!float.IsNaN(temp) && !float.IsInfinity(temp))
						reward += temp;
					else
						reward += -(bone.RigidBody.Position.X * bone.RigidBody.Position.X);

					reward += bone.RigidBody.Position.Y * heightFactor;

					JVector speedVector = GetSpeed(bone, deltaTime);
					float speed = speedVector.X;

					reward += speed * 10000 * velocityFactor;

					meanSubPosition = meanSubPosition + bone.RigidBody.Position;

					//if (printCounter == 1)
					//	Console.WriteLine("Sub position X: " + meanSubPosition + bone.RigidBody.Position);
					 */
				}
				else if (bone.Name.Equals("HFR"))
				{
				}
				else if (bone.Name.Equals("HFL"))
				{
				}
				else if (bone.Name.Equals("HBR"))
				{
				}
				else if (bone.Name.Equals("HBL"))
				{
				}
			}

			bonePositions = new Dictionary<Bone, JVector>();
			foreach (Bone bone in Bones)
			{
				bonePositions.Add(bone, bone.RigidBody.Position);
			}


			rewardCounter += reward;
			if (printCounter > 150)
			{
				Console.WriteLine("Reward: " + Math.Round(rewardCounter / 150, 5) + "  X: " + Math.Round(meanSubPosition.X * 0.00667f, 2));
				rewardCounter = 0;
				printCounter = 0;
				meanSubPosition = new JVector(0, 0, 0);
			}
			printCounter++;

			return reward;
		}

		private JVector GetSpeed(Bone bone, TimeSpan deltaTime)
		{
			if (bonePositions.ContainsKey(bone))
			{
				JVector lastPosition = bonePositions[bone];
				JVector currentPosition = bone.RigidBody.Position;
				JVector distanceVector = currentPosition - lastPosition;
				JVector speedVector = distanceVector * (float)(1 / deltaTime.TotalMilliseconds);
				return speedVector;
			}
			return JVector.Zero;
		}

		private List<int> CreateState(CowLeg leg, CowLeg alignedLeg, CowLeg oppositeLeg)
		{
			List<int> state = new List<int>();
			state.Add(leg.GetOmegaSteps());
			state.Add(alignedLeg.GetOmegaSteps());
			state.Add(oppositeLeg.GetOmegaSteps());
			return state;
		}

		private JVector CalculateSpeed(TimeSpan deltaTime)
		{
			positions.Add(CalculateAveragePosition());
			deltaTimes.Add(deltaTime.Milliseconds);
			while (positions.Count > nPositions)
			{
				positions.RemoveAt(0);
				deltaTimes.RemoveAt(0);
			}
			JVector dPos = positions[0] - positions[positions.Count - 1];
			int totalMillis = 0;
			foreach (int t in deltaTimes)
			{
				totalMillis += t;
			}
			dPos.X /= totalMillis;
			dPos.Y /= totalMillis;
			dPos.Z /= totalMillis;
			return dPos;
		}

		private JVector CalculateAveragePosition()
		{
			JVector totPos = JVector.Zero;
			foreach (Bone bone in Bones)
			{
				totPos += bone.RigidBody.Position;
			}
			totPos.X /= Bones.Count;
			totPos.Y /= Bones.Count;
			totPos.Z /= Bones.Count;
			return totPos;
		}

		#region Private classes

		class CowLeg
		{
			private const float TWO_PI = (float)(2 * Math.PI);
			private const float ANGLE_STEP = TWO_PI / 1000f;
			private int mDicretizationSteps;
			private float maxAmp, maxFreq;
			public int Amplitude, Frequency;
			public float CurrentOmega { get; private set; }
			public LinearMuscle mMuscle;

			public CowLeg(int steps, float maxAmplitude, float maxFrequency, LinearMuscle muscle)
			{
				mMuscle = muscle;
				mDicretizationSteps = steps;
				maxAmp = maxAmplitude;
				maxFreq = maxFrequency;
				Amplitude = mDicretizationSteps;
				Frequency = mDicretizationSteps;
				CurrentOmega = 0;
			}

			public void Update(TimeSpan deltaTime)
			{
				float f = (maxFreq / mDicretizationSteps) * Frequency;
				float dW = ANGLE_STEP * f * deltaTime.Milliseconds;
				CurrentOmega = (CurrentOmega + dW) % TWO_PI;
				float a = (maxAmp / mDicretizationSteps) * Amplitude;
				mMuscle.Strength = ((float)Math.Sin(((double)CurrentOmega))) * a;
			}

			public int GetOmegaSteps()
			{
				//return (int)Math.Round(mMuscle.GetState() * mDicretizationSteps);
				return (int)Math.Round((CurrentOmega / TWO_PI) * 3);
			}
		}

		#endregion
	}
}
