using AnomalyDetection.Algorithm;
using AnomalyDetection.FileParsers;
using AnomalyDetection.Models;
using AnomalyDetection.Settings;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private readonly ILogger log;

        public FileProcessor(
            IOutlierRecordFilter outlierRecordFilter,
            IStreamingMedianCalculator streamingMedianCalculator,
            IEnumerable<IFileParser> fileReaders,
            IEnumerable<IResultsOutputter> resultOutputters,
            IOutlierPercentageSetting outlierPercentageSetting,
            ILogger log)
        {
            this.outlierRecordFilter = outlierRecordFilter;
            this.streamingMedianCalculator = streamingMedianCalculator;
            this.fileReaders = fileReaders;
            this.resultOutputters = resultOutputters;
            this.outlierPercentageSetting = outlierPercentageSetting;
            this.log = log;
        }

        public void ProcessFile(FileInfo file)
        {
            var hasBeenRead = false;

            foreach (var reader in fileReaders)
            {
                if (reader.CanReadFile(file))
                {
                    hasBeenRead = true;
                    ProcessFileWithReader(file, reader);
                    continue;
                }
            }

            if (!hasBeenRead)
                log.Debug("{@Filename} could not be read by any of the registered IFileParser's, skipping file processing", file.Name);
        }

        public void ProcessFileWithReader(FileInfo file, IFileParser reader)
        {
            log.Information("Reading {@Filename} with {@IFileParserType}", file.Name, reader.GetType().Name);

            var records = reader.ReadAndParse(file.OpenText());
            log.Information("Successfully read {@Filename}", file.Name);

            var median = CalculateMedian(records);
            log.Information("{@Filename} has {@RecordCount} Records, with Median of {@Median}", file.Name, records.Count(), median);

            var outliers = GetOutlierRecords(records, median);
            log.Information("{@Filename} has {@OutlierRecordCount} Outliers out of {@RecordCount} Records", file.Name, outliers.Count(), records.Count());

            OutputOutliers(file.Name, median, outliers);
            log.Information("Output outlier records for {@Filename}", file.Name);
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