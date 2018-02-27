using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TrainingOrchestrator;

namespace SupervisedLearningTool
{
    public class DatasetPreparator : DataPreparator
    {
        private readonly static Random random = new Random();

        public readonly List<double[]> trainInput = new List<double[]>();
        public readonly List<double[]> trainOutput = new List<double[]>();
        public readonly List<double[]> validateInput = new List<double[]>();
        public readonly List<double> validateOutput = new List<double>();
        public readonly List<double[]> testInput = new List<double[]>();
        public readonly List<double> testOutput = new List<double>();

        public double[][] GetTrainInput()
        {
            return trainInput.ToArray();
        }
        public double[][] GetTrainOutput()
        {
            return trainOutput.ToArray();
        }
        public double[][] GetValidateInput()
        {
            return validateInput.ToArray();
        }
        public double[] GetValidateOutput()
        {
            return validateOutput.ToArray();
        }
        public double[][] GetTestInput()
        {
            return testInput.ToArray();
        }
        public double[] GetTestOutput()
        {
            return testOutput.ToArray();
        }

        public DatasetPreparator()
        {
            var lines = FillLines();
            var randomLines = RandomizeLines(lines);
            var trainValues = LoadTrainingData(randomLines.TrainingData);
            var validateValues = LoadProcessedData(randomLines.ValidationData);
            var testValues = LoadProcessedData(randomLines.TestData);
            this.trainInput = trainValues.TrainInput.ToList();
            this.trainOutput = trainValues.TrainOutput.ToList();
            this.validateInput = validateValues.ValidateInput.ToList();
            this.validateOutput = validateValues.ValidateOutput.ToList();
            this.testInput = testValues.ValidateInput.ToList();
            this.testOutput = testValues.ValidateOutput.ToList();
        }

        public static string[] FillLines()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("IrisFlowersClassification.totalData.txt"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public ShuffledDataset RandomizeLines(string[] lines)
        {
            var list = new List<KeyValuePair<int, string>>();

            foreach (var s in lines)
            {
                list.Add(new KeyValuePair<int, string>(random.Next(), s));
            }

            var sorted = list.OrderBy(item => item.Key).ToList();

            var result = new string[lines.Length];

            for (var i = 0; i < lines.Length; i++)
            {
                result[i] = sorted[i].Value;
            }

            var trainLines = Convert.ToInt32(6.0 / 10.0 * lines.Length);
            var validationLines = Convert.ToInt32(1.0 / 10.0 * lines.Length);

            var trainingData = result.Take(trainLines).ToArray();
            var validationData = result.Skip(trainLines).Take(validationLines).ToArray();
            var testData = result.Skip(trainLines + validationLines).ToArray();

            return new ShuffledDataset(trainingData, validationData, testData);
        }

        public ClassifiedInputData LoadTrainingData(string[] data)
        {
            var inputValues = new List<double[]>();
            var outputValues = new List<double[]>();

            for (var i = 0; i < data.Length; i++)
            {
                var fields = data[i].Split('\t');

                var inputArray = new double[4];
                inputValues.Add(inputArray);

                for (var j = 0; j < 4; j++)
                {
                    inputValues[i][j] = double.Parse(fields[j]);
                }

                if ((Math.Abs(double.Parse(fields[4]) - 1)) < 0.000001)
                {
                    var outputArray = new double[3] { 0, 0, 1 };
                    outputValues.Add(outputArray);
                }
                else if ((Math.Abs(double.Parse(fields[4]) - 2)) < 0.000001)
                {
                    var outputArray = new double[3] { 0, 1, 0 };
                    outputValues.Add(outputArray);
                }
                else
                {
                    var outputArray = new double[3] { 1, 0, 0 };
                    outputValues.Add(outputArray);
                }
            }
            return new ClassifiedInputData(inputValues.ToArray(), outputValues.ToArray());
        }

        public ClassifiedOutputData LoadProcessedData(string[] data)
        {
            var inputValues = new List<double[]>();
            var outputValues = new List<double>();

            for (var i = 0; i < data.Length; i++)
            {
                var fields = data[i].Split('\t');

                var inputArray = new double[4];
                inputValues.Add(inputArray);

                var outputField = int.Parse(fields[4]);
                outputValues.Add(outputField);

                for (var j = 0; j < 4; j++)
                {
                    inputValues[i][j] = double.Parse(fields[j]);
                }
            }
            return new ClassifiedOutputData(inputValues.ToArray(), outputValues.ToArray());
        }
    }
}
