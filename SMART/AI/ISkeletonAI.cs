using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.AI
{
    interface ISkeletonAI
    {
        /// <summary>
        /// Constructor that initializes the AI with the required parameters to be able to make decisions.
        /// </summary>
        /// <param name="environment">The AI must know about the environment to evaluate its performance.</param>
        /// <param name="myBody">The AI must know about its own body to evaluate its performance.</param>
        /// <param name="myMuscles">These are the muscles that the AI will act upon.</param>
        void Initialize(Scene environment, Skeleton myBody, List<Muscle> myMuscles);

        /// <summary>
        /// This method causes the AI to calculate new actions for its muscles.
        /// </summary>
        void Think();
    }
}
