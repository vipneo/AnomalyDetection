namespace AnomalyDetection.Settings
{
    public interface IOutlierPercentageSetting
    {
        double OutlierPercentage { get; }
    }

    public class OutlierPercentageSetting : IOutlierPercentageSetting
    {
        public double OutlierPercentage => 0.2;
    }
}
