namespace TrainingOrchestrator
{
    public interface DataPreparator
    {
        double[][] GetTrainInput();
        double[][] GetTrainOutput();
        double[][] GetValidateInput();
        double[] GetValidateOutput();
        double[][] GetTestInput();
        double[] GetTestOutput();
    }
}
