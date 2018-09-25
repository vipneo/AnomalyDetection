using AnomalyDetection.Models;
using Autofac;
using Serilog;
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
        private readonly ILogger log;

        public OutlierRecordFilter(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<ParsedInputDataRecord> FilterRecords(IEnumerable<ParsedInputDataRecord> records, double median, double outlierPercentage)
        {
            var lowerBound = GetLowerBound(median, outlierPercentage);
            var upperBound = GetUpperBound(median, outlierPercentage);

            var filteredRecords = records.Where(r => IsOutlierValue(r.Value, lowerBound, upperBound));

            log.Debug("Found {@OutlierRecordCount} outliers in {@RecordCount} records with Median: {@Median}, OutlierPercentage: {@OutlierPercentage}, LowerBound: {@LowerBound}, UpperBound: {@UpperBound}", filteredRecords.Count(), records.Count(), median, outlierPercentage, lowerBound, upperBound);

            return filteredRecords;
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

            var isOutlier = isOutOfBounds && isNotEqualToBothBounds;

            log.Verbose("Checking if value {@Value} is Outlier. With LowerBound {@LowerBound} and UpperBound {@UpperBound}, Result {@IsOutlier}", value, lowerBound, upperBound, isOutlier);
                
            return isOutlier;
        }
    }
}