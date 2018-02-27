using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrainingOrchestrator
{
    public class Orchestrator
    {
        private readonly DataPreparator DataPreparator;
        private readonly SupervisedClassifier Classifier;
        private readonly Validation Validation;

        private readonly List<double> MseList = new List<double>();
        private readonly List<double> LogLossList = new List<double>();

        public Orchestrator(DataPreparator dataPreaparator, SupervisedClassifier classifier, Validation validation)
        {
            this.Classifier = classifier;
            this.DataPreparator = dataPreaparator;
            this.Validation = validation;
        }

        public void WriteErrorsIntoExcel()
        {
            const string fileName = "MseFile.xlsx";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var file = new FileInfo(fileName);

            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("Mse/LogLoss List");

                workSheet.Cells[1, 1].Value = "Mse for each epoch";
                workSheet.Cells[1, 2].Value = "LogLoss for each epoch";

                for (var i = 0; i < MseList.Count; i++)
                {
                    workSheet.Cells[i + 2, 1].Value = MseList[i];
                }

                for (var i = 0; i < LogLossList.Count; i++)
                {
                    workSheet.Cells[i + 2, 2].Value = LogLossList[i];
                }

                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                package.Save();
            }
        }

        public void Train()
        {
            var trainInput = DataPreparator.GetTrainInput();
            var trainOutput = DataPreparator.GetTrainOutput();
            var validateInput = DataPreparator.GetValidateInput();
            var validateOutput = DataPreparator.GetValidateOutput();

            var lastIndex = 0;

            for (var i = 0; i < 9999; i++)
            {
                Classifier.Train(trainInput, trainOutput);
                var mse = Validation.ValidateWithMse(validateInput, validateOutput);
                var logLoss = Validation.ValidateWithLogLoss(validateInput, validateOutput);

                LogLossList.Add(logLoss);
                MseList.Add(mse);

                if (mse < 0.01 || logLoss < 0.01)
                {
                    Classifier.Save();
                    break;
                }
                else if (i - lastIndex > 500)
                {
                    break;
                }
                else if (mse <= MseList.Min())
                {
                    Classifier.Save();
                    lastIndex = i;
                }
            }
        }
    }
}