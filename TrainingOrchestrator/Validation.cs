using System;
using System.Linq;

namespace TrainingOrchestrator
{
    public class Validation
    {
        private readonly SupervisedClassifier Classifier;

        public Validation(SupervisedClassifier classifier)
        {
            this.Classifier = classifier;
        }
        public double ValidateWithMse(double[][] validateInput, double[] validateOutput)
        {
            var sumSquaredError = 0.0;

            for (var i = 0; i < validateInput.Length; i++)
            {
                var result = Classifier.Classify(validateInput[i]);

                var finalResult = result.Max();

                for (var j = 0; j < result.Length; j++)
                {
                    if (Math.Abs(result[j] - finalResult) < 0.000001 && Math.Abs(validateOutput[i] - (result.Length - j)) < 0.000001)
                    {
                        sumSquaredError += Math.Pow((1 - finalResult), 2);
                        break;
                    }
                    else if (j == result.Length - 1)
                    {
                        sumSquaredError += 1;
                    }
                }
            }
            return sumSquaredError / validateInput.Length;
        }

        public double ValidateWithLogLoss(double[][] validateInput, double[] validateOutput)
        {
            var logLoss = 0.0;
            var probability = 0.0;

            for (var i = 0; i < validateInput.Length; i++)
            {
                var result = Classifier.Classify(validateInput[i]);

                for (var j = 0; j < result.Length; j++)
                {
                    if (Math.Abs(validateOutput[i] - (j + 1)) < 0.000001)
                    {
                        probability = result[result.Length - (j + 1)];
                    }
                }

                logLoss += -1 / (double)validateInput.Length * Math.Log(probability);
            }
            return logLoss;
        }
    }
}
