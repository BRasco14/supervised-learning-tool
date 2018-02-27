using Accord;
using Accord.Math;
using System;

namespace TrainingOrchestrator
{
    public class ConfusionMatrix
    {
        private readonly SupervisedClassifier classifier;
        private readonly DataPreparator dataPreparator;
        public ConfusionMatrix(SupervisedClassifier classifier, DataPreparator dataPreparator)
        {
            this.classifier = classifier;
            this.dataPreparator = dataPreparator;
        }
        public int[,] GetConfusionMatrix(int classes)
        {
            var expected = GetExpectedValues();
            var predicted = GetPredictedValues();

            if (expected.Length != predicted.Length)
                throw new DimensionMismatchException("predicted",
                    "The number of expected and predicted observations must match.");

            var confusionMatrix = new int[classes, classes];

            for (var k = 0; k < expected.Length; k++)
            {
                var i = expected[k];
                var j = predicted[k];

                if (i < 0 || i >= classes)
                    throw new ArgumentOutOfRangeException("expected");

                if (j < 0 || j >= classes)
                    throw new ArgumentOutOfRangeException("predicted");

                confusionMatrix[i, j]++;
            }
            return confusionMatrix;
        }

        private int[] GetExpectedValues()
        {
            var testInput = dataPreparator.GetTestInput();
            var testOutput = dataPreparator.GetTestOutput();

            var expected = new int[testOutput.Length];

            for (var i = 0; i < testOutput.Length; i++)
            {
                expected[i] = Convert.ToInt32(testOutput[i] - 1);
            }
            return expected;
        }

        private int[] GetPredictedValues()
        {
            var testInput = dataPreparator.GetTestInput();
            var testOutput = dataPreparator.GetTestOutput();

            var predicted = new int[testOutput.Length];

            for (var i = 0; i < testInput.Length; i++)
            {
                var result = classifier.Classify(testInput[i]);
                var maxResult = result.Max();
                var indexOfMax = result.IndexOf(maxResult);

                for (var j = 0; j < result.Length; j++)
                {
                    if (indexOfMax == j)
                    {
                        predicted[i] = result.Length - j - 1;
                    }
                }
            }
            return predicted;
        }
    }
}
