namespace TrainingOrchestrator
{
    public class ShuffledDataset
    {
        readonly public string[] TrainingData;
        readonly public string[] ValidationData;
        readonly public string[] TestData;
        public ShuffledDataset(string[] trainingLines, string[] validationLines, string[] testLines)
        {
            this.TrainingData = trainingLines;
            this.ValidationData = validationLines;
            this.TestData = testLines;
        }
    }
}