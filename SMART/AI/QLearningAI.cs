using SMART.AI.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Engine;

namespace SMART.AI
{
    
    class QLearningAI : ISkeletonAI
    {
        #region Private members

        private QLearningEngine mQEngine;
        private Scene mEnvironment;
        private Skeleton mSkeleton;
        private List<Muscle> mMuscles;

        #endregion

        #region Public members

        #endregion

        public QLearningAI(Scene environment, Skeleton myBody, List<Muscle> myMuscles)
        {
            Initialize(environment, myBody, myMuscles);
        }

        public void Initialize(Scene environment, Skeleton myBody, List<Muscle> myMuscles)
        {
            mEnvironment = environment;
            mSkeleton = myBody;
            mMuscles = myMuscles;
            mQEngine = QLearningEngine.Create(myMuscles.Count, 3);
        }

        public void Think()
        {
            int[] myAction = mQEngine.GetAction(GenState(), CalculateReward());
        }

        private List<int> GenState()
        {
            // TODO: Implement this
            return null;
        }

        private float CalculateReward()
        {
            // TODO: Implement this
            return 0;
        }

        private void PerformAction(int[] action) {
            // TODO: Implement this
        }
    }
}
