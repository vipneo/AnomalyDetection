using AnomalyDetection.Models;
using Autofac;
using System.Collections.Generic;
using System.Linq;

namespace AnomalyDetection.Algorithm
{
    public interface IOutlierRecordFilter
    {
        IEnumerable<ParsedInputDataRecord> FilterRecords(IEnumerable<ParsedInputDataRecord> records, double median, double outlierPercentage);
    }

    public class OutlierRecordFilter : IOutlierRecordFilter
    {
        public IEnumerable<ParsedInputDataRecord> FilterRecords(IEnumerable<ParsedInputDataRecord> records, double median, double outlierPercentage)
        {
            var lowerBound = GetLowerBound(median, outlierPercentage);
            var upperBound = GetUpperBound(median, outlierPercentage);

            return records.Where(r => IsOutlierValue(r.Value, lowerBound, upperBound));
        }

        public double GetLowerBound(double median, double outlierPercentage)
        {
            return median * (1.0 - outlierPercentage);
        }

        public double GetUpperBound(double median, double outlierPercentage)
        {
            return median * (1.0 + outlierPercentage);
        }

        public bool IsOutlierValue(double value, double lowerBound, double upperBound)
        {
            var isLowerThanLowerBound = value <= lowerBound;
            var isAboveThanUpperBound = value >= upperBound;
            var isOutOfBounds = isLowerThanLowerBound || isAboveThanUpperBound;
            var isNotEqualToBothBounds = value != lowerBound || value != upperBound;

            return isOutOfBounds && isNotEqualToBothBounds;
        }
    }
}