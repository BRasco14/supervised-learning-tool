namespace TrainingOrchestrator
{
    public class ClassifiedOutputData
    {
        readonly public double[][] ValidateInput;
        readonly public double[] ValidateOutput;
        public ClassifiedOutputData(double[][] validateInput, double[] validateOutput)
        {
            this.ValidateInput = validateInput;
            this.ValidateOutput = validateOutput;
        }
    }
}
