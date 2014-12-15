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
        private SMARTWorld mEnvironment;
        private Skeleton mSkeleton;
        private List<LinearMuscle> mMuscles;

        #endregion

        #region Public members

        #endregion

        public QLearningAI(SMARTWorld environment, Skeleton myBody)
        {
            Initialize(environment, myBody);
        }

        public void Initialize(SMARTWorld environment, Skeleton myBody)
        {
            mEnvironment = environment;
            mSkeleton = myBody;
            mMuscles = myBody.Muscles;
            mQEngine = QLearningEngine.Create(mMuscles.Count, 3);
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
