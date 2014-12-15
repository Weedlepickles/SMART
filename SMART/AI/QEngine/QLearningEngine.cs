using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.AI.Engine
{
    
    /// <summary>
    /// A standard QLearning Engine
    /// 
    /// Author:
    /// Emil Olofsson
    /// emiol791@student.liu.se
    /// </summary>
    class QLearningEngine
    {

        #region Engine entities

        protected class QAction
        {
            public int[] Parameters;
            private int hash;

            public QAction(int nParams, int nValues, int id)
            {
                Parameters = new int[nParams];
                int id_cpy = id;
                int paramId = 0;
                while (id_cpy > 0 && paramId < nParams)
                {
                    int t = id_cpy % nValues;
                    Parameters[paramId] = t;
                    id_cpy = (id_cpy - t) / nValues;
                    paramId++;
                }
                hash = id;
            }

            public QAction(string fromString)
            {
                string[] parts = fromString.Split(';');
                int nParams = 0;
                int.TryParse(parts[0], out nParams);
                if (parts.Length != nParams + 2)
                    throw new FormatException("Invalid action format. Parameter count mismatch.");
                Parameters = new int[nParams];
                for (int i = 0; i < nParams; i++)
                {
                    int param = 0;
                    int.TryParse(parts[i + 1], out param);
                    Parameters[i] = param;
                }
                hash = 0;
                int.TryParse(parts[nParams + 1], out hash);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Parameters.Length);
                sb.Append(';');
                foreach (int i in Parameters)
                {
                    sb.Append(i);
                    sb.Append(';');
                }
                sb.Append(hash);
                return sb.ToString();
            }

            public override bool Equals(Object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                QAction p = obj as QAction;
                if ((Object)p == null)
                {
                    return false;
                }

                return (hash == p.GetHashCode());
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }

        protected class QState
        {
            int hash;
            
            public QState(List<int> stateVariables)
            {
                string hashStr = "#QState#";
                foreach (int val in stateVariables)
                {
                    hashStr += val + "#";
                }
                hash = hashStr.GetHashCode();
            }

            public QState(string fromString)
            {
                hash = 0;
                int.TryParse(fromString, out hash);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(hash);
                return sb.ToString();
            }

            public override bool Equals(Object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                QState p = obj as QState;
                if ((Object)p == null)
                {
                    return false;
                }

                return (hash == p.GetHashCode());
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }
        
        protected class QStateActionPair
        {
            public QState State { get; protected set; }
            public QAction Action { get; protected set; }
            private int hash;
            public QStateActionPair(QState state, QAction action)
            {
                State = state;
                Action = action;
                hash = ("" + State.GetHashCode() + Action.GetHashCode()).GetHashCode();
            }

            public QStateActionPair(String fromString)
            {
                string[] parts = fromString.Split('#');
                if(parts.Length != 2)
                    throw new FormatException("Invalid QStateActionPair format. Parameter count mismatch.");
                State = new QState(parts[0]);
                Action = new QAction(parts[1]);
                hash = ("" + State.GetHashCode() + Action.GetHashCode()).GetHashCode();
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(State.ToString());
                sb.Append('#');
                sb.Append(Action.ToString());
                return sb.ToString();
            }

            public override bool Equals(Object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                QStateActionPair p = obj as QStateActionPair;
                if ((Object)p == null)
                {
                    return false;
                }

                return (hash == p.GetHashCode());
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }

        #endregion

        #region Creation

        /// <summary>
        /// Creates a new QLearningEngine initialized with a set of actions specified.
        /// </summary>
        /// <param name="numberOfActions">Actions will be selected from within this range.</param>
        /// <returns>The engine instance.</returns>
        public static QLearningEngine Create(int numberOfActions)
        {
            QLearningEngine engine = new QLearningEngine();
            for(int i = 0; i < numberOfActions; i++) {
                engine.QActions.Add(new QAction(1, int.MaxValue, i));
            }
            return engine;
        }

        /// <summary>
        /// Creates a new QLearningEngine initialized with a set of actions specified.
        /// </summary>
        /// <param name="numberOfActionParameters">Each action can come with a range of parameters.</param>
        /// <param name="maxParameterValues">The different values each parameter can take.</param>
        /// <returns>The engine instance.</returns>
        public static QLearningEngine Create(int numberOfActionParameters, int maxParameterValues)
        {
            QLearningEngine engine = new QLearningEngine();
            int numberOfActions = (int) Math.Pow(maxParameterValues, numberOfActionParameters);
            for (int i = 0; i < numberOfActions; i++)
            {
                engine.QActions.Add(new QAction(numberOfActionParameters, maxParameterValues, i));
            }
            return engine;
        }

        /// <summary>
        /// Creates a new QLearningEngine from a saved engine state file.
        /// </summary>
        /// <param name="fromFile">The file with the saved data.</param>
        /// <returns>The engine instance.</returns>
        public static QLearningEngine Create(string fromFile)
        {
            QLearningEngine engine = new QLearningEngine();
            StreamReader file = new StreamReader(fromFile);
            float factor = 0f;
            float.TryParse(file.ReadLine(), out factor);
            engine.LearningRate = factor;
            float.TryParse(file.ReadLine(), out factor);
            engine.DiscountFactor = factor;
            float.TryParse(file.ReadLine(), out factor);
            engine.ExplorationFactor = factor;
            float.TryParse(file.ReadLine(), out factor);
            engine.RepeatAction = factor;
            int numberOfActions = 0;
            int.TryParse(file.ReadLine(), out numberOfActions);
            for (int i = 0; i < numberOfActions; i++)
            {
                engine.QActions.Add(new QAction(file.ReadLine()));
            }
            int numberOfPairs = 0;
            int.TryParse(file.ReadLine(), out numberOfPairs);
            for (int i = 0; i < numberOfPairs; i++)
            {
                string[] parts = file.ReadLine().Split('|');
                QStateActionPair pair = new QStateActionPair(parts[0]);

                // Look for existing actions. Let garbage collector dispose of unnecessary instances of action.
                // Saves some memory I recon.
                QAction existingAction = engine.QActions.Find(a => a.Equals(pair.Action));
                if (existingAction != null)
                    pair = new QStateActionPair(pair.State, existingAction);

                float qVal = 0;
                float.TryParse(parts[1], out qVal);
                int nVal = 0;
                int.TryParse(parts[2], out nVal);
                engine.QTable[pair] = qVal;
                engine.NTable[pair] = nVal;
            }
            file.Close();
            return engine;
        }

        protected QLearningEngine()
        {
            LearningRate = .9f;
            DiscountFactor = .8f;
            ExplorationFactor = .5f;
            RepeatAction = 30;
        }

        #endregion

        #region QLearning essentials

        protected List<QAction> QActions = new List<QAction>();
        private Random Rnd = new Random();
        protected Hashtable QTable = new Hashtable();
        protected Hashtable NTable = new Hashtable();
        private QStateActionPair previousStateAction = null;
        private QStateActionPair maxStateAction = null;
        private QStateActionPair minExploredStateAction = null;
        private float previousReward = .0f;
        private Dictionary<int, int> actionCount = new Dictionary<int, int>();

        private float FindMaxStateActionQValue(QState state)
        {
            float bestQValue = 0;
            float bestNValue = int.MaxValue;;
            float tmpQValue = 0;
            int tmpNValue = int.MaxValue;
            QStateActionPair pair = null;
            foreach (QAction action in QActions)
            {
                pair = new QStateActionPair(state, action);
                Object t = QTable[pair];
                tmpQValue = (t == null) ? 0 : (float)t;
                t = NTable[pair];
                tmpNValue = (t == null) ? 0 : (int)t;
                if (tmpQValue > bestQValue)
                {
                    bestQValue = tmpQValue;
                    maxStateAction = pair; //Remember best state action
                }
                if (tmpNValue < bestNValue)
                {
                    bestNValue = tmpNValue;
                    minExploredStateAction = pair;
                }
            }
            return bestQValue;
        }

        private QAction DecideAction(QState state)
        {
            if (Rnd.NextDouble() < ExplorationFactor || maxStateAction == null || maxStateAction.Action == null)
            {
                //On explore:
                if (minExploredStateAction != null)
                    return minExploredStateAction.Action;

                return QActions[Rnd.Next(QActions.Count)];
            }
            return maxStateAction.Action;
        }

        private int GetNValue(QStateActionPair pair)
        {
            Object t = NTable[pair];
            return (t == null) ? 0 : (int)t;
        }

        #endregion

        #region Public

        /// <summary>
        /// Determines how to make use of new knowledge.
        /// Range [0, 1]: [Never learn = 0 ... Always overwrite old knowledge = 1]
        /// </summary>
        public float LearningRate { get; set; }

        /// <summary>
        /// Determines importance of future values.
        /// Range [0, 1]: [Only care about now = 0 ... Plan forever ahead = 1]
        /// </summary>
        public float DiscountFactor { get; set; }

        /// <summary>
        /// Determines the chance to take a random action.
        /// Range [0, 1]: [Optimize performance = 0 ... Totally random exploration = 1]
        /// </summary>
        public float ExplorationFactor { get; set; }

        /// <summary>
        /// Limits the amount of times an action is repeated.
        /// Range [0, 100]: [Choose new action always = 0 ... Repeat 100 times = 100]
        /// </summary>
        public float RepeatAction { get; set; }

        /// <summary>
        /// Updates the Q-Table with state and reward info. It then decides what action to take.
        /// </summary>
        /// <param name="currentState">A list of all values that defines the current state.</param>
        /// <param name="currentReward">The reward that should be associated with this state. Should be kept between 0 and 1.</param>
        /// <returns>An array of integers that specifies what action to take.</returns>
        public int[] GetAction(List<int> currentState, float currentReward)
        {
            return GetAction(currentState, currentReward, 0);
        }

        /// <summary>
        /// Updates the Q-Table with state and reward info. It then decides what action to take.
        /// </summary>
        /// <param name="currentState">A list of all values that defines the current state.</param>
        /// <param name="currentReward">The reward that should be associated with this state. Should be kept between 0 and 1.</param>
        /// <param name="currentReward">If several entities use the same engine, they can be specified here.</param>
        /// <returns>An array of integers that specifies what action to take.</returns>
        public int[] GetAction(List<int> currentState, float currentReward, int uniqueEntity)
        {
            QState state = new QState(currentState);
            float oldQValue = 0;
            float maxQValue = FindMaxStateActionQValue(state);
            QAction action = DecideAction(state);

            QStateActionPair currentStateAction = new QStateActionPair(state, action);
            if (previousStateAction != null)
            {
                if (!actionCount.ContainsKey(uniqueEntity))
                    actionCount.Add(uniqueEntity, 0);
                int ac = 0;
                actionCount.TryGetValue(uniqueEntity, out ac);
                if (previousStateAction.State.Equals(state) && ac < RepeatAction)
                {
                    actionCount[uniqueEntity] = (ac + 1);
                    return previousStateAction.Action.Parameters;
                }

                actionCount[uniqueEntity] = 0;

                NTable[currentStateAction] = GetNValue(currentStateAction) + 1;
                    //return currentStateAction.Action.Parameters;

                object t = QTable[previousStateAction];
                if (t != null)
                {
                    oldQValue = (float)t;
                }

                QTable[previousStateAction] =
                    (float)(oldQValue + LearningRate * (currentReward + DiscountFactor * maxQValue - oldQValue)); //previousReward ?
            }

            previousStateAction = currentStateAction;
            previousReward = currentReward;

            return action.Parameters;
        }

        /// <summary>
        /// Resets the engine. Do this if you reset the state of your simulator.
        /// </summary>
        public void Reset()
        {
            previousStateAction = null;
            previousReward = .0f;
        }

        /// <summary>
        /// Saves the entire state-space to file.
        /// </summary>
        /// <param name="fileName">The file in which to store data.</param>
        public void SaveState(string fileName)
        {
            StreamWriter file = new StreamWriter(fileName);
            IEnumerable<QStateActionPair> pairs = QTable.Keys.Cast<QStateActionPair>();
            float qVal = 0;
            int nVal = 0;
            object t;
            file.WriteLine(LearningRate);
            file.WriteLine(DiscountFactor);
            file.WriteLine(ExplorationFactor);
            file.WriteLine(RepeatAction);
            file.WriteLine(QActions.Count);
            foreach(QAction action in QActions) {
                file.WriteLine(action.ToString());
            }
            file.WriteLine(pairs.ToList<QStateActionPair>().Count);
            foreach (QStateActionPair pair in pairs)
            {
                t = QTable[pair];
                qVal = (float)t;
                t = NTable[pair];
                nVal = (t == null) ? 0 : (int)t;
                file.WriteLine(pair.ToString() + "|" + qVal + "|" + nVal);
            }
            file.Close();
        }

        #endregion

    }
}
