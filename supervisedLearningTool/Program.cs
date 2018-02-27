using System;
using IrisFlowersClassification;
using TrainingOrchestrator;

namespace SupervisedLearningTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            var datasetPreparator = new DatasetPreparator();

            var classifier = new Classifier();

            var validator = new Validation(classifier);

            var orchestrator = new Orchestrator(datasetPreparator, classifier, validator);

            orchestrator.Train();

            orchestrator.WriteErrorsIntoExcel();



            var confusionMatrix = new ConfusionMatrix(classifier, datasetPreparator);
            var metricsComputer = new MetricsComputer(confusionMatrix);

            var results = metricsComputer.ComputeOverallResults(3);
            //var results = metricsComputer.ViewSpecifiedMetrics(2, 3);

            Console.WriteLine("Classification accuracy rate is {0}% ", 100.0 * results.Accuracy);
            Console.WriteLine("Classification sensitivity rate is {0}% ", 100.0 * results.Sensitivity);
            Console.WriteLine("Classification specificity rate is {0}% ", 100.0 * results.Specificity);
            Console.WriteLine("Classification precision rate is {0}% ", 100.0 * results.Precision);
            Console.ReadLine();
        }
    }
}
