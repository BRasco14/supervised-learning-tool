namespace TrainingOrchestrator
{
    public class ClassifiedInputData
    {
        readonly public double[][] TrainInput;
        readonly public double[][] TrainOutput;
        public ClassifiedInputData(double[][] trainInput, double[][] trainOutput)
        {
            this.TrainInput = trainInput;
            this.TrainOutput = trainOutput;
        }
    }
}
