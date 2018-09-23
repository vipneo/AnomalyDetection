using AnomalyDetection.Algorithm;
using AnomalyDetection.FileParsers;
using AnomalyDetection.Models;
using AnomalyDetection.Settings;
using System.Collections.Generic;
using System.IO;

namespace AnomalyDetection.IO
{
    public interface IFileProcessor
    {
        void ProcessFile(FileInfo file);
    }

    public class FileProcessor : IFileProcessor
    {
        private readonly IOutlierRecordFilter outlierRecordFilter;
        private readonly IStreamingMedianCalculator streamingMedianCalculator;
        private readonly IEnumerable<IFileParser> fileReaders;
        private readonly IEnumerable<IResultsOutputter> resultOutputters;
        private readonly IOutlierPercentageSetting outlierPercentageSetting;

        public FileProcessor(
            IOutlierRecordFilter outlierRecordFilter, 
            IStreamingMedianCalculator streamingMedianCalculator,
            IEnumerable<IFileParser> fileReaders,
            IEnumerable<IResultsOutputter> resultOutputters,
            IOutlierPercentageSetting outlierPercentageSetting)
        {
            this.outlierRecordFilter = outlierRecordFilter;
            this.streamingMedianCalculator = streamingMedianCalculator;
            this.fileReaders = fileReaders;
            this.resultOutputters = resultOutputters;
            this.outlierPercentageSetting = outlierPercentageSetting;
        }

        public void ProcessFile(FileInfo file)
        {
            foreach (var reader in fileReaders)
            {
                if (reader.CanReadFile(file))
                {
                    ProcessFileWithReader(file, reader);
                    return;
                }
            }
        }

        public void ProcessFileWithReader(FileInfo file, IFileParser reader)
        {
            var records = reader.ReadAndParse(file.OpenText());

            var median = CalculateMedian(records);
            var outliers = GetOutlierRecords(records, median);

            OutputOutliers(file.Name, median, outliers);
        }

        private double CalculateMedian(IEnumerable<ParsedInputDataRecord> records)
        {
            foreach (var r in records)
            {
                streamingMedianCalculator.Push(r.Value);
            }

            return streamingMedianCalculator.Median();
        }

        private IEnumerable<ParsedInputDataRecord> GetOutlierRecords(IEnumerable<ParsedInputDataRecord> records, double median)
        {
            return outlierRecordFilter.FilterRecords(records, median, outlierPercentageSetting.OutlierPercentage);
        }

        private void OutputOutliers(string fileName, double median, IEnumerable<ParsedInputDataRecord> outliers)
        {
            foreach (var outputter in resultOutputters)
            {
                outputter.OutputResults(fileName, median, outliers);
            }
        }
    }
}