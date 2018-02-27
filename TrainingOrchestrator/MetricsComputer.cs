using TrainingOrchestrator;

namespace IrisFlowersClassification
{
    public class MetricsComputer
    {
        private readonly ConfusionMatrix confusionMatrix;
        private readonly static ConfusionMatrixMetrics confusionMatrixMetrics = new ConfusionMatrixMetrics();

        public MetricsComputer(ConfusionMatrix confusionMatrix)
        {
            this.confusionMatrix = confusionMatrix;
        }
        public struct Results
        {
            public double Accuracy;
            public double Sensitivity;
            public double Specificity;
            public double Precision;
            public Results(double accuracy, double sensitivity, double specificity, double precision)
            {
                this.Accuracy = accuracy;
                this.Sensitivity = sensitivity;
                this.Specificity = specificity;
                this.Precision = precision;
            }
        }

        private struct Metrics
        {
            public int TruePositives;
            public int FalseNegatives;
            public int FalsePositives;
            public int TrueNegatives;
            public Metrics(int[,] confusionMatrix, int classWanted)
            {
                this.TruePositives = confusionMatrixMetrics.GetTruePositives(confusionMatrix, classWanted);
                this.FalseNegatives = confusionMatrixMetrics.GetFalseNegatives(confusionMatrix, classWanted);
                this.FalsePositives = confusionMatrixMetrics.GetFalsePositives(confusionMatrix, classWanted);
                this.TrueNegatives = confusionMatrixMetrics.GetTrueNegatives(confusionMatrix, classWanted);
            }
        }

        public Results ViewSpecifiedMetrics(int classWanted, int classes)
        {
            var matrix = confusionMatrix.GetConfusionMatrix(classes);
            var metrics = new Metrics(matrix, classWanted);
            var results = ComputeSpecifiedResults(metrics);

            return results;
        }

        public Results ComputeOverallResults(int classes)
        {
            var matrix = confusionMatrix.GetConfusionMatrix(classes);
            var overallResults = new Results();

            for (var i = 1; i <= classes; i++)
            {
                var metrics = new Metrics(matrix, i);
                var oneClassResults = ComputeSpecifiedResults(metrics);
                overallResults.Accuracy += oneClassResults.Accuracy / classes;
                overallResults.Precision += oneClassResults.Precision / classes;
                overallResults.Sensitivity += oneClassResults.Sensitivity / classes;
                overallResults.Specificity += oneClassResults.Specificity / classes;
            }

            return overallResults;
        }

        private static Results ComputeSpecifiedResults(Metrics metrics)
        {
            var accuracy = (metrics.TruePositives + metrics.TrueNegatives) / (double)(metrics.TruePositives + metrics.TrueNegatives + metrics.FalseNegatives + metrics.FalsePositives);
            var sensitivity = metrics.TruePositives / (double)(metrics.TruePositives + metrics.FalseNegatives);
            var specificity = metrics.TrueNegatives / (double)(metrics.TrueNegatives + metrics.FalsePositives);
            var precision = metrics.TruePositives / (double)(metrics.TruePositives + metrics.FalsePositives);

            return new Results(accuracy, sensitivity, specificity, precision);
        }
    }
}
