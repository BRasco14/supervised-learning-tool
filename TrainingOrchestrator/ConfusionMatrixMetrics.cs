using Accord.Math;

namespace TrainingOrchestrator
{
    public class ConfusionMatrixMetrics
    {
        public int GetTruePositives(int[,] confusionMatrix, int classWanted)
        {
            var truePositivesArray = confusionMatrix.Diagonal();

            return truePositivesArray[classWanted - 1];
        }

        public int GetFalseNegatives(int[,] confusionMatrix, int classWanted)
        {
            var matrixLength = confusionMatrix.GetLength(0);
            var falseNegativesForClassWanted = 0;

            for (var j = 0; j < matrixLength; j++)
            {
                falseNegativesForClassWanted += confusionMatrix[classWanted - 1, j];
            }

            falseNegativesForClassWanted -= confusionMatrix[classWanted - 1, classWanted - 1];

            return falseNegativesForClassWanted;
        }

        public int GetFalsePositives(int[,] confusionMatrix, int classWanted)
        {
            var matrixLength = confusionMatrix.GetLength(0);
            var falsePositivesForClassWanted = 0;

            for (var j = 0; j < matrixLength; j++)
            {
                falsePositivesForClassWanted += confusionMatrix[j, classWanted - 1];
            }

            falsePositivesForClassWanted -= confusionMatrix[classWanted - 1, classWanted - 1];

            return falsePositivesForClassWanted;
        }

        public int GetTrueNegatives(int[,] confusionMatrix, int classWanted)
        {
            var matrixLength = confusionMatrix.GetLength(0);
            var matrixSum = confusionMatrix.Sum();
            var unwantedSum = 0;

            for (var i = 0; i < matrixLength; i++)
            {
                unwantedSum += confusionMatrix[classWanted - 1, i] + confusionMatrix[i, classWanted - 1];
            }
            unwantedSum -= confusionMatrix[classWanted - 1, classWanted - 1];

            var trueNegatives = matrixSum - unwantedSum;

            return trueNegatives;
        }
    }
}
