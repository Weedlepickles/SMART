using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.AI.NeuralNetwork
{
    /// <summary>
    /// A Neural Network engine designed to output several "wires", or sets of actions if you like.
    /// 
    /// Author:
    /// Emil Olofsson
    /// emiol791@student.liu.se
    /// </summary>
    class NNEngine
    {

        #region Enums

        public enum NeuronType
        {
            TAN_SIGMOIDAL,
            LOG_SIGMOIDAL,
            PURE_LINEAR
        };

        #endregion

        #region Network data

        private List<ActivationLink> mInputs = new List<ActivationLink>();
        private List<ActivationLink> mOutputs = new List<ActivationLink>();
        private List<List<Neuron>> mNeurons = new List<List<Neuron>>();
        private NeuronType mNetworkNeuronType = NeuronType.PURE_LINEAR;
        private int mNWires = 1;
        private List<float> prevState = null;

        #endregion

        #region Creation

        public NNEngine(int inDimension, int outDimension, int hiddenLayers, int wires, NeuronType transferFunctionType)
        {
            if (inDimension <= 0 || outDimension <= 0 || wires < 1 || hiddenLayers < 0)
                throw new ArgumentOutOfRangeException();

            mNetworkNeuronType = transferFunctionType;
            mNWires = wires;

            for (int i = 0; i < inDimension; i++)
            {
                mInputs.Add(new ActivationLink());
            }

            List<ActivationLink> inLinks = mInputs;
            ActivationLink feeder = new ActivationLink();
            feeder.Value = 1.0f;
            inLinks.Add(feeder);
            // For all hidden layers
            for (int j = 0; j < hiddenLayers; j++)
            {
                mNeurons.Add(new List<Neuron>());
                List<ActivationLink> linkBuilder = new List<ActivationLink>();
                // Create a neuron for each dimension
                for (int i = 0; i < inDimension; i++)
                {
                    Neuron neuron = null;
                    switch (mNetworkNeuronType)
                    {
                        case NeuronType.LOG_SIGMOIDAL:
                            neuron = new LogSigmoidalNeuron(inLinks);
                            break;
                        case NeuronType.TAN_SIGMOIDAL:
                            neuron = new TanSigmoidalNeuron(inLinks);
                            break;
                        case NeuronType.PURE_LINEAR:
                            neuron = new PureLinearNeuron(inLinks);
                            break;
                    }
                    mNeurons[j].Add(neuron);
                    linkBuilder.Add(neuron.OutLink);
                }
                inLinks = linkBuilder;
                inLinks.Add(feeder);
            }

            // Now we create the output layer
            mNeurons.Add(new List<Neuron>());
            int nOutLinks = outDimension * wires;
            for (int i = 0; i < outDimension; i++)
            {
                Neuron neuron = null;
                switch (mNetworkNeuronType)
                {
                    case NeuronType.LOG_SIGMOIDAL:
                        neuron = new LogSigmoidalNeuron(inLinks);
                        break;
                    case NeuronType.TAN_SIGMOIDAL:
                        neuron = new TanSigmoidalNeuron(inLinks);
                        break;
                    case NeuronType.PURE_LINEAR:
                        neuron = new PureLinearNeuron(inLinks);
                        break;
                }
                mNeurons[hiddenLayers].Add(neuron);
                mOutputs.Add(neuron.OutLink);
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Propagates a state vector through the network, generating n proposed action vectors, where n is the total number of desired wires.
        /// </summary>
        /// <param name="state">The state vector containing all state variables. Must have the same dimension as the given inDimension.</param>
        /// <returns>A list of proposed action vectors with all action variables</returns>
        public List<List<float>> PropagateState(List<float> state)
        {
            if (state.Count + 1 != mInputs.Count)
                throw new ArgumentOutOfRangeException("State must have the same dimension as the given inDimension.");

            // Put state on inputs
            for(int i = 0; i < state.Count; i++) {
                mInputs[i].Value = state[i];
            }

            // Propagate through every layer
            foreach (List<Neuron> layer in mNeurons)
            {
                foreach (Neuron neuron in layer)
                {
                    neuron.Propagate();
                }
            }

            // Compile output vector
            List<List<float>> output = new List<List<float>>();
            int outDim = mOutputs.Count / mNWires;
            for (int i = 0; i < mNWires; i++)
            {
                output.Add(new List<float>());
                for (int j = 0; j < outDim; j++)
                {
                    output[i].Add(mOutputs[i * outDim + j].Value);
                }
            }

            prevState = state;
            return output;
        }

        /// <summary>
        /// Propagates back adjustments on weights through the network to adapt to the training data
        /// </summary>
        /// <param name="trainingData">The "correct" set of data that should be outputted for thet particular state.</param>
        /// <param name="learningFactor">Determines how to make use of new knowledge.</param>
        public void BackPropagate(List<List<float>> trainingData, float learningFactor)
        {
            if (prevState == null) return;
            
            // For every neuron in output layer
            List<Neuron> outputLayer = mNeurons[mNeurons.Count - 1];
            int outDim = outputLayer.Count / mNWires;
            for (int i = 0; i < mNWires; i++)
            {
                for (int j = 0; j < outDim; j++)
                {
                    Neuron outNeuron = outputLayer[i * outDim + j];
                    float tData = trainingData[i][j];
                    float dAF = outNeuron.ActivationFunctionDerivative(); // (outNeuron.OutLink.Value - outNeuron.PreviousOutput) / dState;
                    outNeuron.Delta = dAF * (tData - outNeuron.OutLink.Value);
                }
            }

            // For the rest of the layers
            for (int i = mNeurons.Count - 2; i >= 0; i--)
            {
                for (int j = 0; j < mNeurons[i].Count; j++)
                {
                    Neuron neuron = mNeurons[i][j];
                    float dAF = neuron.ActivationFunctionDerivative(); // (neuron.OutLink.Value - neuron.PreviousOutput) / dState;
                    float wSum = 0f;
                    foreach (Neuron nj in mNeurons[i + 1])
                    {
                        wSum += nj.InLinks[j].Weight * nj.Delta;
                    }
                    neuron.Delta = dAF * wSum;
                }
            }

            foreach (List<Neuron> layer in mNeurons)
            {
                foreach (Neuron neuron in layer)
                {
                    neuron.AdjustWeights(learningFactor);
                }
            }
        }

        /// <summary>
        /// Resets the network to the initial state while keeping all previous data.
        /// Useful for when resetting the model for example.
        /// </summary>
        public void Reset()
        {
            prevState = null;
            //dState = 0f;
        }

        #endregion

        #region Entities

        private abstract class Neuron
        {
            public List<InputLink> InLinks { get; protected set; }
            public ActivationLink OutLink { get; protected set; }
            public float Delta { get; set; }
            public float PreviousOutput { get; protected set; }

            public Neuron(List<ActivationLink> inLinks)
            {
                InLinks = new List<InputLink>();
                foreach (ActivationLink activation in inLinks)
                {
                    InLinks.Add(new InputLink(inLinks.Count, activation));
                }
                OutLink = new ActivationLink();
            }

            public void Propagate()
            {
                PreviousOutput = OutLink.Value;
                ActivationFunction();
            }

            public void AdjustWeights(float learningFactor)
            {
                foreach (InputLink inLink in InLinks)
                {
                    inLink.Weight += learningFactor * (Delta / (inLink.Value * InLinks.Count));
                }
            }

            protected abstract void ActivationFunction();

            public abstract float ActivationFunctionDerivative();
        }

        private class TanSigmoidalNeuron : Neuron
        {
            private const double PI_HALF = Math.PI / 2.0;

            public TanSigmoidalNeuron(List<ActivationLink> inLinks)
                : base(inLinks) { }

            protected override void ActivationFunction()
            {
                float sum = 0;
                foreach (InputLink link in InLinks)
                {
                    sum += link.Weight * link.Value;
                }
                OutLink.Value = (float) (Math.Atan(sum) / PI_HALF);
            }

            public override float ActivationFunctionDerivative()
            {
                // TODO: implement this
                return 1.0f;
            }
        }

        private class LogSigmoidalNeuron : Neuron
        {
            public LogSigmoidalNeuron(List<ActivationLink> inLinks)
                : base(inLinks) { }

            protected override void ActivationFunction()
            {
                float sum = 0;
                foreach (InputLink link in InLinks)
                {
                    sum += link.Weight * link.Value;
                }
                OutLink.Value = (float)(Math.Log10(sum));
            }

            public override float ActivationFunctionDerivative()
            {
                // TODO: implement this
                return 1.0f;
            }
        }

        private class PureLinearNeuron : Neuron
        {
            public PureLinearNeuron(List<ActivationLink> inLinks)
                : base(inLinks) { }

            protected override void ActivationFunction()
            {
                float sum = 0, wsum = 0;
                foreach (InputLink link in InLinks)
                {
                    sum += link.Weight * link.Value;
                    wsum += link.Weight;
                }
                OutLink.Value = sum; // wsum;
            }

            public override float ActivationFunctionDerivative()
            {
                float wsum = 0;
                foreach (InputLink link in InLinks)
                {
                    wsum += link.Weight;
                }
                return 1.0f; // wsum;
            }
        }

        private class ActivationLink
        {
            public float Value { get; set; }
        }

        private class InputLink
        {
            private static Random mRnd = new Random();
            public float Weight { get; set; }
            public ActivationLink Activation { get; protected set; }
            public float Value
            {
                get
                {
                    return Activation.Value;
                }
                set
                {
                    Activation.Value = value;
                }
            }
            public InputLink(int networkDimension, ActivationLink value)
            {
                Activation = value;
                double dist = (mRnd.NextDouble() - 0.5) * 2.0;
                double absRange = 1.0 / Math.Sqrt(networkDimension);
                this.Weight = (float) (dist * absRange);
            }
        }

        #endregion
    }
}
