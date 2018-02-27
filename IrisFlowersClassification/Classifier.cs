using AForge.Neuro;
using AForge.Neuro.Learning;
using System.IO;
using System.Linq;
using TrainingOrchestrator;

namespace IrisFlowersClassification
{
    public class Classifier : SupervisedClassifier
    {
        private readonly static ActivationNetwork neuralNetwork = new ActivationNetwork(new SigmoidFunction(2), 4, 10, 3);
        private readonly static BackPropagationLearning teacher = new BackPropagationLearning(neuralNetwork) { LearningRate = 0.1, Momentum = 0.01 };

        public void Train(double[][] trainInput, double[][] trainOutput)
        {
            teacher.RunEpoch(trainInput, trainOutput);
        }

        private double[] Normalize(double[] result)
        {
            var sum = result.Sum();

            for (var i = 0; i < result.Length; i++)
            {
                result[i] /= sum;
            }

            return result;
        }

        public double[] Classify(double[] validateInput)
        {
            var result = neuralNetwork.Compute(validateInput);
            var normalizedResult = Normalize(result);
            return normalizedResult;
        }

        public void Save()
        {
            using (var stream = new FileStream("BinaryFile.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                neuralNetwork.Save(stream);
            }
        }
    }
}
