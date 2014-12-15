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

            mFrontRightLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[FRONT_RIGHT]);
            mFrontLeftLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[FRONT_LEFT]);
            mBackRightLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[BACK_RIGHT]);
            mBackLeftLeg = new CowLeg(DicretizationSteps, Amplitude, Frequency, this.Muscles[BACK_LEFT]);
        }

        public void SaveState()
        {
            AIEngine.SaveState("AISavedState.txt");
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

        public override void Update(TimeSpan deltaTime)
        {
            base.Update(deltaTime);
            float reward = CalculateReward(deltaTime);

            //this.Muscles[4].Strength = 0.1f;
            //this.Muscles[5].Strength = 0.1f;
            //this.Muscles[6].Strength = 0.1f;
            //this.Muscles[7].Strength = 0.1f;

            List<int> state2 = new List<int>();
            state2.Add(mFrontRightLeg.GetOmegaSteps());
            state2.Add(mFrontRightLeg.Frequency);

            state2.Add(mFrontLeftLeg.GetOmegaSteps());
            state2.Add(mFrontLeftLeg.Frequency);

            state2.Add(mBackRightLeg.GetOmegaSteps());
            state2.Add(mBackRightLeg.Frequency);

            state2.Add(mBackLeftLeg.GetOmegaSteps());
            state2.Add(mBackLeftLeg.Frequency);

            int[] action = AIEngine.GetAction(state2, reward);

            mFrontRightLeg.Frequency = action[0];
            mFrontLeftLeg.Frequency = action[1];
            mBackRightLeg.Frequency = action[2];
            mBackLeftLeg.Frequency = action[3];

            mFrontRightLeg.Update(deltaTime);
            mFrontLeftLeg.Update(deltaTime);
            mBackRightLeg.Update(deltaTime);
            mBackLeftLeg.Update(deltaTime);

            if (CalculateAveragePosition().Y < 2.5f)
            {
                Reset();
            }
            
        }

        private void UpdateLeg(CowLeg leg, int[] action)
        {
            leg.Frequency = action[0];
            leg.Amplitude = action[1];
        }

        private float CalculateReward(TimeSpan deltaTime) {
            float treshold = 0.0001f;
            float max = 0.002f;
            JVector speed = CalculateSpeed(deltaTime);
            if(speed.X < minXSpeed) {
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

            public CowLeg(int steps, float maxAmplitude, float maxFrequency, LinearMuscle muscle) {
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
                return (int)Math.Round((CurrentOmega / TWO_PI) * 4);
            }
        }

        #endregion
    }
}
