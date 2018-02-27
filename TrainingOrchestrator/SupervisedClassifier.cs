namespace TrainingOrchestrator
{
    public interface SupervisedClassifier
    {
        void Train(double[][] trainInput, double[][] trainOutput);
        double[] Classify(double[] validateInput);
        void Save();
    }
}
